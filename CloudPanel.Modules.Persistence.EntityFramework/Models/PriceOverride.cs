using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class PriceOverride
    {
        public string CompanyCode { get; set; }
        public string Price { get; set; }
        public Nullable<int> PlanID { get; set; }
        public string Product { get; set; }
    }
}
