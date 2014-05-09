using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class SvcMailboxDatabaseSize
    {
        public int ID { get; set; }
        public string DatabaseName { get; set; }
        public string Server { get; set; }
        public string DatabaseSize { get; set; }
        public System.DateTime Retrieved { get; set; }
    }
}
