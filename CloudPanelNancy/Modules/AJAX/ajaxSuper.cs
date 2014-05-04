using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Dashboard;
using Nancy;
using Nancy.Responses.Negotiation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules.AJAX
{
    public class ajaxSuper : NancyModule
    {
        public ajaxSuper() : base("/AJAX")
        {
            //this.RequiresAuthentication();
            //this.RequiresAnyClaim(new[] { "SuperAdmin" });

            Get["/Resellers/GetAll"] = parameters => GetResellers();
            Get["/Dashboard/Charting/Column"] = parameters => GetTop5Customers();
        }

        public Response GetResellers()
        {
            List<ResellerObject> resellers = new List<ResellerObject>();
            for (int i = 0; i < 100000; i++)
            {
                resellers.Add(new ResellerObject()
                {
                    CompanyID = i,
                    CompanyCode = "Reseller" + i.ToString(),
                    CompanyName = "Reseller" + i.ToString(),
                    Street = "300 Simpson Rd",
                    City = "Vilonia",
                    State = "AR",
                    ZipCode = "72173",
                    Created = DateTime.Now
                });
            }

            int start = Convert.ToInt32(Request.Query.iDisplayStart.ToString());
            int length = Convert.ToInt32(Request.Query.iDisplayLength.ToString());
            var totalRecords = resellers.Count();
            var secho = Request.Query.sEcho;
            var sorting = Request.Query.sSortDir_0;

            if (sorting == "asc")
            {
                return Response.AsJson(new { aaData = resellers.OrderBy(x => x.CompanyName).Skip(start).Take(length), sEcho = secho, iTotalRecords = totalRecords, iTotalDisplayRecords = totalRecords });
            }
            else
            {
                return Response.AsJson(new { aaData = resellers.OrderByDescending(x => x.CompanyName).Skip(start).Take(length), sEcho = secho.ToString(), iTotalRecords = totalRecords, iTotalDisplayRecords = totalRecords });
            }
        }

        public Response GetTop5Customers()
        {
            List<Top5CustomersStats> data = new List<Top5CustomersStats>();
            data.Add(new Top5CustomersStats()
            {
                CustomerName = "Customer A",
                Users = 1000,
                Mailboxes = 900,
                MailboxUsedInGB = 928,
                MailboxAllocatedInGB = 2004
            });

            data.Add(new Top5CustomersStats()
            {
                CustomerName = "Customer B",
                Users = 2000,
                Mailboxes = 1900,
                MailboxUsedInGB = 1928,
                MailboxAllocatedInGB = 2304
            });

            data.Add(new Top5CustomersStats()
            {
                CustomerName = "Customer C",
                Users = 3000,
                Mailboxes = 2900,
                MailboxUsedInGB = 2928,
                MailboxAllocatedInGB = 3304
            });

            data.Add(new Top5CustomersStats()
            {
                CustomerName = "Customer D",
                Users = 1000,
                Mailboxes = 900,
                MailboxUsedInGB = 928,
                MailboxAllocatedInGB = 2904
            });

            return Response.AsJson(data, HttpStatusCode.OK);
        }
    }
}