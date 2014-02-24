using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Common.Codes;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.ActiveDirectory.OrganizationalUnits
{
    public class ADOrganizationalUnit
    {
        // Information for connecting
        private string username;
        private string password;
        private string domainController;

        public ADOrganizationalUnit(string username, string password, string domainController)
        {
            this.username = username;
            this.password = password;
            this.domainController = domainController;
        }

        public string CreateReseller(string hostingOrganizationalUnit, ResellerObject reseller)
        {
            DirectoryEntry de = null;
            DirectoryEntry newOrg = null;

            try
            {
                de = new DirectoryEntry("LDAP://" + this.domainController + "/" + hostingOrganizationalUnit, this.username, this.password);

                // Add organizational unit
                newOrg = de.Children.Add("OU=" + reseller.CompanyCode, "OrganizationalUnit");

                // Set additional information
                newOrg.Properties["description"].Add(reseller.CompanyName);
                newOrg.Properties["displayName"].Add(reseller.CompanyName);

                // These values may not be set so only set if they are valid
                if (!string.IsNullOrEmpty(reseller.Street))
                    newOrg.Properties["street"].Add(reseller.Street);

                if (!string.IsNullOrEmpty(reseller.City))
                    newOrg.Properties["l"].Add(reseller.City);

                if (!string.IsNullOrEmpty(reseller.State))
                    newOrg.Properties["st"].Add(reseller.State);

                if (!string.IsNullOrEmpty(reseller.ZipCode))
                    newOrg.Properties["postalCode"].Add(reseller.ZipCode);

                if (!string.IsNullOrEmpty(reseller.Telephone))
                    newOrg.Properties["telephoneNumber"].Add(reseller.Telephone);

                if (!string.IsNullOrEmpty(reseller.AdminName))
                    newOrg.Properties["adminDisplayName"].Add(reseller.AdminName);

                if (!string.IsNullOrEmpty(reseller.AdminEmail))
                    newOrg.Properties["adminDescription"].Add(reseller.AdminEmail);

                // Commit all the changes to the new OU
                newOrg.CommitChanges();

                // Commit the changes to the parent OU
                de.CommitChanges();

                // Return the distinguished name
                return newOrg.Path;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (newOrg != null)
                    newOrg.Dispose();

                if (de != null)
                    de.Dispose();
            }
        }
    }
}
