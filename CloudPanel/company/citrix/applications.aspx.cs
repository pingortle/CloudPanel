using CloudPanel.Modules.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.company.citrix
{
    public partial class applications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                GetCitrixApps();
        }

        /// <summary>
        /// Gets the list of Citrix applications that the company has access to
        /// </summary>
        private void GetCitrixApps()
        {
            DB database = null;

            try
            {
                database = new DB();

                var apps = from a in database.Plans_Citrix
                           orderby a.Name
                           select a;

                if (apps != null)
                {
                    var listApps = apps.ToList();

                    foreach (var a in listApps)
                    {
                        if (string.IsNullOrEmpty(a.PictureURL))
                        {
                            if (a.IsServer)
                                a.PictureURL = "~/images/citrix/citrix_server.png";
                            else
                                a.PictureURL = "~/images/citrix/citrix_application.png";
                        }
                    }

                    // Bind to repeater
                    serversRepeater.DataSource = from s in listApps
                                                 where s.IsServer
                                                 select s;
                    serversRepeater.DataBind();

                    repeaterApps.DataSource = from a in listApps
                                              where !a.IsServer
                                              select a;
                    repeaterApps.DataBind();
                }
            }
            catch (Exception ex)
            {
                alertmessage.SetMessage(Modules.Base.Enumerations.AlertType.ERROR, ex.ToString());
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }
    }
}