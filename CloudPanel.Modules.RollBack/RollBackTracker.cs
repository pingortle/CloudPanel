using CloudPanel.Modules.Base.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.RollBack
{
    public class RollBackTracker
    {
        public RollBackAction Action { get; set; }

        public string Variable1;
        public string Variable2;
    }
}
