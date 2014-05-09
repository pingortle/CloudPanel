using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class ResourceMailbox
    {
        public int ResourceID { get; set; }
        public string DisplayName { get; set; }
        public string CompanyCode { get; set; }
        public string UserPrincipalName { get; set; }
        public string PrimarySmtpAddress { get; set; }
        public string ResourceType { get; set; }
        public int MailboxPlan { get; set; }
        public int AdditionalMB { get; set; }
    }
}
