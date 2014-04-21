using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Base.Plans;
using CloudPanel.Modules.Common.Database;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class PlanMailboxViewModel : IViewModel
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<MailboxPlanObject> GetPlans()
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var plans = from p in database.Plans_ExchangeMailbox
                            orderby p.MailboxPlanName
                            select new MailboxPlanObject()
                            {
                                MailboxPlanID = p.MailboxPlanID,
                                MailboxPlanName = p.MailboxPlanName,
                                MailboxPlanDescription = p.MailboxPlanDesc,
                                MaxRecipients = p.MaxRecipients,
                                MaxKeepDeletedItemsInDays = p.MaxKeepDeletedItems,
                                MailboxSizeInMB = p.MailboxSizeMB,
                                MaxMailboxSizeInMB = p.MaxMailboxSizeMB == null ? p.MailboxSizeMB : (int)p.MaxMailboxSizeMB,
                                MaxSendInKB = p.MaxSendKB,
                                MaxReceiveInKB = p.MaxReceiveKB,
                                EnablePOP3 = p.EnablePOP3,
                                EnableIMAP = p.EnableIMAP,
                                EnableOWA = p.EnableOWA,
                                EnableMAPI = p.EnableMAPI,
                                EnableAS = p.EnableAS,
                                EnableECP = p.EnableECP,
                                Cost = p.Cost,
                                Price = p.Price,
                                AdditionalGBPrice = p.AdditionalGBPrice
                            };

                return plans.ToList();
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving mailbox plans", ex);
                ThrowEvent(AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public MailboxPlanObject GetPlan(int planID)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var plan = (from p in database.Plans_ExchangeMailbox
                             where p.MailboxPlanID == planID
                             select new MailboxPlanObject()
                             {
                                 MailboxPlanID = p.MailboxPlanID,
                                 MailboxPlanName = p.MailboxPlanName,
                                 CompanyCode = p.CompanyCode,
                                 MailboxPlanDescription = p.MailboxPlanDesc,
                                 MaxRecipients = p.MaxRecipients,
                                 MaxKeepDeletedItemsInDays = p.MaxKeepDeletedItems,
                                 MailboxSizeInMB = p.MailboxSizeMB,
                                 MaxMailboxSizeInMB = p.MaxMailboxSizeMB == null ? p.MailboxSizeMB : (int)p.MaxMailboxSizeMB,
                                 MaxSendInKB = p.MaxSendKB,
                                 MaxReceiveInKB = p.MaxReceiveKB,
                                 EnablePOP3 = p.EnablePOP3,
                                 EnableIMAP = p.EnableIMAP,
                                 EnableOWA = p.EnableOWA,
                                 EnableMAPI = p.EnableMAPI,
                                 EnableAS = p.EnableAS,
                                 EnableECP = p.EnableECP,
                                 Cost = p.Cost,
                                 Price = p.Price,
                                 AdditionalGBPrice = p.AdditionalGBPrice
                             }).First();

                return plan;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving mailbox plans", ex);
                ThrowEvent(AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public bool CreatePlan(MailboxPlanObject obj)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                Plans_ExchangeMailbox plan = new Plans_ExchangeMailbox();
                plan.MailboxPlanName = obj.MailboxPlanName;
                plan.CompanyCode = obj.CompanyCode;
                plan.MailboxPlanDesc = obj.MailboxPlanDescription;
                plan.MaxRecipients = obj.MaxRecipients;
                plan.MaxKeepDeletedItems = obj.MaxKeepDeletedItemsInDays;
                plan.MailboxSizeMB = obj.MailboxSizeInMB;
                plan.MaxMailboxSizeMB = obj.MaxMailboxSizeInMB;
                plan.MaxSendKB = obj.MaxSendInKB;
                plan.MaxReceiveKB = obj.MaxReceiveInKB;
                plan.EnablePOP3 = obj.EnablePOP3;
                plan.EnableIMAP = obj.EnableIMAP;
                plan.EnableOWA = obj.EnableOWA;
                plan.EnableMAPI = obj.EnableMAPI;
                plan.EnableAS = obj.EnableAS;
                plan.EnableECP = obj.EnableECP;
                plan.Cost = obj.Cost;
                plan.Price = obj.Price;
                plan.AdditionalGBPrice = obj.AdditionalGBPrice;

                database.Plans_ExchangeMailbox.Add(plan);
                database.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error creating new mailbox plan", ex);
                ThrowEvent(AlertID.FAILED, ex.Message);

                return false;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public bool UpdatePlan(MailboxPlanObject obj)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var plan = (from p in database.Plans_ExchangeMailbox
                            where p.MailboxPlanID == obj.MailboxPlanID
                            select p).First();

                plan.MailboxPlanName = obj.MailboxPlanName;
                plan.CompanyCode = obj.CompanyCode;
                plan.MailboxPlanDesc = obj.MailboxPlanDescription;
                plan.MaxRecipients = obj.MaxRecipients;
                plan.MaxKeepDeletedItems = obj.MaxKeepDeletedItemsInDays;
                plan.MailboxSizeMB = obj.MailboxSizeInMB;
                plan.MaxMailboxSizeMB = obj.MaxMailboxSizeInMB;
                plan.MaxSendKB = obj.MaxSendInKB;
                plan.MaxReceiveKB = obj.MaxReceiveInKB;
                plan.EnablePOP3 = obj.EnablePOP3;
                plan.EnableIMAP = obj.EnableIMAP;
                plan.EnableOWA = obj.EnableOWA;
                plan.EnableMAPI = obj.EnableMAPI;
                plan.EnableAS = obj.EnableAS;
                plan.EnableECP = obj.EnableECP;
                plan.Cost = obj.Cost;
                plan.Price = obj.Price;
                plan.AdditionalGBPrice = obj.AdditionalGBPrice;

                database.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error saving mailbox plan id " + obj.MailboxPlanID, ex);
                ThrowEvent(AlertID.FAILED, ex.Message);

                return false;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public bool DeletePlan(int planID)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var usingPlan = (from u in database.Users
                                 where u.MailboxPlan == planID
                                 select u).Count();

                if (usingPlan > 0)
                {
                    ThrowEvent(AlertID.FAILED, "The plan is in use " + planID.ToString());
                    return false;
                }
                else
                {
                    var plan = (from p in database.Plans_ExchangeMailbox
                                where p.MailboxPlanID == planID
                                select p).First();

                    database.Plans_ExchangeMailbox.Remove(plan);
                    database.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Error deleting mailbox plan id " + planID, ex);
                ThrowEvent(AlertID.FAILED, ex.Message);

                return false;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public List<CompanyObject> GetCompanies()
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var companies = from c in database.Companies
                                where !c.IsReseller
                                orderby c.CompanyName
                                select new CompanyObject()
                                {
                                    CompanyCode = c.CompanyCode,
                                    CompanyName = c.CompanyName
                                };

                if (companies != null)
                    return companies.ToList();
                else
                    return null;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving companies", ex);
                ThrowEvent(AlertID.FAILED, ex.Message);
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
