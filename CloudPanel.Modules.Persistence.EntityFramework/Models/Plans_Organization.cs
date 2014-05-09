using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class Plans_Organization
    {
        public int OrgPlanID { get; set; }
        public string OrgPlanName { get; set; }
        public int ProductID { get; set; }
        public string ResellerCode { get; set; }
        public int MaxUsers { get; set; }
        public int MaxDomains { get; set; }
        public int MaxExchangeMailboxes { get; set; }
        public int MaxExchangeContacts { get; set; }
        public int MaxExchangeDistLists { get; set; }
        public int MaxExchangePublicFolders { get; set; }
        public int MaxExchangeMailPublicFolders { get; set; }
        public int MaxExchangeKeepDeletedItems { get; set; }
        public Nullable<int> MaxExchangeActivesyncPolicies { get; set; }
        public int MaxTerminalServerUsers { get; set; }
        public Nullable<int> MaxExchangeResourceMailboxes { get; set; }
    }
}
