using Nancy;
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
            Get["/"] = _ => View["Dashboard"];
        }
    }
}