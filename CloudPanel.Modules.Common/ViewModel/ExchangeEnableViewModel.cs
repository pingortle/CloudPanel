using CloudPanel.Modules.Base.Exchange;
using CloudPanel.Modules.Common.Database;
using CloudPanel.Modules.Common.Rollback;
using CloudPanel.Modules.Common.Settings;
using CloudPanel.Modules.Exchange;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class ExchangeEnableViewModel : IViewModel
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void EnableExchange(string companyCode)
        {
            ExchangePowershell powershell = null;
            CPDatabase database = null;

            CloudPanelTransaction newTransaction = new CloudPanelTransaction();
            try
            {
                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);

                // Enable Exchange objects for company
                powershell.NewGlobalAddressList(RecipientFilters.GetGALName(companyCode), RecipientFilters.GetGALFilter(companyCode));
                newTransaction.NewExchangeGAL(RecipientFilters.GetGALName(companyCode));

                powershell.NewAddressList(RecipientFilters.GetUsersName(companyCode), RecipientFilters.GetUsersFilter(companyCode));
                newTransaction.NewExchangeAddressList(RecipientFilters.GetUsersName(companyCode));

                powershell.NewAddressList(RecipientFilters.GetGroupsName(companyCode), RecipientFilters.GetGroupsFilter(companyCode));
                newTransaction.NewExchangeAddressList(RecipientFilters.GetGroupsName(companyCode));

                powershell.NewAddressList(RecipientFilters.GetContactsName(companyCode), RecipientFilters.GetContactsFilter(companyCode));
                newTransaction.NewExchangeAddressList(RecipientFilters.GetContactsName(companyCode));

                powershell.NewAddressList(RecipientFilters.GetRoomName(companyCode), RecipientFilters.GetRoomFilter(companyCode));
                newTransaction.NewExchangeAddressList(RecipientFilters.GetRoomName(companyCode));

                powershell.NewOfflineAddressBook(RecipientFilters.GetOALName(companyCode),  RecipientFilters.GetGALName(companyCode), "AllUsers@ " + companyCode);
                newTransaction.NewExchangeOAB(RecipientFilters.GetOALName(companyCode));

                powershell.NewAddressBookPolicy(RecipientFilters.GetABPName(companyCode), RecipientFilters.GetGALName(companyCode), RecipientFilters.GetOALName(companyCode), RecipientFilters.GetRoomName(companyCode), RecipientFilters.GetABPAddressLists(companyCode));
                newTransaction.NewExchangeABP(RecipientFilters.GetABPName(companyCode));

                database = new CPDatabase();
                var dn = (from c in database.Companies
                          where !c.IsReseller
                          where c.CompanyCode == companyCode
                          select c).First();

                powershell.NewSecurityDistributionGroup("ExchangeSecurity@" + companyCode, "AllUsers@" + companyCode, companyCode, "OU=Exchange," + dn.DistinguishedName);
                newTransaction.NewExchangeGroup("ExchangeSecurity@" + companyCode);

                // Set Exchange is enabled
                dn.ExchEnabled = true;
                dn.ExchPermFixed = true;

                database.SaveChanges();
            }
            catch (Exception ex)
            {
                this.logger.Error("Error disabling Exchange for company " + companyCode, ex);

                newTransaction.RollBack();

                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
            }
            finally
            {
                if (database != null)
                    database.Dispose();

                if (powershell != null)
                    powershell.Dispose();
            }
        }
    }
}
