using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class Plans_ExchangeMailbox
    {
        public int MailboxPlanID { get; set; }
        public string MailboxPlanName { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string ResellerCode { get; set; }
        public string CompanyCode { get; set; }
        public int MailboxSizeMB { get; set; }
        public Nullable<int> MaxMailboxSizeMB { get; set; }
        public int MaxSendKB { get; set; }
        public int MaxReceiveKB { get; set; }
        public int MaxRecipients { get; set; }
        public bool EnablePOP3 { get; set; }
        public bool EnableIMAP { get; set; }
        public bool EnableOWA { get; set; }
        public bool EnableMAPI { get; set; }
        public bool EnableAS { get; set; }
        public bool EnableECP { get; set; }
        public int MaxKeepDeletedItems { get; set; }
        public string MailboxPlanDesc { get; set; }
        public string Price { get; set; }
        public string Cost { get; set; }
        public string AdditionalGBPrice { get; set; }
    }
}
