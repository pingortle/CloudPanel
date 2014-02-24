using CloudPanel.Modules.Base.Settings;
using CloudPanel.Modules.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Database.Settings
{
    public class DatabaseSettings
    {
        /// <summary>
        /// Retrieves the settings from the database
        /// </summary>
        /// <returns></returns>
        public static SettingsObject GetSettings()
        {
            DB database = null;

            try
            {
                database = new DB();

                var settings = (from s in database.Settings
                                select new SettingsObject
                                {
                                    HostingOU = s.BaseOU,
                                    PrimaryDC = s.PrimaryDC,
                                    Username = s.Username,
                                    Password = s.Password,
                                    SuperAdmins = s.SuperAdmins,
                                    BillingAdmins = s.BillingAdmins,
                                    ExchangeServer = s.ExchangeFqdn,
                                    ExchangePublicFolderServer = s.ExchangePFServer,
                                    ExchangeVersion = s.ExchangeVersion,
                                    ExchangeSSLEnabled = s.ExchangeSSLEnabled,
                                    ExchangeConnectionType = s.ExchangeConnectionType,
                                    CitrixEnabled = s.CitrixEnabled,
                                    PublicFoldersEnabled = s.PublicFolderEnabled,
                                    LyncEnabled = s.LyncEnabled,
                                    ResellersEnabled = s.ResellersEnabled == null ? false : (bool)s.ResellersEnabled,
                                    HostersName = s.CompanysName,
                                    AllowCustomNameAttribute = s.AllowCustomNameAttrib == null ? false : (bool)s.AllowCustomNameAttrib,
                                    IPBlockingEnabled = s.IPBlockingEnabled == null ? false : (bool)s.IPBlockingEnabled,
                                    UsersOU = s.UsersOU,
                                    LoginLogo = s.BrandingLoginLogo,
                                    CornerLogo = s.BrandingCornerLogo,
                                    LockDownEnabled = s.LockdownEnabled == null ? false : (bool)s.LockdownEnabled
                                }).FirstOrDefault();

                return settings;
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
    }
}
