using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class User
    {
        public int ID { get; set; }
        public System.Guid UserGuid { get; set; }
        public string CompanyCode { get; set; }
        public string sAMAccountName { get; set; }
        public string UserPrincipalName { get; set; }
        public string DistinguishedName { get; set; }
        public string DisplayName { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public Nullable<bool> IsEnabled { get; set; }
        public Nullable<bool> IsResellerAdmin { get; set; }
        public Nullable<bool> IsCompanyAdmin { get; set; }
        public Nullable<int> MailboxPlan { get; set; }
        public Nullable<int> TSPlan { get; set; }
        public Nullable<int> LyncPlan { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> AdditionalMB { get; set; }
        public Nullable<int> ActiveSyncPlan { get; set; }
        public Nullable<int> ExchArchivePlan { get; set; }

        public bool IsLyncEnabled {
            get { return (LyncPlan != null && LyncPlan > 0) ? true : false; }
        }

        public bool IsEmailEnabled {
            get { return (MailboxPlan != null && MailboxPlan > 0) ? true : false; }
        }
    }
}
