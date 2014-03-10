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

        public static string Key { get; set;}

        public static string HostingOU { get; set; }

        public static string PrimaryDC { get; set; }

        public static string Username { get; set; }

        public static string Password { get; set; }

        public static string DecryptedPassword
        {
            get { return DataProtection.Decrypt(Password, Key); }
        }

        public static string EncryptedPassword
        {
            get { return Password; }
        }

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

        public static string ExchangeDatabases { get; set; }

        public static bool ExchangeSSLEnabled { get; set; }

        public static bool CitrixEnabled { get; set; }

        public static bool PublicFoldersEnabled { get; set; }

        public static bool LyncEnabled { get; set; }

        public static bool ResellersEnabled { get; set; }

        public static bool ExchangeEnabled { get; set; }

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

                Key = securityKey;

                var settings = (from s in database.Settings
                                select s).FirstOrDefault();
                
                // Populate static settings
                if (settings != null)
                {
                    SecurityKey = securityKey;
                    logger.Debug("Security Key: " + SecurityKey);

                    HostingOU = settings.BaseOU;
                    logger.Debug("Hosting OU: " + HostingOU);

                    PrimaryDC = settings.PrimaryDC;
                    logger.Debug("Domain Controller: " + PrimaryDC);

                    Username = settings.Username;
                    logger.Debug("Username: " + Username);

                    Password = settings.Password;

                    UsersOU = settings.UsersOU;
                    logger.Debug("Users OU: " + UsersOU);

                    SuperAdmins = settings.SuperAdmins;
                    logger.Debug("Super Admins: " + SuperAdmins);

                    BillingAdmins = settings.BillingAdmins;
                    logger.Debug("Billing Admins: " + BillingAdmins);

                    ExchangeServer = settings.ExchangeFqdn;
                    logger.Debug("Exchange Server: " + ExchangeServer);

                    ExchangePublicFolderServer = settings.ExchangePFServer;
                    logger.Debug("Exchange Public Folder Server: " + ExchangePublicFolderServer);

                    ExchangeVersion = settings.ExchangeVersion;
                    logger.Debug("Exchange Version: " + ExchangeVersion.ToString());

                    ExchangeSSLEnabled = settings.ExchangeSSLEnabled;
                    logger.Debug("Exchange SSL Enabled: " +  ExchangeSSLEnabled.ToString());

                    ExchangeConnectionType = settings.ExchangeConnectionType;
                    logger.Debug("Exchange Connection Type: " + ExchangeConnectionType);

                    ExchangeDatabases = settings.ExchDatabases;
                    logger.Debug("Exchange Databases: " + ExchangeDatabases);

                    PublicFoldersEnabled = settings.PublicFolderEnabled;
                    logger.Debug("Public Folders Enabled: " + PublicFoldersEnabled.ToString());

                    CitrixEnabled = settings.CitrixEnabled;
                    logger.Debug("Citrix Enabled: " + CitrixEnabled.ToString());

                    LyncEnabled = settings.LyncEnabled;
                    logger.Debug("Lync Enabled: " + LyncEnabled.ToString());

                    ExchangeEnabled = true;
                    logger.Debug("Exchange Enabled: " + ExchangeEnabled.ToString());

                    HostersName = settings.CompanysName;
                    logger.Debug("Hosters Name: " + HostersName);

                    bool isResellersEnabled = false;
                    bool.TryParse(settings.ResellersEnabled.ToString(), out isResellersEnabled);
                    ResellersEnabled = isResellersEnabled;
                    logger.Debug("Resellers Enabled: " + ResellersEnabled.ToString());

                    bool allowCustomAttrib = false;
                    bool.TryParse(settings.AllowCustomNameAttrib.ToString(), out allowCustomAttrib);
                    AllowCustomNameAttribute = allowCustomAttrib;
                    logger.Debug("Allow Custom Name Attribute: " + AllowCustomNameAttribute);

                    CornerLogo = settings.BrandingCornerLogo;
                    logger.Debug("Corner Logo: " + CornerLogo);

                    LoginLogo = settings.BrandingLoginLogo;
                    logger.Debug("Login Logo: " + LoginLogo);

                    bool ipBlockingEnabled = false;
                    bool.TryParse(settings.IPBlockingEnabled.ToString(), out ipBlockingEnabled);
                    IPBlockingEnabled = ipBlockingEnabled;
                    logger.Debug("IP Blocking Enabled: " + IPBlockingEnabled.ToString());


                    int ipBlockingFailedCount = 0;
                    int.TryParse(settings.IPBlockingFailedCount.ToString(), out ipBlockingFailedCount);
                    IPBlockingFailedCount = ipBlockingFailedCount;
                    logger.Debug("IP Blocking Failed Count: " + IPBlockingFailedCount.ToString());


                    int ipBlockingInMinutes = 0;
                    int.TryParse(settings.IPBlockingLockedMinutes.ToString(), out ipBlockingInMinutes);
                    IPBlockingLockedInMinutes = ipBlockingInMinutes;
                    logger.Debug("IP Blocking Locked Out In Minutes: " + IPBlockingLockedInMinutes.ToString());
                }
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

        /// <summary>
        /// Commits the current static settings to the database
        /// </summary>
        /// <param name="securityKey"></param>
        public static void CommitSettings(string securityKey)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var newSettings = (from s in database.Settings
                                select s).FirstOrDefault();

                if (newSettings == null)
                {
                    // No settings found.. insert new
                    newSettings = new Setting();

                    #region General

                    newSettings.CompanysName = HostersName;
                    newSettings.ResellersEnabled = ResellersEnabled;

                    #endregion

                    #region Active Directory

                    newSettings.BaseOU = HostingOU;
                    newSettings.UsersOU = UsersOU;
                    newSettings.PrimaryDC = PrimaryDC;
                    newSettings.Username = Username;
                    newSettings.Password = EncryptedPassword;

                    #endregion

                    #region Security Groups

                    newSettings.SuperAdmins = SuperAdmins;
                    newSettings.BillingAdmins = BillingAdmins;

                    #endregion

                    #region Billing

                    #endregion

                    #region Exchange

                    newSettings.ExchangeConnectionType = ExchangeConnectionType;
                    newSettings.ExchangeVersion = ExchangeVersion;
                    newSettings.ExchangeFqdn = ExchangeServer;
                    newSettings.ExchangePFServer = ExchangePublicFolderServer;
                    newSettings.ExchDatabases = ExchangeDatabases;
                    newSettings.PublicFolderEnabled = PublicFoldersEnabled;
                    newSettings.ExchangeSSLEnabled = ExchangeSSLEnabled;

                    #endregion

                    #region Modules

                    newSettings.CitrixEnabled = CitrixEnabled;
                    newSettings.LyncEnabled = LyncEnabled;

                    #endregion

                    #region Other

                    newSettings.LockdownEnabled = LockdownEnabled;
                    newSettings.AllowCustomNameAttrib = AllowCustomNameAttribute;
                    newSettings.IPBlockingEnabled = IPBlockingEnabled;
                    newSettings.IPBlockingFailedCount = IPBlockingFailedCount;
                    newSettings.IPBlockingLockedMinutes = IPBlockingLockedInMinutes;

                    #endregion

                    // Insert
                    database.Settings.Add(newSettings);
                    database.SaveChanges();
                }
                else
                {
                    #region General

                    newSettings.CompanysName = HostersName;
                    newSettings.ResellersEnabled = ResellersEnabled;

                    #endregion

                    #region Active Directory

                    newSettings.BaseOU = HostingOU;
                    newSettings.UsersOU = UsersOU;
                    newSettings.PrimaryDC = PrimaryDC;
                    newSettings.Username = Username;
                    newSettings.Password = EncryptedPassword;

                    #endregion

                    #region Security Groups

                    newSettings.SuperAdmins = SuperAdmins;
                    newSettings.BillingAdmins = BillingAdmins;

                    #endregion

                    #region Billing

                    #endregion

                    #region Exchange

                    newSettings.ExchangeConnectionType = ExchangeConnectionType;
                    newSettings.ExchangeVersion = ExchangeVersion;
                    newSettings.ExchangeFqdn = ExchangeServer;
                    newSettings.ExchangePFServer = ExchangePublicFolderServer;
                    newSettings.ExchDatabases = ExchangeDatabases;
                    newSettings.PublicFolderEnabled = PublicFoldersEnabled;
                    newSettings.ExchangeSSLEnabled = ExchangeSSLEnabled;

                    #endregion

                    #region Modules

                    newSettings.CitrixEnabled = CitrixEnabled;
                    newSettings.LyncEnabled = LyncEnabled;

                    #endregion

                    #region Other

                    newSettings.LockdownEnabled = LockdownEnabled;
                    newSettings.AllowCustomNameAttrib = AllowCustomNameAttribute;
                    newSettings.IPBlockingEnabled = IPBlockingEnabled;
                    newSettings.IPBlockingFailedCount = IPBlockingFailedCount;
                    newSettings.IPBlockingLockedMinutes = IPBlockingLockedInMinutes;

                    #endregion

                    // Save
                    database.SaveChanges();
                }
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
