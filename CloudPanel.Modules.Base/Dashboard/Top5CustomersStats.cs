using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Dashboard
{
    public class Top5CustomersStats
    {
        public string CustomerName { get; set; }

        public int Users { get; set; }

        public int Mailboxes { get; set; }

        public int MailboxUsedInGB { get; set; }

        public int MailboxAllocatedInGB { get; set; }
    }
}
