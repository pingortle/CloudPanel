using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Users
{
    [Serializable]
    public class UsersObject
    {
        public Guid UserGuid { get; set; }

        public string CompanyCode { get; set; }

        public string CompanyName { get; set; }

        public string ResellerCode { get; set; }

        public string sAMAccountName { get; set; }

        public string UserPrincipalName { get; set; }

        public string DistinguishedName { get; set; }

        public string DisplayName { get; set; }

        public string Firstname { get; set; }

        public string Middlename { get; set; }

        public string Lastname { get; set; }

        public string Department { get; set; }

        public string Password { get; set; }

        public int LyncPlan { get; set; }

        public DateTime? Created { get; set; }

        public bool PasswordNeverExpires { get; set; }

        public List<string> Groups { get; set; }

        #region Graphics

        public byte[] UserPhoto { get; set; }

        #endregion

        #region Exchange

        public int AdditionalMB { get; set; }

        public int ActiveSyncPlan { get; set; }

        public int MailboxPlan { get; set; }

        public string PrimarySmtpAddress { get; set; }

        public string ForwardingTo { get; set; }

        public string CurrentMailboxDatabase { get; set; }

        public string LitigationHoldComment { get; set; }

        public string LitigationHoldUrl { get; set; }

        public string ThrottlingPolicy { get; set; }

        public string ExchangeAlias { get; set; }

        public List<string> EmailAliases { get; set; }

        public List<string> FullAccessUsers { get; set; }

        public List<string> SendAsUsers { get; set; }

        public bool DeliverToMailboxAndForward { get; set; }

        public bool LitigationHoldEnabled { get; set; }

        public bool HasExchangePicture { get; set; }

        public bool MailboxHiddenFromGAL { get; set; }

        #endregion

        #region Permissions

        public bool IsSuperAdmin { get; set; }

        public bool IsResellerAdmin { get; set; }

        public bool IsCompanyAdmin { get; set; }

        public bool EnableExchangePerm { get; set; }

        public bool DisableExchangePerm { get; set; }

        public bool AddDomainPerm { get; set; }

        public bool DeleteDomainPerm { get; set; }

        public bool EnableAcceptedDomainPerm { get; set; }

        public bool DisableAcceptedDomainPerm { get; set; }

        #endregion
    }
}
