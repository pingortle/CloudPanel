using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Statistics
{
    public class OverallStats
    {
        public int TotalUsers { get; set; }

        public int TodayUsers { get; set; }

        public int TotalResellers { get; set; }

        public int TotalCompanies { get; set; }

        public int TodayCompanies { get; set; }

        public int TotalMailboxes { get; set; }

        public int TotalDistributionGroups { get; set; }

        public int TotalMailContacts { get; set; }

        public int TotalCitrixUsers { get; set; }

        public int TotalCitrixApps { get; set; }

        public int TotalCitrixServers { get; set; }

        public int TotalLyncUsers { get; set; }

        public int TotalDomains { get; set; }

        public int TotalAcceptedDomains { get; set; }

        public decimal TotalUsedEmailSpaceInKB { get; set; }

        public decimal TotalUsedEmailSpace { get; set; }

        public string TotalUsedEmailSpaceSizeType { get; set; }

        public decimal TotalAllocatedEmailSpaceInKB { get; set; }

        public decimal TotalAllocatedEmailSpace { get; set; }

        public string TotalAllocatedEmailSpaceSizeType { get; set; }

    }
}
