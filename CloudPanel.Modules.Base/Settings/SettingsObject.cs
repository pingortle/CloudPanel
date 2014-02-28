using CloudPanel.Modules.Base.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Settings
{
    public class SettingsObject
    {
        #region Variables

        private string _hostersname;
        private string _hostingou;
        private string _primarydc;
        private string _username;
        private string _password;
        private string _superadmins;
        private string _billingadmins;
        private string _exchangeserver;
        private string _exchangepublicfolderserver;
        private string _exchangeconnectiontype;
        private string _usersou;
        private string _loginlogo;
        private string _cornerlogo;
        private string _securitykey;

        private int _exchangeversion;
        private int _ipblockingfailedcount;
        private int _ipblockinglockedinminutes;

        private bool _exchangesslenabled;
        private bool _citrixenabled;
        private bool _publicfoldersenabled;
        private bool _lyncenabled;
        private bool _resellersenabled;
        private bool _allowcustomnameattribute;
        private bool _exchangestatistics;
        private bool _lockdownenabled;
        private bool _ipblockingenabled;

        #endregion

        #region Getters & Setters

        public string HostersName
        {
            get { return _hostersname; }
            set { _hostersname = value; }
        }

        public string HostingOU
        {
            get { return _hostingou; }
            set { _hostingou = value; }
        }

        public string PrimaryDC
        {
            get { return _primarydc; }
            set { _primarydc = value; }
        }

        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        public string Password
        {
            get 
            { 
                // Decrypt password first
                string decrypted = DataProtection.Decrypt(_password, SecurityKey);
                return decrypted;
            }
            set { _password = value; }
        }

        public string SuperAdmins
        {
            get { return _superadmins; }
            set { _superadmins = value; }
        }

        public string BillingAdmins
        {
            get { return _billingadmins; }
            set { _billingadmins = value; }
        }

        public string ExchangeServer
        {
            get { return _exchangeserver; }
            set { _exchangeserver = value; }
        }

        public string ExchangePublicFolderServer
        {
            get { return _exchangepublicfolderserver; }
            set { _exchangepublicfolderserver = value; }
        }

        public string ExchangeConnectionType
        {
            get { return _exchangeconnectiontype; }
            set { _exchangeconnectiontype = value; }
        }

        public string UsersOU
        {
            get { return _usersou; }
            set { _usersou = value; }
        }

        public string LoginLogo
        {
            get { return _loginlogo; }
            set { _loginlogo = value; }
        }

        public string CornerLogo
        {
            get { return _cornerlogo; }
            set { _cornerlogo = value; }
        }

        public int ExchangeVersion
        {
            get { return _exchangeversion; }
            set { _exchangeversion = value; }
        }

        public int IPBlockingFailedCount
        {
            get { return _ipblockingfailedcount; }
            set { _ipblockingfailedcount = value; }
        }

        public int IPBlockingLockedInMinutes
        {
            get { return _ipblockinglockedinminutes; }
            set { _ipblockinglockedinminutes = value; }
        }

        public bool ExchangeSSLEnabled
        {
            get { return _exchangesslenabled; }
            set { _exchangesslenabled = value; }
        }

        public bool CitrixEnabled
        {
            get { return _citrixenabled; }
            set { _citrixenabled = value; }
        }

        public bool PublicFoldersEnabled
        {
            get { return _publicfoldersenabled; }
            set { _publicfoldersenabled = value; }
        }

        public bool LyncEnabled
        {
            get { return _lyncenabled; }
            set { _lyncenabled = value; }
        }

        public bool ResellersEnabled
        {
            get { return _resellersenabled; }
            set { _resellersenabled = value; }
        }

        public bool AllowCustomNameAttribute
        {
            get { return _allowcustomnameattribute; }
            set { _allowcustomnameattribute = value; }
        }

        public bool ExchangeStatistics
        {
            get { return _exchangestatistics; }
            set { _exchangestatistics = value; }
        }

        public bool LockDownEnabled
        {
            get { return _lockdownenabled; }
            set { _lockdownenabled = value; }
        }

        public bool IPBlockingEnabled
        {
            get { return _ipblockingenabled; }
            set { _ipblockingenabled = value; }
        }

        public string SecurityKey
        {
            get { return _securitykey; }
            set { _securitykey = value; }
        }

        #endregion
    }
}
