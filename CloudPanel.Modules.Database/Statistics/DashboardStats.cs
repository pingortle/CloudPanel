using CloudPanel.Modules.Base.Dashboard;
using CloudPanel.Modules.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Database.Statistics
{
    public class DashboardStats
    {
        /// <summary>
        /// Gets the environment overview for statistics
        /// </summary>
        /// <returns></returns>
        public static OverviewStats GetDashboardStatistics()
        {
            DB database = null;

            try
            {
                database = new DB();

                var totalUsers = (from s in database.Users
                                  select s.ID).Count();

                var totalResellers = (from r in database.Companies
                                      where r.IsReseller
                                      select r.CompanyId).Count();

                var totalCompanies = (from c in database.Companies
                                      where !c.IsReseller
                                      select c.CompanyId).Count();

                var totalMailboxes = (from m in database.Users
                                      where m.MailboxPlan > 0
                                      select m.ID).Count();

                OverviewStats stats = new OverviewStats();
                stats.TotalUsers = totalUsers;
                stats.Resellers = totalResellers;
                stats.Companies = totalCompanies;
                stats.Mailboxes = totalMailboxes;

                return stats;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }
    }
}
