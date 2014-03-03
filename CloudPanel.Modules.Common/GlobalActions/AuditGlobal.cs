using CloudPanel.Modules.Common.Database;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.Common.GlobalActions
{
    public static class AuditGlobal
    {
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static void AddAudit(string companyCode, string username, string message)
        {
            CPDatabase database = null;
            
            try
            {
                database = new CPDatabase();

                Audit newAudit = new Audit();
                newAudit.CompanyCode = companyCode;
                newAudit.Username = username;
                newAudit.Message = message;
                newAudit.Date = DateTime.Now;

                database.Audits.Add(newAudit);
                database.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Info("Failed to add audit to database: " + message, ex);
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }
    }
}
