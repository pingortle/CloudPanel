using CloudPanel.Modules.Common.Database;
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
    public class ExchangeDisableViewModel : IViewModel
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void DisableExchange(string companyCode)
        {
            ExchangePowershell powershell = null;
            CPDatabase database = null;

            try
            {
                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);

                // Disable all exchange objects
                powershell.DeleteAllMailboxes(companyCode);
                powershell.DeleteAllContacts(companyCode);
                powershell.DeleteAllGroups(companyCode);
                powershell.DeleteAddressBookPolicy(companyCode + " ABP");
                powershell.DeleteOfflineAddressBook(companyCode + " OAL");
                powershell.DeleteAddressList(companyCode + " - All Rooms");
                powershell.DeleteAddressList(companyCode + " - All Contacts");
                powershell.DeleteAddressList(companyCode + " - All Groups");
                powershell.DeleteAddressList(companyCode + " - All Users");
                powershell.DeleteGlobalAddressList(companyCode + " - GAL");

                // Get all accepted domains
                this.logger.Debug("Retrieving list of accepted domains for " + companyCode);

                database = new CPDatabase();
                var domains = from d in database.Domains
                              where d.IsAcceptedDomain
                              where d.CompanyCode == companyCode
                              select d;

                if (domains != null)
                {
                    foreach (Domain d in domains)
                        powershell.DeleteDomain(d.Domain1);
                }

                // Now update the database
                int r = database.DisableExchange(companyCode);
                this.logger.Debug("Total count returned when calling DisableExchange stored procedure: " + r.ToString());

            }
            catch (Exception ex)
            {
                this.logger.Error("Error disabling Exchange for company " + companyCode, ex);
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
