using Nancy;
using Nancy.Authentication.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules
{
    public class LoginModule : NancyModule
    {
        public LoginModule()
        {
            Get["/login"] = _ => View["Login"];

            Post["/login"] = Login;
        }

        public dynamic Login(dynamic parameters)
        {
            var username = this.Request.Form.username;
            var password = this.Request.Form.password;

            if (username == "admin" && password == "admin")
            {
                var token = Guid.NewGuid();

                return this.Response.AsRedirect("~/Resellers");
            }
            else
            {
                ViewBag.LoginError = "Invalid username and/or password";
                return View["Login"];
            }
        }
    }
}