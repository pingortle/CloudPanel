using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Auditing
{
    public class Audits
    {
        public int AuditID { get; set; }

        public string Username { get; set; }

        public string Message { get; set; }

        public string CompanyCode { get; set; }

        public DateTime WhenEntered { get; set; }

    }
}
