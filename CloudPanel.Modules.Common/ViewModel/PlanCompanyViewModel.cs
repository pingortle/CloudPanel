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
    public class PlanCompanyViewModel : IViewModel
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CompanyPlanObject GetPlan(int planID)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var foundPlans = (from a in database.Plans_Organization
                                  where a.OrgPlanID == planID
                                  select new CompanyPlanObject()
                                  {
                                      CompanyPlanID = a.OrgPlanID,
                                      CompanyPlanName = a.OrgPlanName,
                                      MaxUser = a.MaxUsers,
                                      MaxDomains = a.MaxDomains,
                                      MaxExchangeMailboxes = a.MaxExchangeMailboxes,
                                      MaxExchangeContacts = a.MaxExchangeContacts,
                                      MaxExchangeDistributionGroups = a.MaxExchangeDistLists,
                                      MaxExchangeMailPublicFolders = a.MaxExchangeMailPublicFolders,
                                      MaxExchangeResourceMailboxes = a.MaxExchangeResourceMailboxes == null ? 0 : (int)a.MaxExchangeResourceMailboxes
                                  }).FirstOrDefault();

                if (foundPlans != null)
                    return foundPlans;
                else
                    return null;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving organization plan " + planID.ToString(), ex);
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public List<CompanyPlanObject> GetAllPlans()
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var foundPlans = (from a in database.Plans_Organization
                                  orderby a.OrgPlanName
                                  select new CompanyPlanObject()
                                  {
                                       CompanyPlanID = a.OrgPlanID,
                                       CompanyPlanName = a.OrgPlanName,
                                       MaxUser = a.MaxUsers,
                                       MaxDomains = a.MaxDomains,
                                       MaxExchangeMailboxes = a.MaxExchangeMailboxes,
                                       MaxExchangeContacts = a.MaxExchangeContacts,
                                       MaxExchangeDistributionGroups = a.MaxExchangeDistLists,
                                       MaxExchangeMailPublicFolders = a.MaxExchangeMailPublicFolders,
                                       MaxExchangeResourceMailboxes = a.MaxExchangeResourceMailboxes == null ? 0 : (int)a.MaxExchangeResourceMailboxes
                                  }); 

                if (foundPlans != null)
                    return foundPlans.ToList();
                else
                    return null;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving organization plans from the database", ex);
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public void Create(CompanyPlanObject plan)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                Plans_Organization newPlan = new Plans_Organization();
                newPlan.OrgPlanName = plan.CompanyPlanName;
                newPlan.MaxUsers = plan.MaxUser;
                newPlan.MaxDomains = plan.MaxDomains;
                newPlan.MaxExchangeMailboxes = plan.MaxExchangeMailboxes;
                newPlan.MaxExchangeContacts = plan.MaxExchangeContacts;
                newPlan.MaxExchangeDistLists = plan.MaxExchangeDistributionGroups;
                newPlan.MaxExchangeResourceMailboxes = plan.MaxExchangeResourceMailboxes;
                newPlan.MaxExchangeMailPublicFolders = plan.MaxExchangeMailPublicFolders;

                database.Plans_Organization.Add(newPlan);
                database.SaveChanges();
            }
            catch (Exception ex)
            {
                this.logger.Error("Error saving new company plan " + plan.CompanyPlanName + " to the database.", ex);
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public void Update(CompanyPlanObject plan)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var findPlan = (from p in database.Plans_Organization
                                where p.OrgPlanID == plan.CompanyPlanID
                                select p).FirstOrDefault();

                findPlan.OrgPlanName = plan.CompanyPlanName;
                findPlan.MaxUsers = plan.MaxUser;
                findPlan.MaxDomains = plan.MaxDomains;
                findPlan.MaxExchangeMailboxes = plan.MaxExchangeMailboxes;
                findPlan.MaxExchangeContacts = plan.MaxExchangeContacts;
                findPlan.MaxExchangeDistLists = plan.MaxExchangeDistributionGroups;
                findPlan.MaxExchangeMailboxes = plan.MaxExchangeMailboxes;
                findPlan.MaxExchangeMailPublicFolders = plan.MaxExchangeMailPublicFolders;

                database.SaveChanges();
            }
            catch (Exception ex)
            {
                this.logger.Error("Error updating company plan " + plan.CompanyPlanName + " with id " + plan.CompanyPlanID, ex);
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public void Delete(int planID)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                // Find out if it is in use
                int? inUseNumber = NumberOfTimesPlanInUse(planID);
                if (inUseNumber != null && inUseNumber > 0)
                    ThrowEvent(Base.Enumerations.AlertID.PLAN_IN_USE, inUseNumber == null ? "-1" : inUseNumber.ToString());
                else
                {
                    var deletePlan = (from p in database.Plans_Organization
                                      where p.OrgPlanID == planID
                                      select p).FirstOrDefault();

                    if (deletePlan != null)
                    {
                        database.Plans_Organization.Remove(deletePlan);
                        database.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Error deleting company plan " + planID.ToString(), ex);
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        private int? NumberOfTimesPlanInUse(int planID)
        {
            CPDatabase database = null;

            try
            {
                this.logger.Debug("Checking if plan is in use: " + planID.ToString());

                database = new CPDatabase();

                var findPlan = (from p in database.Companies
                                where p.OrgPlanID == planID
                                select p).Count();

                return findPlan;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error counting the number of times plan " + planID.ToString() + " is in use.", ex);
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
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
