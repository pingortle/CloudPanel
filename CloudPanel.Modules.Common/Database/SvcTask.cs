//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CloudPanel.Modules.Common.Database
{
    using System;
    using System.Collections.Generic;
    
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
