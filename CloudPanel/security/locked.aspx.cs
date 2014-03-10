using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.security
{
    public partial class locked : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DestroySessions();

                GetLockInfo();
            }
        }

        private void DestroySessions()
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session.Abandon();
            }
        }

        private void GetLockInfo()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["DisplayName"]))
                lbDisplayName.Text = Request.QueryString["DisplayName"];
            else
                lbDisplayName.Text = "Unknown";

            if (!string.IsNullOrEmpty(Request.QueryString["Username"]))
                lbLoginName.Text = Request.QueryString["Username"];
            else
                Response.Redirect("~/login.aspx", true);
        }
    }
}