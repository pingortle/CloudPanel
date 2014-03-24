using CloudPanel.Modules.ActiveDirectory.Users;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Base.Users;
using CloudPanel.Modules.Common.Database;
using CloudPanel.Modules.Common.Settings;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class LoginViewModel : IViewModel
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UsersObject Authenticate(string username, string password, string ipAddress, bool isLocalRequest)
        {
            ADUser ldap = null;
            CPDatabase database = null;

            try
            {
                // Check if IP address is blocked from brute force
                if (IsBlockedFromBruteForce(ipAddress) && !isLocalRequest)
                {
                    ThrowEvent(AlertID.BRUTE_FORCE_BLOCKED, ipAddress);
                    return null;
                }
                else
                {
                    database = new CPDatabase();

                    // Find the user in SQL first
                    var user = (from d in database.Users
                                where d.UserPrincipalName == username
                                select d).FirstOrDefault();

                    ldap = new ADUser(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);

                    // Authenticate the user
                    UsersObject userObject = ldap.Authenticate(username, password);
                    if (userObject == null)
                    {
                        // Audit the login
                        AuditLogin(username, ipAddress, false);

                        ThrowEvent(AlertID.LOGIN_FAILED, username + " failed to login.");
                        return null;
                    }
                    else
                    {
                        // Audit the login
                        AuditLogin(username, ipAddress, true);

                        // Now check the groups
                        string[] cpGroups = StaticSettings.SuperAdmins.ToLower().Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        // User could be null if it is a domain admin which won't be in the database.
                        if (user != null)
                        {
                            userObject.CompanyCode = user.CompanyCode;
                            userObject.ResellerCode = GetResellerCode(user.CompanyCode);

                            if (user.IsCompanyAdmin != null && (bool)user.IsCompanyAdmin)
                            {
                                userObject.IsCompanyAdmin = true;
                            }

                            if (user.IsResellerAdmin != null && (bool)user.IsResellerAdmin)
                            {
                                userObject.IsResellerAdmin = true;
                            }
                        }

                        // Now check if they are a super admin
                        foreach (string g in cpGroups)
                        {
                            var isFound = userObject.Groups.Where(a => a.ToLower().StartsWith("cn=" + g)).Count();
                            if (isFound > 0)
                            {
                                userObject.IsSuperAdmin = true;
                                break;
                            }
                        }

                        return userObject;
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Error logging in user " + username, ex);
                ThrowEvent(AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();

                if (ldap != null)
                    ldap.Dispose();
            }
        }

        private string GetResellerCode(string usersCompanyCode)
        {
            CPDatabase database = null;
            try
            {
                var resellerCode = (from c in database.Companies
                                    where c.CompanyCode == usersCompanyCode
                                    select c.ResellerCode).FirstOrDefault();

                if (resellerCode == null)
                    throw new Exception("Unable to retrieve the reseller code for company " + usersCompanyCode);
                else
                    return resellerCode;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        private bool IsBlockedFromBruteForce(string ipAddress)
        {
            CPDatabase database = null;

            try
            {
                if (StaticSettings.IPBlockingEnabled)
                {
                    database = new CPDatabase();

                    var isBlocked = from b in database.AuditLogins
                                    where b.IPAddress == ipAddress
                                    where b.AuditTimeStamp >= DbFunctions.AddMinutes(DateTime.Now, -StaticSettings.IPBlockingLockedInMinutes)
                                    where b.LoginStatus == false
                                    orderby b.AuditTimeStamp descending
                                    select b;

                    if (isBlocked == null)
                        return false;
                    else
                    {
                        logger.Debug("Found a total of " + isBlocked.Count() + " entries in the database for IP Address " + ipAddress);

                        if (isBlocked.Count() >= StaticSettings.IPBlockingFailedCount)
                        {
                            ThrowEvent(AlertID.BRUTE_FORCE_BLOCKED, ipAddress + " is blocked due to too many failed login attempts");
                            return true;
                        }
                        else
                            return false;
                    }
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ThrowEvent(AlertID.FAILED, ex.Message);
                return true;
            }
        }

        private void AuditLogin(string username, string ipAddress, bool isValidLogin)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                // Audit login
                AuditLogin audit = new AuditLogin();
                audit.IPAddress = ipAddress;
                audit.Username = username;
                audit.LoginStatus = isValidLogin;
                audit.AuditTimeStamp = DateTime.Now;

                database.AuditLogins.Add(audit);
                database.SaveChanges();

                this.logger.Debug(username + "attempted to login to CloudPanel. Is valid login? " + isValidLogin.ToString());
            }
            catch (Exception ex)
            {
                this.logger.Error("Error adding entry to the login audit table.", ex);

                throw;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }
    }
}
