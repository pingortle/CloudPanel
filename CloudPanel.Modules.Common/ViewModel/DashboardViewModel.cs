using CloudPanel.Modules.Base.Auditing;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Base.Statistics;
using CloudPanel.Modules.Common.Database;
using log4net;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class DashboardViewModel : IViewModel
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region Area Chart

        public List<Dictionary<string, object>> GetAreaChart()
        {
            CPDatabase database = null;
            try
            {
                database = new CPDatabase();

                // List to contains our dictionary object
                List<Dictionary<string, object>> statistics = new List<Dictionary<string, object>>();

                // To abbreviate the months
                DateTimeFormatInfo mfi = new DateTimeFormatInfo();

                // Object for today
                DateTime todaysDate = DateTime.Today;

                //
                // Start with the users
                //
                Dictionary<string, object> usersStatistics = new Dictionary<string, object>();
                for (int i = 11; i >= 0; i--)
                {
                    DateTime newDate = todaysDate.AddMonths(-i);
                    GetUserHistory(ref database, newDate, ref mfi, ref usersStatistics);
                }
                statistics.Add(usersStatistics);

                //
                // Get mailbox stats
                //
                Dictionary<string, object> mailboxStatistics = new Dictionary<string, object>();
                for (int i = 11; i >= 0; i--)
                {
                    DateTime newDate = todaysDate.AddMonths(-i);
                    GetMailboxHistory(ref database, newDate, ref mfi, ref mailboxStatistics);
                }
                statistics.Add(mailboxStatistics);

                //
                // Get Citrix stats
                //
                Dictionary<string, object> citrixStatistics = new Dictionary<string, object>();
                for (int i = 11; i >= 0; i--)
                {
                    DateTime newDate = todaysDate.AddMonths(-i);
                    GetCitrixHistory(ref database, newDate, ref mfi, ref citrixStatistics);
                }
                statistics.Add(citrixStatistics);

                return statistics;
            }
            catch (Exception ex)
            {
                ThrowEvent(AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }

        }

        private void GetUserHistory(ref CPDatabase database, DateTime current, ref DateTimeFormatInfo mfi, ref Dictionary<string, object> statistics)
        {
            // Get first day of month
            DateTime firstDay = new DateTime(current.Year, current.Month, 1);
            DateTime lastDay = new DateTime(current.Year, current.Month, DateTime.DaysInMonth(current.Year, current.Month));

            var users = new int();
            if (current.Year == DateTime.Today.Year && current.Month == DateTime.Today.Month)
            {
                // If we are doing todays month then we just count the total amount of users
                users = (from u in database.Users
                         select u).Count();
            }
            else
            {
                // Otherwise we are on other months and we need to look at statistics
                users = (from u in database.Stats_UserCount
                             where u.StatDate >= firstDay
                             where u.StatDate <= lastDay
                             orderby u.StatDate ascending
                             select u.UserCount).FirstOrDefault();
            }

            if (users < 1)
            {
                // If this is the very first point and it was null then we need to set it to zero so it will populate the whole graph
                if (firstDay < DateTime.Now.AddMonths(-11))
                    statistics.Add(string.Format("{0} {1}", mfi.GetAbbreviatedMonthName(current.Month), current.Year), 0);
                else
                    statistics.Add(string.Format("{0} {1}", mfi.GetAbbreviatedMonthName(current.Month), current.Year), null);
            }
            else
                statistics.Add(string.Format("{0} {1}", mfi.GetAbbreviatedMonthName(current.Month), current.Year), users);
        }

        private void GetMailboxHistory(ref CPDatabase database, DateTime current, ref DateTimeFormatInfo mfi, ref Dictionary<string, object> statistics)
        {
            // Get first day of month
            DateTime firstDay = new DateTime(current.Year, current.Month, 1);
            DateTime lastDay = new DateTime(current.Year, current.Month, DateTime.DaysInMonth(current.Year, current.Month));

            var users = new int();
            if (current.Year == DateTime.Today.Year && current.Month == DateTime.Today.Month)
            {
                // If we are doing todays month then we just count the total amount of mailboxes
                users = (from u in database.Users
                         where u.MailboxPlan > 0
                         select u).Count();
            }
            else
            {
                // Otherwise we are on other months and we need to look at statistics
                users = (from u in database.Stats_ExchCount
                         where u.StatDate >= firstDay
                         where u.StatDate <= lastDay
                         orderby u.StatDate ascending
                         select u.UserCount).FirstOrDefault();
            }

            if (users < 1)
            {
                // If this is the very first point and it was null then we need to set it to zero so it will populate the whole graph
                if (firstDay < DateTime.Now.AddMonths(-11))
                    statistics.Add(string.Format("{0} {1}", mfi.GetAbbreviatedMonthName(current.Month), current.Year), 0);
                else
                    statistics.Add(string.Format("{0} {1}", mfi.GetAbbreviatedMonthName(current.Month), current.Year), null);
            }
            else
                statistics.Add(string.Format("{0} {1}", mfi.GetAbbreviatedMonthName(current.Month), current.Year), users);
        }

        private void GetCitrixHistory(ref CPDatabase database, DateTime current, ref DateTimeFormatInfo mfi, ref Dictionary<string, object> statistics)
        {
            // Get first day of month
            DateTime firstDay = new DateTime(current.Year, current.Month, 1);
            DateTime lastDay = new DateTime(current.Year, current.Month, DateTime.DaysInMonth(current.Year, current.Month));

            var users = new int();
            if (current.Year == DateTime.Today.Year && current.Month == DateTime.Today.Month)
            {
                // If we are doing todays month then we just count the total amount of citrix users
                users = (from u in database.UserPlansCitrices
                         select u.UserID).Distinct().Count();
            }
            else
            {
                // Otherwise we are on other months and we need to look at statistics
                users = (from u in database.Stats_CitrixCount
                         where u.StatDate >= firstDay
                         where u.StatDate <= lastDay
                         orderby u.StatDate ascending
                         select u.UserCount).FirstOrDefault();
            }

            if (users < 1)
            {
                // If this is the very first point and it was null then we need to set it to zero so it will populate the whole graph
                if (firstDay < DateTime.Now.AddMonths(-11))
                    statistics.Add(string.Format("{0} {1}", mfi.GetAbbreviatedMonthName(current.Month), current.Year), 0);
                else
                    statistics.Add(string.Format("{0} {1}", mfi.GetAbbreviatedMonthName(current.Month), current.Year), null);
            }
            else
                statistics.Add(string.Format("{0} {1}", mfi.GetAbbreviatedMonthName(current.Month), current.Year), users);
        }

        #endregion

        #region Mailbox Database Size

        public Dictionary<string, object> GetDatabaseSizeChart(string entityConnectionString)
        {
            SqlConnection sql = null;
            SqlCommand cmd = null;

            Dictionary<string, object> databaseSizes = new Dictionary<string, object>();
            try
            {
                string providerConnectionString = new EntityConnectionStringBuilder(entityConnectionString).ProviderConnectionString;

                
                sql = new SqlConnection(providerConnectionString);
                cmd = new SqlCommand(@"SELECT
	                                        DatabaseName,
	                                        DatabaseSize,
	                                        Retrieved
                                        FROM
	                                        SvcMailboxDatabaseSizes
                                        WHERE
	                                        CONVERT(date, Retrieved) = (SELECT TOP 1 CONVERT(date, Retrieved) FROM SvcMailboxDatabaseSizes ORDER BY Retrieved DESC)
                                        ORDER BY
	                                        DatabaseName", sql);

                sql.Open();
                
                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        string databaseSetSize = r["DatabaseSize"].ToString();
                        string formattedSize = string.Format(CultureInfo.InvariantCulture, "{0:0.00}", (decimal.Parse(databaseSetSize, CultureInfo.InvariantCulture) / 1024 / 1024));

                        string databaseName = string.Format("{0}<br/>[{1}]", r["DatabaseName"].ToString(), DateTime.Parse(r["Retrieved"].ToString()).ToShortDateString());

                        databaseSizes.Add(databaseName, formattedSize);
                    }
                }

                r.Close();
                r.Dispose();

                return databaseSizes;
            }
            catch (Exception ex)
            {
                logger.Error("Error getting Exchange database sizes", ex);
                return null;
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (sql != null)
                    sql.Dispose();
            }
        }

        #endregion

        #region Get Other Statistics

        public OverallStats GetOtherStatistics(string entityConnectionString)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                OverallStats stats = null;

                // Get all users
                var users = from u in database.Users
                            select u;

                // Don't continue we don't find any users
                if (users != null)
                {
                    stats = new OverallStats();
                    stats.TotalUsers = users.Count();
                    stats.TodayUsers = (from u in users where DbFunctions.TruncateTime(u.Created) == DbFunctions.TruncateTime(DateTime.Now) select u.UserPrincipalName).Count();
                    stats.TotalMailboxes = (from u in users where u.MailboxPlan > 0 select u.UserPrincipalName).Count();
                    stats.TotalLyncUsers = (from u in users where u.LyncPlan > 0 select u.UserPrincipalName).Count();

                    // Get all companies
                    var companies = from c in database.Companies
                                    select c;

                    if (companies != null)
                    {
                        stats.TotalResellers = (from r in companies where r.IsReseller select r.CompanyCode).Count();
                        stats.TotalCompanies = (from c in companies where !c.IsReseller select c.CompanyCode).Count();
                        stats.TodayCompanies = (from c in companies where DbFunctions.TruncateTime(c.Created) == DbFunctions.TruncateTime(DateTime.Now) select c.CompanyCode).Count();
                    }

                    // Get all domains
                    var domains = from d in database.Domains
                                  select d;

                    if (domains != null)
                    {
                        stats.TotalDomains = (from d in domains select d.Domain1).Count();
                        stats.TotalAcceptedDomains = (from a in domains where a.IsAcceptedDomain select a.Domain1).Count();
                    }

                    // Get other Exchange stats
                    stats.TotalDistributionGroups = (from d in database.DistributionGroups select d.ID).Count();
                    stats.TotalMailContacts = (from c in database.Contacts select c.DistinguishedName).Count();

                    // Get Citrix users
                    var citrix = (from c in database.UserPlansCitrices
                                  select c.UserID).Distinct().Count();
                    stats.TotalCitrixUsers = citrix;

                    // Get Citrix data
                    var citrixApps = from c in database.Plans_Citrix
                                     select c;

                    if (citrixApps != null)
                    {
                        stats.TotalCitrixApps = (from c in citrixApps where !c.IsServer select c).Count();
                        stats.TotalCitrixServers = (from c in citrixApps where c.IsServer select c).Count();
                    }


                    // Get total allocated space
                    GetTotalAllocatedMailboxSize(entityConnectionString, ref stats);

                    // Get total used spaces
                    GetTotalUsedMailboxSize(entityConnectionString, ref stats);
                }

                // Return data
                return stats;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error getting other statistics.", ex);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        private void GetTotalAllocatedMailboxSize(string entityConnectionString, ref OverallStats stats)
        {
            SqlConnection sql = null;
            SqlCommand cmd = null;

            try
            {
                string providerConnectionString = new EntityConnectionStringBuilder(entityConnectionString).ProviderConnectionString;


                sql = new SqlConnection(providerConnectionString);
                cmd = new SqlCommand(@"SELECT UserPrincipalName, MailboxPlan, AdditionalMB, 
                                       (SELECT MailboxSizeMB FROM Plans_ExchangeMailbox WHERE MailboxPlanID=Users.MailboxPlan) AS MailboxPlanSize,
                                       ((SELECT MailboxSizeMB FROM Plans_ExchangeMailbox WHERE MailboxPlanID=Users.MailboxPlan) + AdditionalMB) AS TotalSize
                                       FROM Users WHERE MailboxPlan > 0", sql);

                sql.Open();

                // Keep track of total
                decimal total = 0;

                // Read data
                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        if (r["TotalSize"] == DBNull.Value)
                            total = total + decimal.Parse(r["MailboxPlanSize"].ToString(), CultureInfo.InvariantCulture);
                        else
                            total = total + decimal.Parse(r["TotalSize"].ToString(), CultureInfo.InvariantCulture);
                    }

                    // Convert to GB, TB, etc
                    if (total > 0)
                    {
                        if (total >= 1048576)
                        {
                            stats.TotalAllocatedEmailSpace = (total / 1024) / 1024;
                            stats.TotalAllocatedEmailSpaceSizeType = "TB";
                        }
                        else if (total >= 1024)
                        {
                            stats.TotalAllocatedEmailSpace = total / 1024;
                            stats.TotalAllocatedEmailSpaceSizeType = "GB";
                        }

                        // Convert the MB to KB for the progress bars
                        stats.TotalAllocatedEmailSpaceInKB = total * 1024;
                    }
                }

                r.Close();
                r.Dispose();

                if (total < 0)
                {
                    stats.TotalAllocatedEmailSpaceInKB = 0;
                    stats.TotalAllocatedEmailSpace = 0;
                    stats.TotalAllocatedEmailSpaceSizeType = "?";
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error getting Exchange allocated mailbox sizes", ex);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (sql != null)
                    sql.Dispose();
            }
        }

        private void GetTotalUsedMailboxSize(string entityConnectionString, ref OverallStats stats)
        {
            SqlConnection sql = null;
            SqlCommand cmd = null;

            try
            {
                string providerConnectionString = new EntityConnectionStringBuilder(entityConnectionString).ProviderConnectionString;


                sql = new SqlConnection(providerConnectionString);
                cmd = new SqlCommand(@"SELECT UserPrincipalName, Retrieved, TotalItemSizeInKB FROM SvcMailboxSizes 
                                        WHERE CONVERT(date, Retrieved) = (SELECT TOP 1 CONVERT(date, Retrieved) FROM SvcMailboxSizes ORDER BY Retrieved DESC)
                                        ORDER BY Retrieved DESC", sql);

                sql.Open();

                // Keep track of total
                decimal total = 0;

                // Read data
                SqlDataReader r = cmd.ExecuteReader();
                if (r.HasRows)
                {
                    while (r.Read())
                    {
                        decimal parsed = 0;
                        decimal.TryParse(r["TotalItemSizeInKB"].ToString(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out parsed);

                        total = total + parsed;
                    }

                    // Convert to MB, GB, TB, etc
                    if (total > 0)
                    {
                        if (total >= 1073741824)
                        {
                            stats.TotalUsedEmailSpace = ((total / 1024) / 1024) / 1024;
                            stats.TotalUsedEmailSpaceSizeType = "TB";
                        }
                        else if (total >= 1048576)
                        {
                            stats.TotalUsedEmailSpace = (total / 1024) / 1024;
                            stats.TotalUsedEmailSpaceSizeType = "GB";
                        }
                        else if (total >= 1024)
                        {
                            stats.TotalUsedEmailSpace = total / 1024;
                            stats.TotalUsedEmailSpaceSizeType = "MB";
                        }

                        stats.TotalUsedEmailSpaceInKB = total;
                    }
                }

                r.Close();
                r.Dispose();

                if (total < 0)
                {
                    stats.TotalUsedEmailSpaceInKB = 0;
                    stats.TotalUsedEmailSpace = 0;
                    stats.TotalUsedEmailSpaceSizeType = "?";
                }
            }
            catch (Exception ex)
            {
                logger.Error("Error getting Exchange mailbox sizes", ex);
            }
            finally
            {
                if (cmd != null)
                    cmd.Dispose();

                if (sql != null)
                    sql.Dispose();
            }
        }

        #endregion
    }
}
