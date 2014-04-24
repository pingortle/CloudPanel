using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Common.ViewModel;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules
{
    public class ResellerModule : NancyModule
    {
        ResellerViewModel viewModel = new ResellerViewModel();

        public ResellerModule() : base("Resellers")
        {
            Get["/"] = _ =>
                {
                    List<ResellerObject> resellers = viewModel.GetResellers();

                    return View["/Resellers/List", resellers];
                };

            Get["/{CompanyCode}/Edit"] = parameters =>
                {
                    ResellerObject reseller = viewModel.GetReseller(parameters.CompanyCode);

                    return View["/Edit", reseller];
                };

            Post["/{CompanyCode}/Edit"] = parameters =>
                {
                    var companyName = this.Request.Form.txtCompanyName;

                    ViewBag.EditCompanyName = companyName;
                    return View["Edit"];
                };
        }


    }
}