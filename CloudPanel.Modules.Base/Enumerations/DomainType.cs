using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Base.Enumerations
{
    public enum DomainType
    {
        Unknown = -1,
        AuthoritativeDomain = 0,
        InternalRelayDomain = 1,
        ExternalRelayDomain = 2,
        BasicDomain = 3,
        SubDomain = 4
    }
}
