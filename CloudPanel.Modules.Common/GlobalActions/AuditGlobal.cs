using CloudPanel.Modules.Base.Auditing;
using CloudPanel.Modules.Base.Enumerations;
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

        public static void AddAudit(string companyCode, string username, ActionID actionID, string variable1, string variable2 = null)
        {
            CPDatabase database = null;
            
            try
            {
                database = new CPDatabase();

                Audit newAudit = new Audit();
                newAudit.CompanyCode = companyCode;
                newAudit.Username = username;
                newAudit.Date = DateTime.Now;
                newAudit.ActionID = (int)actionID;
                newAudit.Variable1 = variable1;
                newAudit.Variable2 = variable2;

                database.Audits.Add(newAudit);
                database.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Info("Failed to add audit to database: " + actionID.ToString(), ex);
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public static List<Audits> RetrieveAudits(string companyCode)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                List<Audit> audits = null;

                if (string.IsNullOrEmpty(companyCode))
                    audits = (from a in database.Audits
                              orderby a.Date descending
                              select a).ToList();
                else
                    audits = (from a in database.Audits
                              where a.CompanyCode == companyCode
                              orderby a.Date descending
                              select a).ToList();

                if (audits == null)
                    return null;
                else
                {
                    List<Audits> foundAudits = new List<Audits>();
                    foreach (var a in audits)
                    {
                        foundAudits.Add(new Audits()
                        {
                            AuditID = a.AuditID,
                            Username = a.Username,
                            WhenEntered = a.Date,
                            CompanyCode = a.CompanyCode,
                            Action = (ActionID)a.ActionID,
                            Variable1 = a.Variable1,
                            Variable2 = a.Variable2
                        });
                    }

                    return foundAudits;
                }
            }
            catch (Exception ex)
            {
                logger.Error("Failed to retrieve audits from the database", ex);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }
    }
}
