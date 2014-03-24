using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Common.Rollback
{
    public class CloudPanelEvent
    {
        public CloudPanelEventType EventType { get; set; }

        #region Variables

        /// <summary>
        /// Distinguished Name of the object in Active Directory
        /// </summary>
        public string DistinguishedName { get; set; }

        /// <summary>
        /// The name of a group
        /// </summary>
        public string GroupName { get; set; }

        /// <summary>
        /// Company code
        /// </summary>
        public string CompanyCode { get; set; }

        /// <summary>
        /// The userprincipalname of the user
        /// </summary>
        public string UserPrincipalName { get; set; }

        /// <summary>
        /// The domain name used for add domains to companies (or removing)
        /// </summary>
        public string DomainName { get; set; }

        /// <summary>
        /// Used for various methods (such as address lists, gal, etc)
        /// </summary>
        public string Name { get; set; }

        #endregion
    }
}
