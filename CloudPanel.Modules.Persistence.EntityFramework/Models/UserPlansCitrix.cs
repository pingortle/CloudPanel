using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class UserPlansCitrix
    {
        public int UPCID { get; set; }
        public int UserID { get; set; }
        public int CitrixPlanID { get; set; }
    }
}
