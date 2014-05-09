using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class Stats_CitrixCount
    {
        public System.DateTime StatDate { get; set; }
        public int UserCount { get; set; }
    }
}
