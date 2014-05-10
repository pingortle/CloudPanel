//
// Copyright (c) 2014, Jacob Dixon
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 3. All advertising materials mentioning features or use of this software
//    must display the following acknowledgement:
//    This product includes software developed by KnowMoreIT and Compsys.
// 4. Neither the name of KnowMoreIT and Compsys nor the
//    names of its contributors may be used to endorse or promote products
//    derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY Jacob Dixon ''AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL Jacob Dixon BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

using CloudPanel.Modules.Base.Companies;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Security.Principal;
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
        /// Creates a new company organizational unit
        /// </summary>
        /// <param name="hostingOrganizationalUnit"></param>
        /// <param name="company"></param>
        /// <returns></returns>
        public string CreateCompany(string parentOrganizationalUnit, CompanyObject company)
        {
            DirectoryEntry de = null;
            DirectoryEntry newOrg = null;

            try
            {
                this.logger.Debug("Attempting to create new organizational unit for company " + company.CompanyName + " on path " + parentOrganizationalUnit);

                de = new DirectoryEntry("LDAP://" + this.domainController + "/" + parentOrganizationalUnit, this.username, this.password);

                // Add organizational unit
                newOrg = de.Children.Add("OU=" + company.CompanyCode, "OrganizationalUnit");

                // Set additional information
                newOrg.Properties["description"].Add(company.CompanyName);
                newOrg.Properties["displayName"].Add(company.CompanyName);
                newOrg.Properties["uPNSuffixes"].Add(company.Domains[0]);

                // These values may not be set so only set if they are valid
                if (!string.IsNullOrEmpty(company.Street))
                    newOrg.Properties["street"].Add(company.Street);

                if (!string.IsNullOrEmpty(company.City))
                    newOrg.Properties["l"].Add(company.City);

                if (!string.IsNullOrEmpty(company.State))
                    newOrg.Properties["st"].Add(company.State);

                if (!string.IsNullOrEmpty(company.ZipCode))
                    newOrg.Properties["postalCode"].Add(company.ZipCode);

                if (!string.IsNullOrEmpty(company.Telephone))
                    newOrg.Properties["telephoneNumber"].Add(company.Telephone);

                if (!string.IsNullOrEmpty(company.AdminName))
                    newOrg.Properties["adminDisplayName"].Add(company.AdminName);

                if (!string.IsNullOrEmpty(company.AdminEmail))
                    newOrg.Properties["adminDescription"].Add(company.AdminEmail);

                // Commit all the changes to the new OU
                newOrg.CommitChanges();
                this.logger.Debug("Committed changes to new organizational unit for company " + company.CompanyName);

                // Commit the changes to the parent OU
                de.CommitChanges();
                this.logger.Info("Finished creating new organizational unit for company " + company.CompanyName);

                // Return the distinguished name
                return newOrg.Properties["distinguishedName"][0].ToString();
            }
            catch (Exception ex)
            {
                this.logger.Error("There was an error creating a new organizational unit for company " + company.CompanyName, ex);

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
        /// Creates a new organizational unit
        /// </summary>
        /// <param name="parentOrganizationalUnit"></param>
        /// <param name="organizationalUnitName"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public string CreateOU(string parentOrganizationalUnit, string organizationalUnitName, string domain)
        {
            DirectoryEntry de = null;
            DirectoryEntry newOrg = null;

            try
            {
                this.logger.Debug("Attempting to create new organizational unit " + organizationalUnitName);

                de = new DirectoryEntry("LDAP://" + this.domainController + "/" + parentOrganizationalUnit, this.username, this.password);

                // Add organizational unit
                newOrg = de.Children.Add("OU=" + organizationalUnitName, "OrganizationalUnit");

                // Set additional information
                if (!string.IsNullOrEmpty(domain))
                    newOrg.Properties["uPNSuffixes"].Add(domain);

                // Commit all the changes to the new OU
                newOrg.CommitChanges();
                this.logger.Debug("Committed changes to new organizational unit " + newOrg.Properties["distinguishedName"][0].ToString());

                // Commit the changes to the parent OU
                de.CommitChanges();
                this.logger.Info("Finished creating new organizational unit " + newOrg.Properties["distinguishedName"][0].ToString());

                // Return the distinguished name
                return newOrg.Properties["distinguishedName"][0].ToString();
            }
            catch (Exception ex)
            {
                this.logger.Error("There was an error creating a new organizational unit " + organizationalUnitName + " in path " + parentOrganizationalUnit, ex);

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
        /// Deletes an OU from Active Directory
        /// </summary>
        /// <param name="distinguishedName"></param>
        public void DeleteOU(string distinguishedName)
        {
            DirectoryEntry de = null;

            try
            {
                this.logger.Debug("Deleting organizational unit " + distinguishedName);

                de = new DirectoryEntry("LDAP://" + this.domainController + "/" + distinguishedName, this.username, this.password);
                de.DeleteTree();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
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

        /// <summary>
        /// Removes Authenticated Users from the OU
        /// </summary>
        /// <param name="oupath"></param>
        public void RemoveAuthUsersRights(string oupath)
        {
            using (DirectoryEntry de = new DirectoryEntry("LDAP://" + this.domainController + "/" + oupath, this.username, this.password))
            {
                try
                {
                    AuthorizationRuleCollection arc = de.ObjectSecurity.GetAccessRules(true, true, typeof(NTAccount));
                    foreach (ActiveDirectoryAccessRule adar in arc)
                    {
                        if (adar.IdentityReference.Value.Equals("NT AUTHORITY\\AUTHENTICATED USERS", StringComparison.CurrentCultureIgnoreCase))
                        {
                            bool modified = false;
                            de.ObjectSecurity.ModifyAccessRule(AccessControlModification.RemoveAll, adar, out modified);
                            de.CommitChanges();
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.logger.Error("Failed to remove authenticated users access rights from " + oupath, ex);
                    throw;
                }
            }
        }

        /// <summary>
        /// Adds a new domain to the company OU and all child OU's
        /// </summary>
        /// <param name="companyOUPath"></param>
        /// <param name="domainName"></param>
        public void AddDomain(string companyOUPath, string domainName)
        {
            DirectoryEntry de = null;
            DirectorySearcher ds = null;

            try
            {
                this.logger.Debug("Attempting to add domain to all organizational units for company on path " + companyOUPath);

                de = new DirectoryEntry("LDAP://" + this.domainController + "/" + companyOUPath, this.username, this.password);

                // Add domain to all children and the base
                ds = new DirectorySearcher(de);
                ds.Filter = "(objectClass=organizationalUnit)";
                ds.SearchScope = SearchScope.Subtree;

                foreach (SearchResult sr in ds.FindAll())
                {
                    DirectoryEntry tmp = sr.GetDirectoryEntry();

                    // Make sure it doesn't already exists
                    bool alreadyExists = false;
                    foreach (string upn in tmp.Properties["uPNSuffixes"])
                    {
                        if (upn.Equals(domainName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            alreadyExists = true;
                            break;
                        }
                    }

                    if (!alreadyExists)
                    {
                        tmp.Properties["uPNSuffixes"].Add(domainName);
                        tmp.CommitChanges();
                        this.logger.Info("Added new domain to organiational unit " + tmp.Path);
                    }
                }

                de.CommitChanges();
            }
            catch (Exception ex)
            {
                this.logger.Error("There was an error adding the domain "+ domainName + " to the path (and all child org units) " + companyOUPath, ex);

                throw;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();

                if (de != null)
                    de.Dispose();
            }
        }

        /// <summary>
        /// Removes a domain from the company OU and all child OU's
        /// </summary>
        /// <param name="companyOUPath"></param>
        /// <param name="domainName"></param>
        public void RemoveDomain(string companyOUPath, string domainName)
        {
            DirectoryEntry de = null;
            DirectorySearcher ds = null;

            try
            {
                this.logger.Debug("Attempting to remove domain to all organizational units for company on path " + companyOUPath);

                de = new DirectoryEntry("LDAP://" + this.domainController + "/" + companyOUPath, this.username, this.password);

                // Add domain to all children and the base
                ds = new DirectorySearcher(de);
                ds.Filter = "(objectClass=organizationalUnit)";
                ds.SearchScope = SearchScope.Subtree;

                foreach (SearchResult sr in ds.FindAll())
                {
                    DirectoryEntry tmp = sr.GetDirectoryEntry();

                    // Make sure it doesn't already exists
                    bool alreadyExists = false;
                    foreach (string upn in tmp.Properties["uPNSuffixes"])
                    {
                        if (upn.Equals(domainName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            alreadyExists = true;
                            break;
                        }
                    }

                    if (alreadyExists)
                    {
                        tmp.Properties["uPNSuffixes"].Remove(domainName);
                        tmp.CommitChanges();
                        this.logger.Info("Removed domain " + domainName + " from organiational unit " + tmp.Path);
                    }
                }

                de.CommitChanges();
            }
            catch (Exception ex)
            {
                this.logger.Error("There was an error removing the domain " + domainName + " from the path (and all child org units) " + companyOUPath, ex);

                throw;
            }
            finally
            {
                if (ds != null)
                    ds.Dispose();

                if (de != null)
                    de.Dispose();
            }
        }
    }
}
