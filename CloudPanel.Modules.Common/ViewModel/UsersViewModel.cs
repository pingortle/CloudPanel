using CloudPanel.Modules.ActiveDirectory.Groups;
using CloudPanel.Modules.ActiveDirectory.Users;
using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Base.Plans;
using CloudPanel.Modules.Base.Users;
using CloudPanel.Modules.Common.Database;
using CloudPanel.Modules.Common.Rollback;
using CloudPanel.Modules.Common.Settings;
using CloudPanel.Modules.Exchange;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class UsersViewModel : IViewModel
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<DomainsObject> GetDomains(string companyCode)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var foundDomains = from d in database.Domains
                                   where d.CompanyCode == companyCode
                                   orderby d.Domain1
                                   select new DomainsObject()
                                   {
                                       DomainID = d.DomainID,
                                       CompanyCode = d.CompanyCode,
                                       DomainName = d.Domain1,
                                       IsSubDomain = d.IsSubDomain == null ? false : (bool)d.IsSubDomain,
                                       IsDefault = d.IsDefault,
                                       IsAcceptedDomain = d.IsAcceptedDomain,
                                       TypeOfDomain= d.DomainType == null ? DomainType.Unknown : (DomainType)d.DomainType
                                   };

                if (foundDomains != null)
                    return foundDomains.ToList();
                else
                    return null;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error trying to retrieve domains for company " + companyCode, ex);
                ThrowEvent(AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public List<UsersObject> GetUsers(string companyCode)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var foundUsers = from u in database.Users
                                  where u.CompanyCode == companyCode
                                  orderby u.DisplayName
                                  select new UsersObject()
                                  {
                                      CompanyCode = u.CompanyCode,
                                      sAMAccountName = u.sAMAccountName,
                                      UserPrincipalName = u.UserPrincipalName,
                                      DisplayName = u.DisplayName,
                                      Department = u.Department,
                                      IsResellerAdmin = u.IsResellerAdmin == null ? false : (bool)u.IsResellerAdmin,
                                      IsCompanyAdmin = u.IsCompanyAdmin == null ? false : (bool)u.IsCompanyAdmin,
                                      MailboxPlan = u.MailboxPlan == null ? 0 : (int)u.MailboxPlan,
                                      LyncPlan = u.LyncPlan == null ? 0 : (int)u.LyncPlan
                                  };

                if (foundUsers != null)
                    return foundUsers.ToList();
                else
                    return null;
            }
            catch(Exception ex)
            {
                this.logger.Error("Failed to retrieve users for company " + companyCode, ex);
                ThrowEvent(AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public UsersObject GetUser(string userPrincipalName)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                // Get from the database
                var findUser = (from u in database.Users
                                from p in database.UserPermissions.Where(m => m.UserID == u.ID).DefaultIfEmpty()
                                where u.UserPrincipalName == userPrincipalName
                                orderby u.DisplayName
                                select new UsersObject()
                                {
                                    UserGuid = u.UserGuid,
                                    CompanyCode = u.CompanyCode,
                                    sAMAccountName = u.sAMAccountName,
                                    UserPrincipalName = u.UserPrincipalName,
                                    Firstname = u.Firstname,
                                    Middlename = u.Middlename,
                                    Lastname = u.Lastname,
                                    DisplayName = u.DisplayName,
                                    Department = u.Department,
                                    DistinguishedName = u.DistinguishedName,
                                    Created = u.Created,
                                    IsCompanyAdmin = u.IsCompanyAdmin == null ? false : (bool)u.IsCompanyAdmin,
                                    IsResellerAdmin = u.IsResellerAdmin == null ? false : (bool)u.IsResellerAdmin,
                                    MailboxPlan = u.MailboxPlan == null ? 0 : (int)u.MailboxPlan,
                                    AdditionalMB = u.AdditionalMB == null ? 0 : (int)u.AdditionalMB,
                                    ActiveSyncPlan = u.ActiveSyncPlan == null ? 0 : (int)u.ActiveSyncPlan,
                                    EnableExchangePerm = p.EnableExchange == null ? false : p.EnableExchange,
                                    DisableExchangePerm = p.DisableExchange == null ? false : p.DisableExchange,
                                    AddDomainPerm = p.AddDomain == null ? false : p.AddDomain,
                                    DeleteDomainPerm = p.DeleteDomain == null ? false : p.DeleteDomain,
                                    EnableAcceptedDomainPerm = p.EnableAcceptedDomain == null ? false : p.EnableAcceptedDomain,
                                    DisableAcceptedDomainPerm = p.DisableAcceptedDomain == null ? false : p.DisableAcceptedDomain
                                }).FirstOrDefault();

                return findUser;               
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to retrieve user " + userPrincipalName, ex);
                ThrowEvent(AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public UsersObject GetUserMailbox(string identity)
        {
            ExchangePowershell powershell = null;

            try
            {
                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);

                UsersObject obj = powershell.GetUser(identity, StaticSettings.ExchangeVersion);
                
                return obj;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to retrieve user mailbox information " + identity, ex);
                ThrowEvent(AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (powershell != null)
                    powershell.Dispose();
            }
        }

        public List<MailboxPlanObject> GetMailboxPlans(string companyCode)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var foundPlans = from p in database.Plans_ExchangeMailbox
                                 where p.CompanyCode == companyCode || string.IsNullOrEmpty(p.CompanyCode)
                                 orderby p.MailboxPlanName
                                 select new MailboxPlanObject()
                                 {
                                     MailboxPlanID = p.MailboxPlanID,
                                     MailboxPlanName = p.MailboxPlanName,
                                     MailboxPlanDescription = p.MailboxPlanDesc,
                                     CompanyCode = p.CompanyCode,
                                     Cost = string.IsNullOrEmpty(p.Cost) ? "0.00" : p.Cost,
                                     Price = string.IsNullOrEmpty(p.Price) ? "0.00": p.Price,
                                     AdditionalGBPrice = string.IsNullOrEmpty(p.AdditionalGBPrice) ? "0.00" : p.AdditionalGBPrice,
                                     MailboxSizeInMB = p.MailboxSizeMB,
                                     MaxMailboxSizeInMB = p.MaxMailboxSizeMB == null ? p.MailboxSizeMB : (int)p.MaxMailboxSizeMB
                                 };

                if (foundPlans != null)
                    return foundPlans.ToList();
                else
                    return null;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error trying to retrieve mailbox plans for company " + companyCode, ex);
                ThrowEvent(AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public byte[] GetPhoto(string userPrincipalName)
        {
            ADUser user = null;

            try
            {
                user = new ADUser(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);

                byte[] data = user.GetPhoto(userPrincipalName);

                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (user != null)
                    user.Dispose();
            }
        }

        public void CreateUser(UsersObject newUser)
        {
            CPDatabase database = null;
            ADGroup ldapGroup = null;
            ADUser ldapUser = null;

            CloudPanelTransaction newUserTransaction = new CloudPanelTransaction();
            try
            {
                // Insert into database
                database = new CPDatabase();

                // Make sure the user doesn't already exist
                var foundUser = (from u in database.Users
                                 where u.UserPrincipalName == newUser.UserPrincipalName
                                 select u).FirstOrDefault();

                if (foundUser != null)
                    ThrowEvent(AlertID.USER_ALREADY_EXISTS, newUser.UserPrincipalName);
                else
                {
                    // Get the company's OU where we need to save the user
                    var companyDistinguishedName = (from c in database.Companies
                                                    where !c.IsReseller
                                                    where c.CompanyCode == newUser.CompanyCode
                                                    select c.DistinguishedName).First();

                    // Check if they are using a custom user's OU
                    if (!string.IsNullOrEmpty(StaticSettings.UsersOU))
                        companyDistinguishedName = string.Format("OU={0},{1}", StaticSettings.UsersOU, companyDistinguishedName);

                    ldapUser = new ADUser(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                    UsersObject createdUser = ldapUser.NewUser(newUser, companyDistinguishedName, StaticSettings.AllowCustomNameAttribute);
                    newUserTransaction.NewUser(createdUser.UserPrincipalName);

                    // Add the users to the groups
                    ldapGroup = new ADGroup(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                    ldapGroup.AddMember("AllUsers@" + newUser.CompanyCode, createdUser.UserPrincipalName, "upn");

                    if (newUser.IsCompanyAdmin)
                        ldapGroup.AddMember("Admins@" + newUser.CompanyCode, createdUser.UserPrincipalName, "upn");

                    // Insert into database
                    User sqlUser = new User();
                    sqlUser.UserGuid = createdUser.UserGuid;
                    sqlUser.CompanyCode = createdUser.CompanyCode;
                    sqlUser.sAMAccountName = createdUser.sAMAccountName;
                    sqlUser.UserPrincipalName = createdUser.UserPrincipalName;
                    sqlUser.DistinguishedName = createdUser.DistinguishedName;
                    sqlUser.DisplayName = createdUser.DisplayName;
                    sqlUser.Firstname = createdUser.Firstname;
                    sqlUser.Middlename = createdUser.Middlename;
                    sqlUser.Lastname = createdUser.Lastname;
                    sqlUser.Email = string.Empty;
                    sqlUser.Department = createdUser.Department;
                    sqlUser.IsResellerAdmin = createdUser.IsResellerAdmin;
                    sqlUser.IsCompanyAdmin = createdUser.IsCompanyAdmin;
                    sqlUser.MailboxPlan = 0;
                    sqlUser.TSPlan = 0;
                    sqlUser.LyncPlan = 0;
                    sqlUser.Created = DateTime.Now;
                    sqlUser.AdditionalMB = 0;
                    sqlUser.ActiveSyncPlan = 0;
                    database.Users.Add(sqlUser);

                    // Insert permissions into database
                    if (createdUser.IsCompanyAdmin)
                    {
                        UserPermission newPermissions = new UserPermission();
                        newPermissions.UserID = sqlUser.ID;
                        newPermissions.EnableExchange = createdUser.EnableExchangePerm;
                        newPermissions.DisableExchange = createdUser.DisableExchangePerm;
                        newPermissions.AddDomain = createdUser.AddDomainPerm;
                        newPermissions.DeleteDomain = createdUser.DeleteDomainPerm;
                        newPermissions.EnableAcceptedDomain = createdUser.EnableAcceptedDomainPerm;
                        newPermissions.DisableAcceptedDomain = createdUser.DisableAcceptedDomainPerm;
                        database.UserPermissions.Add(newPermissions);
                    }

                    database.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ThrowEvent(AlertID.FAILED, ex.Message);

                // Rollback on error
                newUserTransaction.RollBack();
            }
            finally
            {
                if (ldapUser != null)
                    ldapUser.Dispose();

                if (ldapGroup != null)
                    ldapGroup.Dispose();

                if (database != null)
                    database.Dispose();
            }
        }

        public void UpdateUser(UsersObject updateUser, bool isSuperOrResellerAdmin)
        {
            CPDatabase database = null;
            ADGroup ldapGroup = null;
            ADUser ldapUser = null;

            try
            {
                database = new CPDatabase();

                // Get the user from the database
                var foundUser = (from u in database.Users
                                 where u.UserPrincipalName == updateUser.UserPrincipalName
                                 select u).FirstOrDefault();

                if (foundUser == null)
                    ThrowEvent(AlertID.USER_UNKNOWN, updateUser.UserPrincipalName);
                else
                {
                    this.logger.Debug("Found user " + foundUser.UserPrincipalName + " in the database. Continuing...");

                    // Update the user values
                    foundUser.Firstname = updateUser.Firstname;
                    foundUser.Middlename = updateUser.Middlename;
                    foundUser.Lastname = updateUser.Lastname;
                    foundUser.DisplayName = updateUser.DisplayName;
                    foundUser.Department = updateUser.Department;

                    // Update user in Active Directory
                    ldapUser = new ADUser(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                    ldapUser.UpdateUser(updateUser, StaticSettings.AllowCustomNameAttribute);

                    // Only update these values if super admin or reseller admin is modifying the user
                    if (isSuperOrResellerAdmin)
                    {
                        this.logger.Debug("Super admin or reseller is updating user so we can check comapny admin permissions and reseller permissions");

                        foundUser.IsCompanyAdmin = updateUser.IsCompanyAdmin;
                        foundUser.IsResellerAdmin = updateUser.IsResellerAdmin;

                        // Get permissions from database
                        var userPermissions = (from p in database.UserPermissions
                                               where p.UserID == foundUser.ID
                                               select p).FirstOrDefault();


                        // If the user is no longer a company admin then remove permissions from the database
                        if (userPermissions != null && !updateUser.IsCompanyAdmin)
                        {
                            this.logger.Debug("User " + updateUser.UserPrincipalName + " is no longer a comapny admin. Need to remove rights from database and security group");

                            database.UserPermissions.Remove(userPermissions);

                            // Remove from Admins@ security group
                            ldapGroup = new ADGroup(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                            ldapGroup.RemoveMember("Admins@" + updateUser.CompanyCode, updateUser.UserPrincipalName, "upn");
                        }
                        else if (userPermissions != null && updateUser.IsCompanyAdmin)
                        {
                            this.logger.Debug("User " + updateUser.UserPrincipalName + " is a company admin. Need to update company admin rights in database.");

                            // If user permissions was found and the user is company admin then update the values
                            userPermissions.EnableExchange = updateUser.EnableExchangePerm;
                            userPermissions.DisableExchange = updateUser.DisableExchangePerm;
                            userPermissions.AddDomain = updateUser.AddDomainPerm;
                            userPermissions.DeleteDomain = updateUser.DeleteDomainPerm;
                            userPermissions.EnableAcceptedDomain = updateUser.EnableAcceptedDomainPerm;
                            userPermissions.DisableAcceptedDomain = updateUser.DisableAcceptedDomainPerm;
                        }
                        else if (userPermissions == null && updateUser.IsCompanyAdmin)
                        {
                            this.logger.Debug("User " + updateUser.UserPrincipalName + " does not have any existing company admin rights. We need to add them to the database.");

                            // No existing permissions were found and we need to add to database
                            userPermissions = new UserPermission();
                            userPermissions.UserID = foundUser.ID;
                            userPermissions.EnableExchange = updateUser.EnableExchangePerm;
                            userPermissions.DisableExchange = updateUser.DisableExchangePerm;
                            userPermissions.AddDomain = updateUser.AddDomainPerm;
                            userPermissions.DeleteDomain = updateUser.DeleteDomainPerm;
                            userPermissions.EnableAcceptedDomain = updateUser.EnableAcceptedDomainPerm;
                            userPermissions.DisableAcceptedDomain = updateUser.DisableAcceptedDomainPerm;
                            database.UserPermissions.Add(userPermissions);

                            // Add to Admins@ security group
                            ldapGroup = new ADGroup(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                            ldapGroup.AddMember("Admins@" + updateUser.CompanyCode, updateUser.UserPrincipalName, "upn");
                        }
                    }
                    else
                        this.logger.Debug("User making changes to " + updateUser.UserPrincipalName + " is not a super admin or reseller admin. We cannot update company admin or reseller admin permissions unless the user making changes is a super or reseller admin.");

                    // Update database
                    database.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this.logger.Debug("Error updating user " + updateUser.UserPrincipalName, ex);
                ThrowEvent(AlertID.FAILED, ex.Message);
            }
            finally
            {
                if (ldapUser != null)
                    ldapUser.Dispose();

                if (ldapGroup != null)
                    ldapGroup.Dispose();

                if (database != null)
                    database.Dispose();
            }
        }

        public void DeleteUser(string userPrincipalName)
        {
            CPDatabase database = null;
            ADUser ldapUser = null;

            try
            {
                ldapUser = new ADUser(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                ldapUser.DeleteUser(userPrincipalName);

                // Delete from database
                database = new CPDatabase();
                database.DeleteUser(userPrincipalName);
            }
            catch (Exception ex)
            {
                ThrowEvent(AlertID.FAILED, ex.Message);
            }
            finally
            {
                if (ldapUser != null)
                    ldapUser.Dispose();

                if (database != null)
                    database.Dispose();
            }
        }

        public void ResetPassword(string userPrincipalName, string newPassword, string companyCode)
        {
            ADUser user = null;
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var sqlUser = (from u in database.Users
                            where u.UserPrincipalName == userPrincipalName
                            select u).First();

                if (sqlUser.CompanyCode.Equals(companyCode, StringComparison.CurrentCultureIgnoreCase))
                {
                    user = new ADUser(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                    user.ResetPassword(userPrincipalName, newPassword);
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Error resetting password for " + userPrincipalName, ex);
                ThrowEvent(AlertID.FAILED, ex.Message);
            }
            finally
            {
                if (user != null)
                    user.Dispose();
            }
        }
    }
}
