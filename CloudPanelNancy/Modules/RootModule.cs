using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;
using Nancy.Authentication.Forms;

namespace CloudPanelNancy.Modules
{
    public class RootModule : NancyModule
    {
        public RootModule()
        {
            Get["/", ctx => ctx.CurrentUser == null] = _ => Response.AsRedirect("Login");
            Get["/login"] = _ => View["Login"];

            Post["/login"] = parameters =>
            {
                var username = this.Request.Form.username;
                var password = this.Request.Form.password;

                Guid? userGuid = UserMapper.ValidateUser(username, password);
                if (userGuid == null)
                {
                    ViewBag.LoginError = "Invalid username and/or password";
                    return View["Login"];
                }
                else
                {
                    return this.LoginAndRedirect(userGuid.Value, null, "Dashboard");
                }
            };
        }
    }
}