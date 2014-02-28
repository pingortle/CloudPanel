using CloudPanel.Modules.Base.Companies;
using log4net;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text;

namespace CloudPanel.Modules.ActiveDirectory.OrganizationalUnits
{
    public class ADOrganizationalUnit
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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

        /// <summary>
        /// Creates a new reseller organizational unit
        /// </summary>
        /// <param name="hostingOrganizationalUnit"></param>
        /// <param name="reseller"></param>
        /// <returns></returns>
        public string CreateReseller(string hostingOrganizationalUnit, ResellerObject reseller)
        {
            DirectoryEntry de = null;
            DirectoryEntry newOrg = null;

            try
            {
                this.logger.Debug("Attempting to create new organizational unit for reseller " + reseller.CompanyName);

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
                this.logger.Debug("Committed changes to new organizational unit for reseller " + reseller.CompanyName);

                // Commit the changes to the parent OU
                de.CommitChanges();
                this.logger.Info("Finished creating new organizational unit for reseller " + reseller.CompanyName);

                // Return the distinguished name
                return newOrg.Properties["distinguishedName"][0].ToString();
            }
            catch (Exception ex)
            {
                this.logger.Error("There was an error creating a new organizational unit for reseller " + reseller.CompanyName, ex);

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

        /// <summary>
        /// Adds access rights to the OU for a specific group
        /// </summary>
        /// <param name="ouDistinguishedName"></param>
        /// <param name="groupName"></param>
        public void AddAccessRights(string ouDistinguishedName, string groupName)
        {
            DirectoryEntry de = null;
            PrincipalContext pc = null;
            GroupPrincipal gp = null;

            try
            {
                de = new DirectoryEntry("LDAP://" + this.domainController + "/" + ouDistinguishedName, this.username, this.password);
                pc = new PrincipalContext(ContextType.Domain, this.domainController, this.username, this.password);

                // Search for the group
                gp = GroupPrincipal.FindByIdentity(pc, IdentityType.Name, groupName.Replace(" ", string.Empty));
                if (gp == null)
                    throw new Exception("Unable to find the group  " + groupName + " in the system.");
                else
                {
                    // Add Read Property
                    de.ObjectSecurity.AddAccessRule(new ActiveDirectoryAccessRule(gp.Sid, ActiveDirectoryRights.ReadProperty, AccessControlType.Allow));

                    // Add List Object
                    de.ObjectSecurity.AddAccessRule(new ActiveDirectoryAccessRule(gp.Sid, ActiveDirectoryRights.ListObject, AccessControlType.Allow));

                    // Deny List Content
                    de.ObjectSecurity.AddAccessRule(new ActiveDirectoryAccessRule(gp.Sid, ActiveDirectoryRights.ListChildren, AccessControlType.Deny));

                    de.CommitChanges();
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to add access rights to organizational unit " + ouDistinguishedName + " for group " + groupName, ex);

                throw;
            }
            finally
            {
                if (gp != null)
                    gp.Dispose();

                if (pc != null)
                    pc.Dispose();

                if (de != null)
                    de.Dispose();
            }
        }
    }
}
