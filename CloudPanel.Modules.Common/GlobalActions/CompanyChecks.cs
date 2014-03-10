using CloudPanel.Modules.Common.Database;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.Common.GlobalActions
{
    public class CompanyChecks
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static bool IsExchangeEnabled(string companyCode)
        {
            CPDatabase database = null;

            try
            {
                if (string.IsNullOrEmpty(companyCode))
                    return false;
                else
                {
                    database = new CPDatabase();

                    var isEnabled = (from c in database.Companies
                                     where !c.IsReseller
                                     where c.CompanyCode == companyCode
                                     where c.ExchEnabled
                                     select c.ExchEnabled).Count();

                    if (isEnabled > 0)
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error checking if compay " + companyCode + " is enabled for Exchange", ex);
                return false;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }
    }
}
