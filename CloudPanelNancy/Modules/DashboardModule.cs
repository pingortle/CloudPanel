using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules
{
    public class DashboardModule : NancyModule
    {
        public DashboardModule() : base("/Dashboard")
        {
            //this.RequiresAuthentication();
            //this.RequiresAnyClaim(new[] { "SuperAdmin", "ResellerAdmin", "CompanyAdmin" })

            Get["/"] = _ =>
            {

                this.Context.Culture = new System.Globalization.CultureInfo("de-CH");
                return View["Dashboard"];
            };
        }
    }
}