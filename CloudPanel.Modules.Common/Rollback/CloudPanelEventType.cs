using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Common.Rollback
{
    public enum CloudPanelEventType
    {
        Create_OrganizationalUnit,
        Create_DistributionGroup,
        Create_SecurityGroup,
        Insert_Company_Into_Database,
        Insert_Reseller_Into_Database,
        Create_NewUser,
        Add_Domain,
        Create_GlobalAddressList,
        Create_ExchangeAddressList,
        Create_OfflineAddressBook,
        Create_AddressBookPolicy,
        Create_SecurityDistributionGroup,
        Create_AcceptedDomain,
        Create_Contact,
        Create_Mailbox
    }
}
