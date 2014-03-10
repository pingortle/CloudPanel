using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Citrix
{
    public class ApplicationsObject
    {
        public int CitrixPlanID { get; set; }

        public string DisplayName { get; set; }

        public string GroupName { get; set; }

        public string Description { get; set; }

        public string CompanyCode { get; set; }

        public string Price { get; set; }

        public string Cost { get; set; }

        public string PictureURL { get; set; }

        public bool IsServer { get; set; }
    }
}
