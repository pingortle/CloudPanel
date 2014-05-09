using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class Audit
    {
        public int AuditID { get; set; }
        public string CompanyCode { get; set; }
        public string Username { get; set; }
        public System.DateTime Date { get; set; }
        public int ActionID { get; set; }
        public string Variable1 { get; set; }
        public string Variable2 { get; set; }
    }
}
