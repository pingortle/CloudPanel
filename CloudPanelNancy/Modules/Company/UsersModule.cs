using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules
{
    public class UsersModule : NancyModule
    {
        public UsersModule() : base("Company")
        {
            Get["{CompanyCode}/Users"] = parameters =>
            {
                return View["Company/Users/UsersList.cshtml"];
            };
        }
    }
}