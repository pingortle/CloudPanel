using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class AuditLogin
    {
        public int ID { get; set; }
        public string IPAddress { get; set; }
        public string Username { get; set; }
        public bool LoginStatus { get; set; }
        public System.DateTime AuditTimeStamp { get; set; }
    }
}
