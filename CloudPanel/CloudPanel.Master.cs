using CloudPanel.Modules.Base.Users;
using CloudPanel.Modules.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.Template
{
    public partial class CloudPanel : System.Web.UI.MasterPage
    {
        protected int warningTimeoutInMilliseconds = 600000000;
        protected int expiredTimeoutInMilliseconds = 1200000000;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.Url.AbsoluteUri.Contains("/settings.aspx") && Request.IsLocal)
            {
                // Do not redirect local requests access settings page
            }
            else
            {
                // Check if user is authenticated
                if (!WebSessionHandler.IsLoggedIn)
                    Response.Redirect("~/login.aspx", true);

                SetSessionTimeout();

                if (!IsPostBack)
                {
                    imgLoggedInUser.ImageUrl = string.Format("~/company/services/UserPhotoHandler.ashx?id={0}", WebSessionHandler.Username);
                }
            }
        }

        private void SetSessionTimeout()
        {
            int expiredDefault = HttpContext.Current.Session.Timeout * 60 * 1000;
            int warningDefault = expiredDefault - 2;

            expiredTimeoutInMilliseconds = int.Parse(expiredDefault.ToString(), CultureInfo.InvariantCulture);
            warningTimeoutInMilliseconds = int.Parse(warningDefault.ToString(), CultureInfo.InvariantCulture);
        }

        #region Button Click Events

        /// <summary>
        /// Locks the screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void lnkLockScreen_Click(object sender, EventArgs e)
        {
            string redirectUrl = string.Format("~/security/locked.aspx?username={0}&displayname={1}", WebSessionHandler.Username, WebSessionHandler.DisplayName);
            Response.Redirect(redirectUrl);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchViewModel viewModel = new SearchViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            UsersObject[] foundResults = viewModel.Search(txtSearch.Text);
            if (foundResults != null)
            {
                UsersObject found = foundResults[0];
                WebSessionHandler.SelectedResellerCode = found.ResellerCode;
                WebSessionHandler.SelectedCompanyCode = found.CompanyCode;
                WebSessionHandler.SelectedCompanyName = found.CompanyName;

                Session["CPSearchResultUser"] = found.UserPrincipalName;

                Response.Redirect("~/company/users.aspx", false);
            }
            else
                txtSearch.Text = "NO RESULTS FOUND";
        }

        void viewModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            throw new Exception(message);
        }

        #endregion 
    }
}