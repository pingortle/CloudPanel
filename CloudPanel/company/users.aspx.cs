using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.company
{
    public partial class users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }
        #region Button Click Events

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            panelUserList.Visible = false;
            panelEditCreateUser.Visible = true;

            // Clear the hidden field since we are adding a new user
            hfEditUserPrincipalName.Value = string.Empty;

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // GET LIST OF USERS
            panelUserList.Visible = true;
            panelEditCreateUser.Visible = false;
        }

        #endregion

    }
}