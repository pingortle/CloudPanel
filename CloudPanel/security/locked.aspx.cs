using CloudPanel.Modules.Base.Users;
using CloudPanel.Modules.Common.ViewModel;
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
                GetLockInfo();

                DestroySessions();
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
            {
                lbLoginName.Text = Request.QueryString["Username"];
            }
            else
                Response.Redirect("~/login.aspx", true);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            LoginViewModel login = new LoginViewModel();
            login.ViewModelEvent += login_ViewModelEvent;

            string ip = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
                ip = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            else
                ip = ip.Split(',')[0];

            UsersObject user = login.Authenticate(lbLoginName.Text, txtPassword.Text, ip, Request.IsLocal);
            if (user != null)
            {
                // User is authenticated
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, lbLoginName.Text, DateTime.Now, DateTime.Now.AddHours(8), true, "");

                string cookieEncrypt = FormsAuthentication.Encrypt(ticket);

                HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieEncrypt);
                cookie.Path = FormsAuthentication.FormsCookiePath;
                Response.Cookies.Add(cookie);

                if (user.IsSuperAdmin)
                    WebSessionHandler.IsSuperAdmin = true;

                if (user.IsResellerAdmin)
                {
                    WebSessionHandler.IsResellerAdmin = true;
                    WebSessionHandler.SelectedResellerCode = user.ResellerCode;
                }

                if (user.IsCompanyAdmin)
                {
                    WebSessionHandler.IsCompanyAdmin = true;
                    WebSessionHandler.SelectedResellerCode = user.ResellerCode;
                    WebSessionHandler.SelectedCompanyCode = user.CompanyCode;
                }

                if (!string.IsNullOrEmpty(user.DisplayName))
                    WebSessionHandler.DisplayName = user.DisplayName;
                else
                    WebSessionHandler.DisplayName = lbLoginName.Text;

                // Redirect to dashbaord
                Response.Redirect("~/dashboard.aspx", false);
            }
            else
                lbError.Text = "Login failed";
        }

        void login_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            lbError.Text = message;
        }
    }
}