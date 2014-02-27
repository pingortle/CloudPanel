using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.company.email
{
    public partial class groups : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void GetAllUsersTest()
        {
            
        }

        #region Button Click Events

        protected void btnAddGroup_Click(object sender, EventArgs e)
        {
            panelGroupList.Visible = false;
            panelEditGroup.Visible = true;

            // LAB
            GetAllUsersTest();
        }

        #endregion
    }
}