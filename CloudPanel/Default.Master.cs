using CloudPanel.Modules.Base.Users;
using CloudPanel.Modules.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel
{
    public partial class Default : System.Web.UI.MasterPage
    {
        protected int warningTimeoutInMilliseconds = 600000000;
        protected int expiredTimeoutInMilliseconds = 1200000000;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Request.Url.AbsoluteUri.Contains("/settings.aspx") || !Request.IsLocal)
            {
                // Check if user is authenticated
#if !DEBUG
                if (!WebSessionHandler.IsLoggedIn)
                    Response.Redirect("~/login.aspx", true);

                SetSessionTimeout();

                if (!IsPostBack)
                {
                    imgLoggedInUser.ImageUrl = string.Format("~/company/services/UserPhotoHandler.ashx?id={0}", WebSessionHandler.Username);
                }
#endif
            }

            ProcessPlaceHolders();
        }

        private void SetSessionTimeout()
        {
            expiredTimeoutInMilliseconds = HttpContext.Current.Session.Timeout * 60 * 1000;
            warningTimeoutInMilliseconds = (HttpContext.Current.Session.Timeout - 2) * 60 * 1000;
        }

        private void ProcessPlaceHolders()
        {
            bool isSuperAdmin = WebSessionHandler.IsSuperAdmin;
            bool isResellerAdmin = WebSessionHandler.IsResellerAdmin;
            string selectedCompanyCode = WebSessionHandler.SelectedCompanyCode;

            PlaceHolderResellersMenu.Visible = isSuperAdmin;
            PlaceHolderSelectedCompanyMenu.Visible = !string.IsNullOrEmpty(selectedCompanyCode);
            PlaceHolderCompaniesMenu.Visible = (isSuperAdmin || isResellerAdmin) && !string.IsNullOrEmpty(selectedCompanyCode);

            if (!string.IsNullOrEmpty(selectedCompanyCode))
            {
                bool isExchangeEnabled = CloudPanel.Modules.Common.GlobalActions.CompanyChecks.IsExchangeEnabled(selectedCompanyCode);

                PlaceHolderExchangeDisabled.Visible = !isExchangeEnabled;
                PlaceHolderExchangeEnabled.Visible = isExchangeEnabled;
            }
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

        #endregion 
    }
}
