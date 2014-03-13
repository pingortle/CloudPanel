using CloudPanel.Modules.Base.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Companies
{
    public class DomainsObject
    {
        public int DomainID { get; set; }

        public DomainType TypeOfDomain { get; set; }

        public string CompanyCode { get; set; }

        public string DomainName { get; set; }

        public bool IsAcceptedDomain { get; set; }

        public bool IsLyncDomain { get; set; }

        public bool IsDefault { get; set; }

        public bool IsSubDomain { get; set; }
    }
}
