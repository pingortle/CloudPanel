using CloudPanel.Modules.Base.Enumerations;
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

        public string CompanyCode { get; set; }

        public DateTime WhenEntered { get; set; }

        public ActionID Action { get; set; }

        public string ActionIDGlobalization { get; set; }

        public string Variable1 { get; set; }

        public string Variable2 { get; set; }

        public string FormattedMessage { get; set; }


    }
}
