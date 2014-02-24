using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            }
        }

        private void GetLockInfo()
        {
            string displayName = Request.QueryString["DisplayName"];
            string loginName = Request.QueryString["LoginName"];

            lbDisplayName.Text = displayName;
            lbLoginName.Text = loginName;
        }
    }
}