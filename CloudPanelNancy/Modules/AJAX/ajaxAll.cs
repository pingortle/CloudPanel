//
// Copyright (c) 2014, Jacob Dixon
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 3. All advertising materials mentioning features or use of this software
//    must display the following acknowledgement:
//    This product includes software developed by KnowMoreIT and Compsys.
// 4. Neither the name of KnowMoreIT and Compsys nor the
//    names of its contributors may be used to endorse or promote products
//    derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY Jacob Dixon ''AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL Jacob Dixon BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

using CloudPanel.Modules.Base.Plans;
using CloudPanel.Modules.Persistence.EntityFramework;
using CloudPanel.Modules.Persistence.EntityFramework.Models;
using Nancy;
using Nancy.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules.AJAX
{
    public class ajaxAll : NancyModule
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(ajaxAll));

        public ajaxAll() : base("/AJAX")
        {
            //this.RequiresAuthentication();

            Get["/Plans/Company/Get/{ID}"] = parameters => GetCompanyPlan(parameters.ID);
            Get["/Plans/Mailbox/Get/{ID}"] = parameters => GetMailboxPlan(parameters.ID);
            Get["/Company/Charting/Column/{CompanyCode}"] = parameters => GetCompanyColumnChart(parameters.CompanyCode);
            Get["/Company/{CompanyCode}/Users/GetAll"] = parameters => GetUsers(parameters.CompanyCode);
        }

        public Response GetCompanyPlan(string id)
        {
            CloudPanelContext ctx = null;
            try
            {
                ctx = new CloudPanelContext(Settings.ConnectionString);

                int intId = int.Parse(id);
                var plans = (from p in ctx.Plans_Organization
                             where p.OrgPlanID == intId
                             select p).FirstOrDefault();

                return Response.AsJson(plans, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                log.Error("Error retrieving company plans", ex);
                return Response.AsJson(ex.Message, HttpStatusCode.InternalServerError);
            }
            finally
            {
                ctx.Dispose();
            }
        }

        public Response GetMailboxPlan(string id)
        {
            MailboxPlanObject test = new MailboxPlanObject();
            test.MailboxPlanName = "Bronze";
            test.MailboxPlanDescription = "Bronze package includes everything but Activesync";
            test.MaxRecipients = 100;
            test.MaxKeepDeletedItemsInDays = 15;
            test.MailboxSizeInMB = 2048;
            test.MaxMailboxSizeInMB = 4096;
            test.MaxSendInKB = 25600;
            test.MaxReceiveInKB = 25600;
            test.EnablePOP3 = true;
            test.EnableIMAP = true;
            test.EnableOWA = true;
            test.EnableMAPI = true;
            test.EnableAS = false;
            test.EnableECP = true;
            test.Cost = "1.95";
            test.Price = "3.95";
            test.AdditionalGBPrice = "1.00";

            return Response.AsJson(test, HttpStatusCode.OK);
        }

        public Response GetCompanyColumnChart(string companyCode)
        {
            // Set default values
            Dictionary<string, int> data = new Dictionary<string, int>();
            data.Add("Users", 0);
            data.Add("Mailboxes", 0);
            data.Add("Citrix Users", 0);
            data.Add("Lync Users", 0);
            data.Add("Distribution Groups", 0);
            data.Add("Contacts", 0);

            // Query database for actual values
            CloudPanelContext ctx = null;
            try
            {
                ctx = new CloudPanelContext(Settings.ConnectionString);

                var companyUsers = from u in ctx.Users where u.CompanyCode == companyCode select u;
                var userIds = from u in companyUsers select u.ID;

                data["Users"] = companyUsers.Count();
                data["Mailboxes"] = (from u in companyUsers where u.MailboxPlan > 0 select u).Count();
                data["Citrix Users"] = (from u in ctx.UserPlansCitrix where userIds.Contains(u.UserID) select u.UserID).Distinct().Count();
                data["Lync Users"] = 0;
                data["Distribution Groups"] = (from d in ctx.DistributionGroups where d.CompanyCode == companyCode select d).Count();
                data["Contacts"] = (from c in ctx.Contacts where c.CompanyCode == companyCode select c).Count();
            }
            catch (Exception ex)
            {
                log.Error("Error retrieving company column chart for " + companyCode, ex);
            }
            finally
            {
                ctx.Dispose();
            }

            return Response.AsJson(data, HttpStatusCode.OK);
        }

        public Response GetUsers(string companyCode)
        {
            // Query database for actual values
            CloudPanelContext ctx = null;
            try
            {
                ctx = new CloudPanelContext(Settings.ConnectionString);

                var users = from u in ctx.Users
                            where u.CompanyCode == companyCode
                            select u;

                var search = Request.Query.sSearch.HasValue ? ((string)Request.Query.sSearch).ToLower() : "";

                var filteredUsers = new List<User>();
                filteredUsers = users.ToList();

                // This is if we are searching..
                if (!string.IsNullOrEmpty(search))
                    filteredUsers = filteredUsers.Where(c => (
                                        c.DisplayName.ToLower().Contains(search) ||
                                        c.UserPrincipalName.ToLower().Contains(search) ||
                                        c.sAMAccountName.ToLower().Contains(search) ||
                                        c.Department.ToLower().Contains(search)
                                    )).ToList();

                int start = Convert.ToInt32(Request.Query.iDisplayStart.ToString());
                int length = Convert.ToInt32(Request.Query.iDisplayLength.ToString());
                var totalRecords = filteredUsers.Count();
                var secho = Request.Query.sEcho;
                var sorting = Request.Query.sSortDir_0;

                if (sorting == "asc")
                {
                    return Response.AsJson(new { aaData = filteredUsers.OrderBy(x => x.DisplayName).Skip(start).Take(length), sEcho = secho, iTotalRecords = totalRecords, iTotalDisplayRecords = totalRecords });
                }
                else
                {
                    return Response.AsJson(new { aaData = filteredUsers.OrderByDescending(x => x.DisplayName).Skip(start).Take(length), sEcho = secho.ToString(), iTotalRecords = totalRecords, iTotalDisplayRecords = totalRecords });
                }
            }
            catch (Exception ex)
            {
                log.Error("Error retrieving users for " + companyCode, ex);
                return Nancy.Response.NoBody;
            }
            finally
            {
                if (ctx != null)
                    ctx.Dispose();
            }
        }
    }
}