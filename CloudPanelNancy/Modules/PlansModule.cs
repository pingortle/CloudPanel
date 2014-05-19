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
using Nancy;
using Nancy.Security;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CloudPanel.Modules.Persistence.EntityFramework.Models;

namespace CloudPanelNancy.Modules
{
    public class PlansModule : NancyModule
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(PlansModule));

        public PlansModule()  : base("Plans")
        {
            //this.RequiresAuthentication();
            //this.RequiresAnyClaim(new[] { "SuperAdmin" });

            Get["/Mailbox"] = parameters =>
            {
                List<MailboxPlanObject> plans = new List<MailboxPlanObject>();
                plans.Add(new MailboxPlanObject()
                {
                     MailboxPlanID = 1,
                     MailboxPlanName = "Bronze"
                });

                ViewBag.Plans = plans;
                return View["Plans/MailboxPlans.cshtml"];
            };
            
            Get["/Company"] = parameters =>
            {
                CloudPanelContext ctx = null;
                try
                {
                    ctx = new CloudPanelContext(Settings.ConnectionString);

                    var plans = (from p in ctx.Plans_Organization
                                 select p).ToList();

                    return View["Plans/CompanyPlans.cshtml", plans];
                }
                catch (Exception ex)
                {
                    log.Error("Error retrieving company plans", ex);
                    return View["Plans/CompanyPlans.cshtml", null];
                }
                finally
                {
                    ctx.Dispose();
                }
            };

            Post["/Company/Update"] = parameters =>
            {
                var formModel = this.Bind<Plans_Organization>();

                CloudPanelContext ctx = null;
                try
                {
                    ctx = new CloudPanelContext(Settings.ConnectionString);

                    if (formModel.OrgPlanID <= 0)
                    {
                        ctx.Plans_Organization.Add(formModel);
                        ctx.SaveChanges();
                    }
                    else
                    {
                        var plan = (from p in ctx.Plans_Organization
                                    where p.OrgPlanID == formModel.OrgPlanID
                                    select p).FirstOrDefault();

                        plan.OrgPlanName = formModel.OrgPlanName;
                        plan.MaxUsers = formModel.MaxUsers;
                        plan.MaxDomains = formModel.MaxDomains;
                        plan.MaxExchangeMailboxes = formModel.MaxExchangeMailboxes;
                        plan.MaxExchangeContacts = formModel.MaxExchangeContacts;
                        plan.MaxExchangeDistLists = formModel.MaxExchangeDistLists;
                        plan.MaxExchangeResourceMailboxes = formModel.MaxExchangeResourceMailboxes;
                        plan.MaxExchangeMailPublicFolders = formModel.MaxExchangeMailPublicFolders;

                        ctx.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    log.Error("Error retrieving company plans", ex);
                }
                finally
                {
                    ctx.Dispose();
                }

                return Response.AsRedirect("~/Plans/Company");
            };
        }
    }
}