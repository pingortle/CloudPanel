using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Base.Users;
using log4net;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.ActiveDirectory.Users
{
    public class ADUser : IDisposable
    {
        // Disposing information
        private bool disposed = false;

        // Information for connecting
        private string username;
        private string password;
        private string domainController;

        // Logger
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Create a new Users object instance
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="domainController"></param>
        public ADUser(string username, string password, string domainController)
        {
            this.username = username;
            this.password = password;
            this.domainController = domainController;
        }

        
        /// <summary>
        /// Authenticates a user a returns an array of groups they belong to
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="userPassword"></param>
        /// <returns>Array of groups the user belongs to</returns>
        public UsersObject Authenticate(string userName, string userPassword)
        {
            PrincipalContext pc = null;
            UserPrincipal up = null;
            DirectoryEntry de = null;

            try
            {
                pc = new PrincipalContext(ContextType.Domain, this.domainController, this.username, this.password);

                logger.Debug("Attempting to authenticate user " + userName);

                // Try to authenticate
                bool authenticated = pc.ValidateCredentials(userName, userPassword);

                if (authenticated)
                {
                    logger.Debug(userName + " successfully authenticated. Attempting to retrieve groups that the user belongs to.");

                    UsersObject loggedInUser = new UsersObject();

                    up = UserPrincipal.FindByIdentity(pc, IdentityType.UserPrincipalName, userName);
                    de = up.GetUnderlyingObject() as DirectoryEntry;

                    // Set values
                    loggedInUser.UserPrincipalName = up.UserPrincipalName;
                    loggedInUser.DisplayName = up.DisplayName;

                    loggedInUser.Groups = new List<string>();
                    for (int i = 0; i < de.Properties["memberOf"].Count; i++)
                    {
                        loggedInUser.Groups.Add(de.Properties["memberOf"][i].ToString());
                    }

                    logger.Debug(userName + " belongs to the following groups: " + String.Join(", ", loggedInUser.Groups));

                    return loggedInUser;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error authenticating user " + userName, ex);

                throw;
            }
            finally
            {
                if (de != null)
                    de.Dispose();

                if (up != null)
                    up.Dispose();

                if (pc != null)
                    pc.Dispose();
            }
        }

        /// <summary>
        /// Find an availble sAMAccountName
        /// It loops and appends a number to the end of a sAMAccountNAme if the original doesn't exist
        /// </summary>
        /// <param name="userPrincipalName"></param>
        /// <returns></returns>
        private string GetAvailableSamAccountName(string userPrincipalName)
        {
            DirectoryEntry de = null;
            DirectorySearcher ds = null;

            try
            {
                logger.Debug("Attempting to find an available sAMAccountName for " + userPrincipalName);

                // Get the first part of the user principal name
                string upnFirstPart = userPrincipalName.Split('@')[0];
                string sAMAccountName = upnFirstPart;

                de = new DirectoryEntry("LDAP://" + this.domainController, this.username, this.password);
                ds = new DirectorySearcher(de);
                ds.SearchScope = SearchScope.Subtree;
                ds.Filter = string.Format("(&(objectClass=User)(sAMAccountName={0}))", upnFirstPart);

                int count = 0;
                while (ds.FindOne() != null)
                {
                    count++;

                    sAMAccountName = string.Format("{0}{1}", upnFirstPart, count.ToString());

                    ds.Filter = string.Format("(&(objectClass=User)(sAMAccountName={0}))", sAMAccountName);
                }

                // We found our available sAMAccountName
                return sAMAccountName;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving user information " + userPrincipalName, ex);

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
        /// Checks if a userprincipalname exists
        /// </summary>
        /// <param name="userPrincipalName"></param>
        /// <returns></returns>
        private bool DoesUserPrincipalNameExist(string userPrincipalName)
        {
            DirectoryEntry de = null;
            DirectorySearcher ds = null;

            try
            {
                logger.Debug("Attempting to find out if userprincipalname exists " + userPrincipalName);
                
                de = new DirectoryEntry("LDAP://" + this.domainController, this.username, this.password);
                ds = new DirectorySearcher(de);
                ds.SearchScope = SearchScope.Subtree;
                ds.Filter = string.Format("(userPrincipalName={0})", userPrincipalName);

                if (ds.FindOne() != null)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error checking if user exists " + userPrincipalName, ex);

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
        /// Gets detailed user information about a specific user
        /// </summary>
        /// <param name="userPrincipalName"></param>
        /// <returns></returns>
        public UsersObject GetUser(string userPrincipalName)
        {
            PrincipalContext pc = null;
            UserPrincipalExt up = null;

            try
            {
                pc = new PrincipalContext(ContextType.Domain, this.domainController, this.username, this.password);

                logger.Debug("Attempting to retrieve user " + userPrincipalName);

                up = UserPrincipalExt.FindByIdentity(pc, IdentityType.UserPrincipalName, userPrincipalName);
                if (up == null)
                    throw new Exception("USER_NOT_FOUND");
                else
                {
                    UsersObject returnUser = new UsersObject();
                    returnUser.UserPrincipalName = up.UserPrincipalName;
                    returnUser.sAMAccountName = up.SamAccountName;
                    returnUser.Firstname = up.GivenName;
                    returnUser.Middlename = up.MiddleName;
                    returnUser.Lastname = up.Surname;
                    returnUser.DisplayName = up.DisplayName;
                    returnUser.Department = up.Department;
                    returnUser.IsEnabled = up.Enabled == null ? true : (bool)up.Enabled;

                    return returnUser;
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving user information " + userPrincipalName, ex);

                throw;
            }
            finally
            {
                if (up != null)
                    up.Dispose();

                if (pc != null)
                    pc.Dispose();
            }
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user"></param>
        /// <param name="baseOrganizationalUnit"></param>
        /// <param name="isUsingDisplayNameAsNameAttribute"></param>
        public UsersObject NewUser(UsersObject user, string companyUsersPath, bool isUsingDisplayNameAsNameAttribute)
        {
            PrincipalContext pc = null;
            UserPrincipalExt up = null;

            try
            {
                pc = new PrincipalContext(ContextType.Domain, this.domainController, companyUsersPath, this.username, this.password);

                logger.Debug("Looking to see if user already exists: " + user.UserPrincipalName);

                bool doesExist = DoesUserPrincipalNameExist(user.UserPrincipalName);
                if (doesExist)
                    throw new Exception(AlertID.USER_ALREADY_EXISTS.ToString());
                else
                {
                    // Find an available sAMAccountName
                    user.sAMAccountName = GetAvailableSamAccountName(user.UserPrincipalName);

                    // User was not found so lets create the new user
                    up = new UserPrincipalExt(pc, user.sAMAccountName, user.Password, true);
                    up.UserPrincipalName = user.UserPrincipalName;
                    up.DisplayName = user.DisplayName;
                    up.PasswordNeverExpires = user.PasswordNeverExpires;

                    if (isUsingDisplayNameAsNameAttribute)
                        up.Name = user.DisplayName;
                    else
                        up.Name = user.UserPrincipalName;

                    if (!string.IsNullOrEmpty(user.Firstname))
                        up.GivenName = user.Firstname;
                    
                    if (!string.IsNullOrEmpty(user.Middlename))
                        up.MiddleName = user.Middlename;

                    if (!string.IsNullOrEmpty(user.Lastname))
                        up.LastName = user.Lastname;

                    if (!string.IsNullOrEmpty(up.Department))
                        up.Department = user.Department;

                    up.Save();

                    // Get the user's GUID
                    user.UserGuid = (Guid)up.Guid;

                    // Get the user's distinguished name
                    user.DistinguishedName = up.DistinguishedName;

                    // Return the user with the information
                    return user;
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Error creating new user " + user.UserPrincipalName, ex);

                throw;
            }
            finally
            {
                if (up != null)
                    up.Dispose();

                if (pc != null)
                    pc.Dispose();
            }
        }

        /// <summary>
        /// Updates a user in Active Directory
        /// </summary>
        /// <param name="user"></param>
        /// <param name="isUsingDisplayNameAsNameAttribute"></param>
        public void UpdateUser(UsersObject user, bool isUsingDisplayNameAsNameAttribute)
        {
            PrincipalContext pc = null;
            UserPrincipalExt up = null;

            try
            {
                pc = new PrincipalContext(ContextType.Domain, this.domainController, this.username, this.password);

                logger.Debug("Finding user in Active Directory: " + user.UserPrincipalName);

                up = UserPrincipalExt.FindByIdentity(pc, IdentityType.UserPrincipalName, user.UserPrincipalName);
                if (up == null)
                    throw new Exception(AlertID.USER_UNKNOWN.ToString());
                else
                {
                    up.GivenName = user.Firstname;
                    up.DisplayName = user.DisplayName;
                    up.Enabled = user.IsEnabled;

                    if (!string.IsNullOrEmpty(user.Middlename))
                        up.MiddleName = user.Middlename;
                    else
                        up.MiddleName = null;

                    if (!string.IsNullOrEmpty(user.Lastname))
                        up.LastName = user.Lastname;
                    else
                        up.LastName = null;

                    if (!string.IsNullOrEmpty(user.Department))
                        up.Department = user.Department;
                    else
                        up.Department = null;

                    if (isUsingDisplayNameAsNameAttribute)
                        up.Name = user.DisplayName;

                    // Save changes
                    up.Save();
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Error updating user " + user.UserPrincipalName, ex);

                throw;
            }
            finally
            {
                if (up != null)
                    up.Dispose();

                if (pc != null)
                    pc.Dispose();
            }
        }

        /// <summary>
        /// Deletes a user from the system
        /// </summary>
        /// <param name="userPrincipalName"></param>
        public void DeleteUser(string userPrincipalName)
        {
            PrincipalContext pc = null;
            UserPrincipal up = null;

            try
            {
                pc = new PrincipalContext(ContextType.Domain, this.domainController, this.username, this.password);

                logger.Debug("Looking to see if user already exists: " + userPrincipalName);

                up = UserPrincipal.FindByIdentity(pc, IdentityType.UserPrincipalName, userPrincipalName);
                if (up != null)
                {
                    up.Delete();

                    logger.Info("Deleted user " + userPrincipalName);
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Error deleting user " + userPrincipalName, ex);
                throw;
            }
            finally
            {
                if (up != null)
                    up.Dispose();

                if (pc != null)
                    pc.Dispose();
            }
        }

        /// <summary>
        /// Resets the password for the user
        /// </summary>
        /// <param name="userPrincipalName"></param>
        /// <param name="newPassword"></param>
        public void ResetPassword(string userPrincipalName, string newPassword)
        {
            PrincipalContext pc = null;
            UserPrincipal up = null;

            try
            {
                pc = new PrincipalContext(ContextType.Domain, this.domainController, this.username, this.password);

                logger.Debug("Looking to see if user already exists to reset the password: " + userPrincipalName);

                up = UserPrincipal.FindByIdentity(pc, IdentityType.UserPrincipalName, userPrincipalName);
                if (up != null)
                {
                    up.SetPassword(newPassword);
                    up.Save();

                    logger.Info("Successfully changed password for " + userPrincipalName);
                }
            }
            catch (PasswordException ex)
            {
                this.logger.Error("Error changing password for " + userPrincipalName, ex);
                throw;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error changing password for " + userPrincipalName, ex);
                throw;
            }
            finally
            {
                if (up != null)
                    up.Dispose();

                if (pc != null)
                    pc.Dispose();
            }
        }

        /// <summary>
        /// Gets the photo for the user from the thumbnailPhoto attribute in Active Directory
        /// </summary>
        /// <param name="userPrincipalName"></param>
        /// <returns></returns>
        public byte[] GetPhoto(string userPrincipalName)
        {
            DirectoryEntry de = null;
            DirectorySearcher ds = null;

            try
            {
                logger.Debug("Attempting to find photo for " + userPrincipalName);

                de = new DirectoryEntry("LDAP://" + this.domainController, this.username, this.password);
                ds = new DirectorySearcher(de);
                ds.SearchScope = SearchScope.Subtree;
                ds.Filter = string.Format("(&(objectClass=User)(userPrincipalName={0}))", userPrincipalName);

                SearchResult found = ds.FindOne();

                byte[] data = null;
                if (found != null)
                {
                    using (DirectoryEntry u = new DirectoryEntry(found.Path))
                    {
                        if (u.Properties["thumbnailPhoto"].Value != null)
                            data = u.Properties["thumbnailPhoto"].Value as byte[];
                    }
                }

                return data;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error getting photto for " + userPrincipalName, ex);
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

        #region Dispose
        /// <summary>
        /// Disposing
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    username = null;
                    password = null;
                    domainController = null;
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
