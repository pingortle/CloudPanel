using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Plans
{
    public class ArchivePlanObject
    {
        public int ArchivePlanID { get; set; }

        public int ArchiveQuotaInMB { get; set; }

        public int ArchiveWarningQuotaInMB { get; set; }

        public string Cost { get; set; }

        public string Price { get; set; }
    }
}
