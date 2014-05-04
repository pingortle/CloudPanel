using CloudPanel.Modules.Base.Plans;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules.AJAX
{
    public class ajaxAll : NancyModule
    {
        public ajaxAll() : base("/AJAX")
        {
            //this.RequiresAuthentication();

            Get["/Plans/Company/Get/{ID}"] = parameters => GetCompanyPlan(parameters.ID);
            Get["/Plans/Mailbox/Get/{ID}"] = parameters => GetMailboxPlan(parameters.ID);
            Get["/Company/Charting/Pie/{CompanyCode}"] = parameters => GetCompanyPieChart(parameters.CompanyCode);
        }

        public Response GetCompanyPlan(string id)
        {
            CompanyPlanObject test = new CompanyPlanObject();
            test.CompanyPlanName = "BLAH";
            test.MaxUser = 30;
            test.MaxDomains = 5;
            test.MaxExchangeMailboxes = 30;
            test.MaxExchangeContacts = 5;
            test.MaxExchangeDistributionGroups = 5;
            test.MaxExchangeResourceMailboxes = 5;
            test.MaxExchangeMailPublicFolders = 5;

            return Response.AsJson(test, HttpStatusCode.OK);
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

        public Response GetCompanyPieChart(string companyCode)
        {
            Dictionary<string, int> data = new Dictionary<string, int>();
            data.Add("Users", 1000);
            data.Add("Mailboxes", 500);
            data.Add("Citrix Users", 100);
            data.Add("Lync Users", 300);
            data.Add("Distribution Groups", 25);
            data.Add("Contacts", 10);

            return Response.AsJson(data, HttpStatusCode.OK);
        }
    }
}