using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class Domain
    {
        public int DomainID { get; set; }
        public string CompanyCode { get; set; }
        public string Domain1 { get; set; }
        public Nullable<bool> IsSubDomain { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAcceptedDomain { get; set; }
        public Nullable<bool> IsLyncDomain { get; set; }
        public Nullable<int> DomainType { get; set; }
    }
}
