using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class CompanyStat
    {
        public int CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public int UserCount { get; set; }
        public int DomainCount { get; set; }
        public int SubDomainCount { get; set; }
        public int ExchMailboxCount { get; set; }
        public int ExchContactsCount { get; set; }
        public int ExchDistListsCount { get; set; }
        public int ExchPublicFolderCount { get; set; }
        public int ExchMailPublicFolderCount { get; set; }
        public int ExchKeepDeletedItems { get; set; }
        public int RDPUserCount { get; set; }
    }
}
