using System;
using System.Collections.Generic;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models
{
    public partial class Company
    {
        public int CompanyId { get; set; }
        public bool IsReseller { get; set; }
        public string ResellerCode { get; set; }
        public Nullable<int> OrgPlanID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string AdminName { get; set; }
        public string AdminEmail { get; set; }
        public string DistinguishedName { get; set; }
        public System.DateTime Created { get; set; }
        public bool ExchEnabled { get; set; }
        public Nullable<bool> LyncEnabled { get; set; }
        public Nullable<bool> CitrixEnabled { get; set; }
        public Nullable<int> ExchPFPlan { get; set; }
        public string Country { get; set; }
        public Nullable<bool> ExchPermFixed { get; set; }
    }
}
