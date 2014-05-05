using CloudPanel.Modules.Base.Plans;
using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules
{
    public class PlansModule : NancyModule
    {
        public PlansModule()  : base("Plans")
        {
            this.RequiresAuthentication();

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
                List<CompanyPlanObject> plans = new List<CompanyPlanObject>();
                plans.Add(new CompanyPlanObject()
                    {
                        CompanyPlanID = 5,
                        CompanyPlanName = "Test 1"
                    });

                ViewBag.Plans = plans;

                return View["Plans/CompanyPlans.cshtml"];
            };
        }
    }
}