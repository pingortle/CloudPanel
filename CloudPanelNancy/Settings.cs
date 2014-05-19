using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy
{
    public static class Settings
    {
        public static string ConnectionString { 
#if DEBUG
            get { return "server=DXN-PC\\SQLEXPRESS;database=CloudPanel;uid=CloudPanel;password=password;"; }
#else
            get; 
#endif
        }

        #region Basic Settings

        public static string HostingOU { get; set; }

        public static string PrimaryDC { get; set; }

        public static string Username { get; set; }

        public static string Password { get; set; }

        public static string SuperAdmins { get; set; }

        public static string BillingAdmins { get; set; }
        
        #endregion

        #region Exchange Settings

        public static string ExchangeServer { get; set; }

        public static string ExchangePFServer { get; set; }

        public static int Version { get; set; }

        public static bool ExchangeSSL { get; set; }

        public static string ExchangeConnection { get; set; }

        #endregion
    }
}