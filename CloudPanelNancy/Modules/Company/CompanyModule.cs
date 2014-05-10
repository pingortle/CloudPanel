using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;
using Nancy.Authentication.Forms;

namespace CloudPanelNancy.Modules
{
    public class CompanyModule : NancyModule
    {
        public CompanyModule() : base("Company")
        {
            this.RequiresAuthentication();
            this.RequiresAnyClaim(new[] { "SuperAdmin", "ResellerAdmin", "CompanyAdmin" });

            Get["{CompanyCode}"] = parameters =>
                {
                    var user = this.Context.CurrentUser as AuthenticatedUser;
                    user.SelectedCompanyCode = parameters.CompanyCode;

                    return View["Company/Overview"];
                };
        }
    }
}