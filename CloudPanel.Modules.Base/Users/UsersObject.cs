using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Users
{
    public class UsersObject
    {
        public Guid UserGuid { get; set; }

        public string CompanyCode { get; set; }

        public string sAMAccountName { get; set; }

        public string UserPrincipalName { get; set; }

        public string DistinguishedName { get; set; }

        public string DisplayName { get; set; }

        public string Firstname { get; set; }

        public string Middlename { get; set; }

        public string Lastname { get; set; }

        public string PrimarySmtpAddress { get; set; }

        public string Department { get; set; }

        public string Password { get; set; }

        public bool IsResellerAdmin { get; set; }

        public bool IsCompanyAdmin { get; set; }

        public int MailboxPlan { get; set; }

        public int LyncPlan { get; set; }

        public int AdditionalMB { get; set; }

        public int ActiveSyncPlan { get; set; }

        public DateTime Created { get; set; }

        public bool PasswordNeverExpires { get; set; }

        #region Permissions

        public bool EnableExchangePerm { get; set; }

        public bool DisableExchangePerm { get; set; }

        public bool AddDomainPerm { get; set; }

        public bool DeleteDomainPerm { get; set; }

        public bool EnableAcceptedDomainPerm { get; set; }

        public bool DisableAcceptedDomainPerm { get; set; }

        #endregion
    }
}
