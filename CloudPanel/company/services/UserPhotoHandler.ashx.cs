using CloudPanel.Modules.Common.ViewModel;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;

namespace CloudPanel.company.services
{
    /// <summary>
    /// Summary description for UserPhotoHandler
    /// </summary>
    public class UserPhotoHandler : IHttpHandler, System.Web.SessionState.IReadOnlySessionState
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                if (WebSessionHandler.IsLoggedIn)
                {
                    if (context.Request.QueryString["id"] != null)
                    {
                        UsersViewModel viewModel = new UsersViewModel();

                        byte[] data = viewModel.GetPhoto(context.Request.QueryString["id"]);
                        if (data == null)
                        {
                            context.Response.ContentType = "image/png";
                            context.Response.WriteFile(context.Server.MapPath("~/images/photos/unknown-user.png"));
                        }
                        else
                        {
                            context.Response.ContentType = "image/jpg";
                            context.Response.BinaryWrite(data);
                        }
                    }
                    else
                    {
                        context.Response.ContentType = "image/png";
                        context.Response.WriteFile(context.Server.MapPath("~/images/photos/unknown-user.png"));
                    }
                }
                else
                {
                    context.Response.ContentType = "image/png";
                    context.Response.WriteFile(context.Server.MapPath("~/images/photos/unknown-user.png"));
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to retrieve default image for user", ex);
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}