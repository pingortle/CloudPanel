using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Common.Settings
{
    public class StaticSettings
    {

        #region Database Settings


        #endregion

        #region Active Directory

        /// <summary>
        /// The base organizational unit for hosting resellers & companies
        /// </summary>
        public string HostingOU { get; set; }

        /// <summary>
        /// The primary domain controller to communicate with
        /// </summary>
        public string PrimaryDC { get; set; }

        #endregion

        #region Authentication

        /// <summary>
        /// DOMAIN\Username to use when running remote commands
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// The password for the username
        /// </summary>
        public string Password { get; set; }

        #endregion

        #region Exchange

        /// <summary>
        /// The Exchange server for powershell commands
        /// </summary>
        public string ExchangeFqdn { get; set; }

        #endregion
    }
}
