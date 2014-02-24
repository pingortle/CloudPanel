using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Database.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel
{
    public partial class resellers : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get a list of resellers
                GetResellers();
            }
        }

        /// <summary>
        /// Gets a list of resellers from the database
        /// </summary>
        private void GetResellers()
        {
            List<ResellerObject> resellers = Resellers.GetResellers();
            resellersRepeater.DataSource = resellers;
            resellersRepeater.DataBind();

            // Show panel
            panelResellerList.Visible = true;
            panelEditCreateReseller.Visible = false;
        }

        /// <summary>
        /// Gets a reseller that was selected for editing
        /// </summary>
        /// <param name="resellerCode"></param>
        private void GetReseller(string resellerCode)
        {
            ResellerObject reseller = Resellers.GetReseller(resellerCode);
            hfResellerCode.Value = reseller.CompanyCode;
            txtName.Text = reseller.CompanyName;
            txtContactsName.Text = reseller.AdminName;
            txtEmail.Text = reseller.AdminEmail;
            txtTelephone.Text = reseller.Telephone;
            txtStreetAddress.Text = reseller.Street;
            txtCity.Text = reseller.City;
            txtState.Text = reseller.State;
            txtZipCode.Text = reseller.ZipCode;

            if (!string.IsNullOrEmpty(reseller.Country))
            {
                ListItem item = ddlCountry.Items.FindByValue(reseller.Country);
                if (item != null)
                    ddlCountry.SelectedIndex = ddlCountry.Items.IndexOf(item);
            }

            // Show panel
            panelEditCreateReseller.Visible = true;
            panelResellerList.Visible = false;
        }

        protected void resellersRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
                GetReseller(e.CommandArgument.ToString());
        }

        #region Button Clicks

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            GetResellers();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

        }

        protected void btnAddReseller_Click(object sender, EventArgs e)
        {
            // Set the hidden field to null because we are creating a new reseller
            hfResellerCode.Value = string.Empty;

            panelEditCreateReseller.Visible = true;
            panelResellerList.Visible = false;

            // Clear values
            foreach (Control ctrl in panelEditCreateReseller.Controls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
                else if (ctrl is DropDownList)
                    ddlCountry.SelectedIndex = -1;
            }
        }

        #endregion

    }
}