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
        public string[] Authenticate(string userName, string userPassword)
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

                    up = UserPrincipal.FindByIdentity(pc, IdentityType.UserPrincipalName, userName);
                    de = up.GetUnderlyingObject() as DirectoryEntry;

                    string[] groups = new string[de.Properties["memberOf"].Count];
                    for (int i = 0; i < groups.Length - 1; i++)
                    {
                        groups[i] = de.Properties["memberOf"][i].ToString();
                    }

                    logger.Debug(userName + " belongs to the following groups: " + String.Join(", ", groups));

                    return groups;
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
