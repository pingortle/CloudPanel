using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.company
{
    public partial class domains : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        #region Button Click Events

        protected void btnAddDomain_Click(object sender, EventArgs e)
        {
            panelDomainList.Visible = false;
            panelEditCreateDomain.Visible = true;

            // Clear hidden field since we are creating a new domain
            hfDomainID.Value = string.Empty;

            // Clear domain field and checkbox field
            txtDomainName.Text = string.Empty;
            cbIsDefaultDomain.Checked = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // Cancel and go back to main screen
            panelDomainList.Visible = true;
            panelEditCreateDomain.Visible = false;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            // Save new or edit existing domain
        }

        #endregion
    }
}