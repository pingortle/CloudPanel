//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CloudPanel.Modules.Database.Entity
{
    using System;
    using System.Collections.Generic;
    
    public partial class Audit
    {
        public int AuditID { get; set; }
        public string Username { get; set; }
        public System.DateTime Date { get; set; }
        public Nullable<int> SeverityID { get; set; }
        public string MethodName { get; set; }
        public string Parameters { get; set; }
        public string Message { get; set; }
    }
}
