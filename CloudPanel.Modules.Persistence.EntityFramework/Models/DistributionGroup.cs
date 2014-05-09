using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class DistributionGroup
    {
        public int ID { get; set; }
        public string DistinguishedName { get; set; }
        public string CompanyCode { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool Hidden { get; set; }
    }
}
