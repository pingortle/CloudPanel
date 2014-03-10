using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Plans
{
    public class CompanyPlanObject
    {
        public int CompanyPlanID { get; set; }

        public int ProductID { get; set; }

        public int MaxUser { get; set; }

        public int MaxDomains { get; set; }

        public int MaxExchangeMailboxes { get; set; }

        public int MaxExchangeContacts { get; set; }

        public int MaxExchangeDistributionGroups { get; set; }

        public int MaxExchangeMailPublicFolders { get; set; }

        public int MaxExchangeKeepDeletedItems { get; set; }

        public int MaxExchangeActivesyncPolicies { get; set; }

        public int MaxCitrixUsers { get; set; }

        public int MaxExchangeResourceMailboxes { get; set; }

        public string ResellerCode { get; set; }

        public string CompanyPlanName { get; set; }

    }
}
