using CloudPanel.Modules.Base.Auditing;
using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Base.Plans;
using CloudPanel.Modules.Base.Statistics;
using CloudPanel.Modules.Common.Database;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class CompanyOverviewViewModel : IViewModel
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CompanyObject GetCompany(string companyCode)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var foundCompany = (from c in database.Companies
                                    where !c.IsReseller
                                    where c.CompanyCode == companyCode
                                    select new CompanyObject
                                    {
                                         CompanyName = c.CompanyName,
                                         CompanyCode = companyCode,
                                         AdminName = c.AdminName,
                                         Telephone = c.PhoneNumber,
                                         Created = c.Created,
                                         Street = c.Street,
                                         City = c.City,
                                         State = c.State,
                                         ZipCode = c.ZipCode,
                                         Country = c.Country,
                                         CompanyPlanID = c.OrgPlanID == null ? 0 : (int)c.OrgPlanID
                                     }).FirstOrDefault();

                if (foundCompany != null)
                {
                    // Get the company plan details
                    var companyPlan = (from p in database.Plans_Organization
                                       where p.OrgPlanID == foundCompany.CompanyPlanID
                                       select new CompanyPlanObject
                                       {
                                           CompanyPlanID = p.OrgPlanID,
                                           CompanyPlanName = p.OrgPlanName,
                                           MaxUser = p.MaxUsers,
                                           MaxDomains = p.MaxDomains,
                                           MaxExchangeMailboxes = p.MaxExchangeMailboxes,
                                           MaxExchangeContacts = p.MaxExchangeContacts,
                                           MaxExchangeDistributionGroups = p.MaxExchangeDistLists,
                                           MaxExchangeActivesyncPolicies = p.MaxExchangeActivesyncPolicies == null ? 1 : (int)p.MaxExchangeActivesyncPolicies,
                                           MaxCitrixUsers = p.MaxTerminalServerUsers,
                                           MaxExchangeResourceMailboxes = p.MaxExchangeResourceMailboxes == null ? 1 : (int)p.MaxExchangeResourceMailboxes
                                       }).FirstOrDefault();

                    if (companyPlan != null)
                        foundCompany.CompanyPlanObject = companyPlan;

                    return foundCompany;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving company from the database for " + companyCode, ex);
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public OverallStats GetCompanyStats(string companyCode)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                OverallStats companyStats = new OverallStats();

                companyStats.TotalUsers = (from s in database.Users
                                           where s.CompanyCode == companyCode
                                           select s.UserPrincipalName).Count();

                companyStats.TotalDomains = (from d in database.Domains
                                             where d.CompanyCode == companyCode
                                             select d.Domain1).Count();

                companyStats.TotalMailboxes = (from m in database.Users
                                               where m.CompanyCode == companyCode
                                               where m.MailboxPlan > 0
                                               select m.UserPrincipalName).Count();

                companyStats.TotalLyncUsers = (from l in database.Users
                                               where l.CompanyCode == companyCode
                                               where l.LyncPlan > 0
                                               select l.UserPrincipalName).Count();


                return companyStats;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving company statistics from the database for " + companyCode, ex);
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public List<CompanyPlanObject> GetCompanyPlans(string companyCode)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var getAllPlans = from p in database.Plans_Organization
                                  orderby p.OrgPlanName
                                  select new CompanyPlanObject
                                  {
                                       CompanyPlanID = p.OrgPlanID,
                                       CompanyPlanName = p.OrgPlanName
                                  };

                if (getAllPlans != null)
                {
                    List<CompanyPlanObject> objects = getAllPlans.ToList();
                    return objects;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving company plans from the database for " + companyCode, ex);
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
