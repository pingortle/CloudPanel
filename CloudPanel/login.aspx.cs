using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CloudPanel.Modules.Common.ViewModel;
using System.Web.Security;
using CloudPanel.Modules.Base.Users;

namespace CloudPanel
{
    public partial class login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
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

            UsersObject user = login.Authenticate(txtUsername.Text, txtPassword.Text, ip, Request.IsLocal);
            if (user != null)
            {
                // User is authenticated
                FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, txtUsername.Text, DateTime.Now, DateTime.Now.AddHours(8), true, "");

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
                    WebSessionHandler.DisplayName = txtUsername.Text;

                // Redirect to dashbaord
                Response.Redirect("~/dashboard.aspx", false);
            }
        }

        void login_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            switch (errorID)
            {
                case Modules.Base.Enumerations.AlertID.LOGIN_FAILED:
                    lbLoginMessage.Text = Resources.LocalizedText.Login_LoginFailed;
                    break;
                case Modules.Base.Enumerations.AlertID.USER_UNKNOWN:
                    lbLoginMessage.Text = Resources.LocalizedText.Login_LoginFailed;
                    break;
                case Modules.Base.Enumerations.AlertID.RETRIEVE_GROUPS_FAILED:
                    lbLoginMessage.Text = Resources.LocalizedText.Login_GroupsFailed;
                    break;
                case Modules.Base.Enumerations.AlertID.BRUTE_FORCE_BLOCKED:
                    lbLoginMessage.Text = Resources.LocalizedText.Login_BruteForceBlocked;
                    break;
                default:
                    lbLoginMessage.Text = message;
                    break;
            }
        }
    }
}