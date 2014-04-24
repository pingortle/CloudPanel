using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules
{
    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/", ctx => ctx.CurrentUser == null] = _ => Response.AsRedirect("~/Login");
        }
    }
}