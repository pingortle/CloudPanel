//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CloudPanel.data
{
    using System;
    using System.Collections.Generic;
    
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
