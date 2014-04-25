using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Common.ViewModel;
using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules.API
{
    public class ApiModule : NancyModule
    {
        public ApiModule() : base("/api")
        {
            this.RequiresAuthentication();
            this.RequiresClaims(new[] { "Super" });

            Get["/Resellers/GetAll"] = parameters =>
                {
                    ResellerViewModel viewModel = new ResellerViewModel();
                    List<ResellerObject> resellers = viewModel.GetResellers();

                    if (resellers != null)
                    {
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
                    else
                        return Response.AsJson(new { aaData = new List<ResellerObject>(), iTotalRecords = 0, iTotalDisplayRecords = 0 }, HttpStatusCode.NoContent);
                };


            Get["/Resellers/{ResellerCode}/Companies/GetAll"] = parameters =>
                {
                    CompanyViewModel viewModel = new CompanyViewModel();
                    List<CompanyObject> companies = viewModel.GetCompanies(parameters.ResellerCode);

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
                };
        }
    }
}