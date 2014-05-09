using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class Plans_Citrix
    {
        public int CitrixPlanID { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public bool IsServer { get; set; }
        public string CompanyCode { get; set; }
        public string Price { get; set; }
        public string Cost { get; set; }
        public string PictureURL { get; set; }
    }
}
