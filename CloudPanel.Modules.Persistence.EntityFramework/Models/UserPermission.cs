using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class UserPermission
    {
        public int UserID { get; set; }
        public bool EnableExchange { get; set; }
        public bool DisableExchange { get; set; }
        public bool AddDomain { get; set; }
        public bool DeleteDomain { get; set; }
        public bool EnableAcceptedDomain { get; set; }
        public bool DisableAcceptedDomain { get; set; }
    }
}
