using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Statistics
{
    public class OverallStats
    {
        public int TotalUsers { get; set; }

        public int TotalResellers { get; set; }

        public int TotalCompanies { get; set; }

        public int TotalMailboxes { get; set; }

        public int TotalCitrixUsers { get; set; }

        public int TotalLyncUsers { get; set; }

        public int TotalDomains { get; set; }

        public int TotalAcceptedDomains { get; set; }

        public string TotalAllocatedEmailSpace { get; set; }
    }
}
