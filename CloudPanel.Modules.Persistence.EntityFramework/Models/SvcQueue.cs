using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class SvcQueue
    {
        public int SvcQueueID { get; set; }
        public int TaskID { get; set; }
        public string UserPrincipalName { get; set; }
        public string CompanyCode { get; set; }
        public string TaskOutput { get; set; }
        public System.DateTime TaskCreated { get; set; }
        public Nullable<System.DateTime> TaskCompleted { get; set; }
        public int TaskDelayInMinutes { get; set; }
        public int TaskSuccess { get; set; }
    }
}
