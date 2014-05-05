using CloudPanel.Modules.Base.Companies;
using Nancy;
using Nancy.Security;
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules
{
    public class ResellerModule : NancyModule
    {
        public ResellerModule()
        {
            this.RequiresAuthentication();

            Get["/Resellers"] = _ =>
                {
                    List<ResellerObject> resellers = null; // Get all resellers

                    return View["Admin/ResellerList.cshtml", resellers];
                };

            Get["/Resellers/Add"] = parameters =>
            {
                ResellerObject newReseller = new ResellerObject();

                return View["Admin/ResellerEdit.cshtml", newReseller];
            };

            Get["/Resellers/{ResellerCode}/Edit"] = parameters =>
                {
                    ResellerObject reseller = new ResellerObject();
                    reseller.CompanyID = 0;
                    reseller.CompanyCode = parameters.ResellerCode;
                    reseller.CompanyName = "Reseller";
                    reseller.Street = "300 Simpson Rd";
                    reseller.City = "Vilonia";
                    reseller.State = "AR";
                    reseller.ZipCode = "72173";
                    reseller.Created = DateTime.Now;
                    reseller.AdminName = "";
                    reseller.AdminEmail = "";
                    reseller.Telephone = "";

                    return View["Admin/ResellerEdit.cshtml", reseller];
                };

            Get["/Resellers/{CompanyCode}/Companies"] = parameters =>
            {
                if (this.Context.CurrentUser != null)
                {
                    var user = this.Context.CurrentUser as AuthenticatedUser;
                    user.SelectedResellerCode = parameters.CompanyCode;
                }

                return View["Admin/CompanyList", parameters.CompanyCode];
            };

            Post["/Resellers/Edit"] = parameters =>
            {
                ViewBag.Testing = "NEW RESELLER";

                var test = this.Bind<ResellerObject>(); // Gather data from form

                                

                return View["Admin/ResellerEdit", test];
            };

            Post["/Resellers/Edit/{ResellerCode}"] = parameters =>
                {
                    ViewBag.Testing = "EDIT RESELLER";

                    var test = this.Bind<ResellerObject>();  // Gather data from form

                    return View["Admin/ResellerEdit", test];
                };
        }
    }
}