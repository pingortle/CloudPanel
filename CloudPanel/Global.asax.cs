using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using CloudPanel.Modules.Base.Settings;
using CloudPanel.Modules.Common.Settings;
using log4net.Config;
using log4net;
using System.Reflection;
using System.Configuration;

namespace CloudPanel
{
    public class Global : System.Web.HttpApplication
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected void Application_Start(object sender, EventArgs e)
        {
            // Start logger
            XmlConfigurator.Configure();

            try
            {
                StaticSettings.GetSettings(ConfigurationManager.AppSettings["Key"]);
            }
            catch (Exception ex)
            {
                this.logger.Fatal("Error loading settings", ex);

                // TODO SHOW ERROR
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