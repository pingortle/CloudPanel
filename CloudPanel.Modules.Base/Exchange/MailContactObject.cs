using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Exchange
{
    public class MailContactObject
    {
        public string DistinguishedName { get; set; }

        public string CompanyCode { get; set; }

        public string DisplayName { get; set; }

        public string Email { get; set; }

        public bool Hidden { get; set; }

    }
}
