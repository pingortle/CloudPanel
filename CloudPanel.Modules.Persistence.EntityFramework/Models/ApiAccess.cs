using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class ApiAccess
    {
        public string CustomerKey { get; set; }
        public string CustomerSecret { get; set; }
        public string CompanyCode { get; set; }
    }
}
