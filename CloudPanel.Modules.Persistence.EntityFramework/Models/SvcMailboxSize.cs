using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class SvcMailboxSize
    {
        public int ID { get; set; }
        public string UserPrincipalName { get; set; }
        public string MailboxDatabase { get; set; }
        public string TotalItemSizeInKB { get; set; }
        public string TotalDeletedItemSizeInKB { get; set; }
        public int ItemCount { get; set; }
        public int DeletedItemCount { get; set; }
        public System.DateTime Retrieved { get; set; }
    }
}
