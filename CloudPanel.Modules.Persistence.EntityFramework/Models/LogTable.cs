using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class LogTable
    {
        public int AuditID { get; set; }
        public System.DateTime LogTime { get; set; }
        public int LogType { get; set; }
        public string CodeClass { get; set; }
        public string UserPrincipalName { get; set; }
        public string CompanyCode { get; set; }
        public string ResellerCode { get; set; }
        public string Message { get; set; }
        public string RecommendedAction { get; set; }
        public string Exception { get; set; }
    }
}
