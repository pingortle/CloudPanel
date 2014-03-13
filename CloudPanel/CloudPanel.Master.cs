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

        }

        #endregion 
    }
}