using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using NLog;
using CloudPanel.Modules.Base.Settings;
using CloudPanel.Modules.Database.Settings;
using CloudPanel.Modules.Common.Settings;

namespace CloudPanel
{
    public class Global : System.Web.HttpApplication
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                SettingsObject settings = DatabaseSettings.GetSettings();

                // Set the settings in static variables
                StaticSettings.HostingOU = settings.HostingOU;
                StaticSettings.PrimaryDC = settings.PrimaryDC;
                StaticSettings.Username = settings.Username;
                StaticSettings.Password = settings.Password;
                StaticSettings.SuperAdmins = settings.SuperAdmins;
                StaticSettings.BillingAdmins = settings.BillingAdmins;
                StaticSettings.ExchangeServer = settings.ExchangeServer;
                StaticSettings.ExchangePublicFolderServer = settings.ExchangePublicFolderServer;
                StaticSettings.ExchangeVersion = settings.ExchangeVersion;
                StaticSettings.ExchangeSSLEnabled = settings.ExchangeSSLEnabled;
                StaticSettings.ExchangeConnectionType = settings.ExchangeConnectionType;
                StaticSettings.CitrixEnabled = settings.CitrixEnabled;
                StaticSettings.PublicFoldersEnabled = settings.PublicFoldersEnabled;
                StaticSettings.LyncEnabled = settings.LyncEnabled;
                StaticSettings.ResellersEnabled = settings.ResellersEnabled;
                StaticSettings.HostersName = settings.HostersName;
                StaticSettings.AllowCustomNameAttribute = settings.AllowCustomNameAttribute;
                StaticSettings.IPBlockingEnabled = settings.IPBlockingEnabled;
                StaticSettings.IPBlockingFailedCount = settings.IPBlockingFailedCount;
                StaticSettings.IPBlockingLockedInMinutes = settings.IPBlockingLockedInMinutes;
                StaticSettings.UsersOU = settings.UsersOU;
                StaticSettings.LoginLogo = settings.LoginLogo;
                StaticSettings.CornerLogo = settings.CornerLogo;
                StaticSettings.LockdownEnabled = settings.LockDownEnabled;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {

        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}