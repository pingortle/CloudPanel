using CloudPanel.Modules.Common.Database;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.Entity;
using CloudPanel.Modules.Base.Security;

namespace CloudPanel.Modules.Common.Settings
{
    public class StaticSettings
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Variables

        public static string HostingOU { get; set; }

        public static string PrimaryDC { get; set; }

        public static string Username { get; set; }

        public static string Password { get; set; }

        public static string SuperAdmins { get; set; }

        public static string BillingAdmins { get; set; }

        public static string ExchangeServer { get; set; }

        public static string ExchangePublicFolderServer { get; set; }

        public static string ExchangeConnectionType { get; set; }

        public static string HostersName { get; set; }

        public static string UsersOU { get; set; }

        public static string LoginLogo { get; set; }

        public static string CornerLogo { get; set; }

        public static string SecurityKey { get; set; }

        public static bool ExchangeSSLEnabled { get; set; }

        public static bool CitrixEnabled { get; set; }

        public static bool PublicFoldersEnabled { get; set; }

        public static bool LyncEnabled { get; set; }

        public static bool ResellersEnabled { get; set; }

        public static bool AllowCustomNameAttribute { get; set; }

        public static bool IPBlockingEnabled { get; set; }

        public static bool LockdownEnabled { get; set; }

        public static int IPBlockingFailedCount { get; set; }

        public static int IPBlockingLockedInMinutes { get; set; }

        public static int ExchangeVersion { get; set; }

        #endregion

        /// <summary>
        /// Retrieves the settings from the database
        /// </summary>
        /// <returns></returns>
        public static void GetSettings(string securityKey)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var settings = (from s in database.Settings
                                select s).FirstOrDefault();

                // Populate static settings
                SecurityKey = securityKey;
                logger.Debug("Security Key: " + SecurityKey);

                HostingOU = settings.BaseOU;
                logger.Debug("Hosting OU: " + HostingOU);

                PrimaryDC = settings.PrimaryDC;
                logger.Debug("Domain Controller: " + PrimaryDC);

                Username = settings.Username;
                logger.Debug("Username: " + Username);

                Password = DataProtection.Decrypt(settings.Password, SecurityKey);
                logger.Debug("Password: " + Password);

                UsersOU = settings.UsersOU;

                SuperAdmins = settings.SuperAdmins;
                BillingAdmins = settings.BillingAdmins;

                ExchangeServer = settings.ExchangeFqdn;
                ExchangePublicFolderServer = settings.ExchangePFServer;
                ExchangeVersion = settings.ExchangeVersion;
                ExchangeSSLEnabled = settings.ExchangeSSLEnabled;
                ExchangeConnectionType = settings.ExchangeConnectionType;
                PublicFoldersEnabled = settings.PublicFolderEnabled;

                CitrixEnabled = settings.CitrixEnabled;
                LyncEnabled = settings.LyncEnabled;
                ResellersEnabled = (bool)settings.ResellersEnabled;

                HostersName = settings.CompanysName;

                AllowCustomNameAttribute = (bool)settings.AllowCustomNameAttrib;

                CornerLogo = settings.BrandingCornerLogo;
                LoginLogo = settings.BrandingLoginLogo;

                IPBlockingEnabled = (bool)settings.IPBlockingEnabled;
                IPBlockingFailedCount = (int)settings.IPBlockingFailedCount;
                IPBlockingLockedInMinutes = (int)settings.IPBlockingLockedMinutes;
            }
            catch (Exception)
            {
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
