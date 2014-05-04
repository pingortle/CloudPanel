using CloudPanel.Modules.Base.Companies;
using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules.AJAX
{
    public class ajaxReseller : NancyModule
    {
        public ajaxReseller() : base("/AJAX")
        {
            //this.RequiresAuthentication();
            //this.RequiresAnyClaim(new[] { "SuperAdmin", "ResellerAdmin" });

            Get["/Resellers/{ResellerCode}/Companies/GetAll"] = parameters => GetCompanies(parameters.ResellerCode);
        }

        public Response GetCompanies(string resellerCode)
        {
            //CompanyViewModel viewModel = new CompanyViewModel();
                   // List<CompanyObject> companies = viewModel.GetCompanies(parameters.ResellerCode);
            List<CompanyObject> companies = new List<CompanyObject>();
            for (int i = 0; i < 1000; i++)
            {
                companies.Add(new CompanyObject()
                {
                    CompanyID = i,
                    CompanyCode = "Company" + i.ToString(),
                    CompanyName = "Company" + i.ToString(),
                    Street = "300 Simpson Rd",
                    City = "Vilonia",
                    State = "AR",
                    ZipCode = "72173",
                    Created = DateTime.Now
                });
            }

            if (companies != null)
            {
                        int start = Convert.ToInt32(Request.Query.iDisplayStart.ToString());
                        int length = Convert.ToInt32(Request.Query.iDisplayLength.ToString());
                        var totalRecords = companies.Count();
                        var secho = Request.Query.sEcho;
                        var sorting = Request.Query.sSortDir_0;

                        if (sorting == "asc")
                        {
                            return Response.AsJson(new { aaData = companies.OrderBy(x => x.CompanyName).Skip(start).Take(length), sEcho = secho, iTotalRecords = totalRecords, iTotalDisplayRecords = totalRecords });
                        }
                        else
                        {
                            return Response.AsJson(new { aaData = companies.OrderByDescending(x => x.CompanyName).Skip(start).Take(length), sEcho = secho.ToString(), iTotalRecords = totalRecords, iTotalDisplayRecords = totalRecords });
                        }
              }
              else
                return Response.AsJson(new { aaData = new List<CompanyObject>(), iTotalRecords = 0, iTotalDisplayRecords = 0 }, HttpStatusCode.NoContent);
        }
    }
}