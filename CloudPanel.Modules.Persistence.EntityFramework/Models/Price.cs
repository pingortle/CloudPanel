using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class Price
    {
        public int PriceID { get; set; }
        public int ProductID { get; set; }
        public int PlanID { get; set; }
        public string CompanyCode { get; set; }
        public decimal Price1 { get; set; }
    }
}
