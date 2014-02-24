using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Common.Settings
{
    public class StaticSettings
    {
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

    }
}
