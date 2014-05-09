using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class SvcTask
    {
        public int SvcTaskID { get; set; }
        public int TaskType { get; set; }
        public System.DateTime LastRun { get; set; }
        public Nullable<System.DateTime> NextRun { get; set; }
        public string TaskOutput { get; set; }
        public int TaskDelayInMinutes { get; set; }
        public Nullable<System.DateTime> TaskCreated { get; set; }
        public bool Reoccurring { get; set; }
    }
}
