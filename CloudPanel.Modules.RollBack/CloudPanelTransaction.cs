using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.RollBack
{
    public class CloudPanelTransaction
    {
        private readonly List<CloudPanelEvent> Events;

        public CloudPanelTransaction()
        {
            Events = new List<CloudPanelEvent>();
        }

        #region Active Directory

        /// <summary>
        /// Registers a new event for creating an organizational unit
        /// </summary>
        /// <param name="distinguishedName"></param>
        public void NewOrganizationalUnitEvent(string distinguishedName)
        {
            Events.Add(new CloudPanelEvent()
            {
                 EventType = CloudPanelEventType.Create_OrganizationalUnit,
                 DistinguishedName = distinguishedName
            });
        }

        /// <summary>
        /// Registers a new event for creating a security group
        /// </summary>
        /// <param name="groupName"></param>
        public void NewSecurityGroup(string groupName)
        {
            Events.Add(new CloudPanelEvent()
            {
                EventType = CloudPanelEventType.Create_SecurityGroup,
                GroupName = groupName
            });
        }

        #endregion

        #region RollBack

        public void RollBack()
        {
            // Roll back all CloudPanelEvents that have occurred
            Events.Reverse();

            foreach (CloudPanelEvent e in Events)
            {
                switch (e.EventType)
                {
                    case CloudPanelEventType.Create_OrganizationalUnit:
                        // Delete Organizational Unit

                        break;
                    case CloudPanelEventType.Create_SecurityGroup:
                        // Delete Security Gorup

                        break;
                    default:
                        break;
                }
            }
        }

        #endregion
    }
}
