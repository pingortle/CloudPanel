using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Common.Database;
using CloudPanel.Modules.Common.GlobalActions;
using CloudPanel.Modules.Common.Settings;
using CloudPanel.Modules.Common.ViewModel;
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
        ResellerViewModel resellerModel = new ResellerViewModel();

        protected void Page_Load(object sender, EventArgs e)
        {
            resellerModel.ViewModelEvent += resellerModel_ViewModelEvent;

            if (!IsPostBack)
            {
                // Get a list of resellers
                PopulateResellers();
            }
        }

        /// <summary>
        /// Gets a list of resellers from the database
        /// </summary>
        private void PopulateResellers()
        {
            List<ResellerObject> foundResellers = resellerModel.GetResellers();
            if (foundResellers != null)
            {
                resellersRepeater.DataSource = foundResellers;
                resellersRepeater.DataBind();
            }

            // Show the panel no matter what
            panelEditCreateReseller.Visible = false;
            panelResellerList.Visible = true;
        }

        /// <summary>
        /// Populates a specific reseller
        /// </summary>
        /// <param name="companyCode"></param>
        private void PopulateReseller(string companyCode)
        {
            ResellerObject reseller = resellerModel.GetReseller(companyCode);

            hfResellerCode.Value = companyCode;
            txtName.Text = reseller.CompanyName;
            txtContactsName.Text = reseller.AdminName;
            txtEmail.Text = reseller.AdminEmail;
            txtTelephone.Text = reseller.Telephone;
            txtStreetAddress.Text = reseller.Street;
            txtCity.Text = reseller.City;
            txtState.Text = reseller.State;
            txtZipCode.Text = reseller.ZipCode;

            ListItem item = ddlCountry.Items.FindByValue(reseller.Country);
            if (item != null)
            {
                ddlCountry.SelectedValue = item.Value;
            }

            // Show the edit panel and readonly the textbox
            panelEditCreateReseller.Visible = true;
            panelResellerList.Visible = false;
        }

        /// <summary>
        /// Adds a new reseller to the environment
        /// </summary>
        private void AddNewReseller()
        {
            ResellerObject newReseller = new ResellerObject();
            newReseller.CompanyName = txtName.Text;
            newReseller.AdminName = txtContactsName.Text;
            newReseller.AdminEmail = txtEmail.Text;
            newReseller.Telephone = txtTelephone.Text;
            newReseller.Street = txtStreetAddress.Text;
            newReseller.City = txtCity.Text;
            newReseller.State = txtState.Text;
            newReseller.ZipCode = txtZipCode.Text;

            if (ddlCountry.SelectedIndex > 0)
                newReseller.Country = ddlCountry.SelectedValue;
            else
                newReseller.Country = string.Empty;

            // Validate
            if (newReseller.CompanyName.Length < 3)
                alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.WARNING, "The company name must be three characters or more");
            else
            {
                //
                // Create new reseller
                //
                resellerModel.NewReseller(newReseller, StaticSettings.HostingOU);
            }

            PopulateResellers();
        }

        /// <summary>
        /// Updates an existing resellers information
        /// </summary>
        /// <param name="stringId"></param>
        private void UpdateReseller(string companyCode)
        {
            ResellerObject reseller = new ResellerObject();
            reseller.CompanyCode = companyCode;
            reseller.CompanyName = txtName.Text;
            reseller.AdminName = txtContactsName.Text;
            reseller.AdminEmail = txtEmail.Text;
            reseller.Telephone = txtTelephone.Text;
            reseller.Street = txtStreetAddress.Text;
            reseller.City = txtCity.Text;
            reseller.State = txtState.Text;
            reseller.ZipCode = txtZipCode.Text;

            if (ddlCountry.SelectedIndex > 0)
                reseller.Country = ddlCountry.SelectedValue;
            else
                reseller.Country = string.Empty;

            // Validate
            if (reseller.CompanyName.Length < 3)
                alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.WARNING, "The company name must be three characters or more");
            else
            {
                //
                // Update reseller
                //
                resellerModel.UpdateReseller(reseller);
            }

            PopulateResellers();
        }

        protected void resellersRepeater_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                PopulateReseller(e.CommandArgument.ToString());
            }
            else if (e.CommandName == "Select")
            {
                WebSessionHandler.SelectedResellerCode = e.CommandArgument.ToString();
                Server.Transfer("companies.aspx");
            }
        }

        #region Events

        void resellerModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            alertmessage.SetMessage(errorID, message);

            // Repopulate resellers
            PopulateResellers();
        }

        #endregion

        #region Button Clicks

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PopulateResellers();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfResellerCode.Value))
                AddNewReseller();
            else
                UpdateReseller(hfResellerCode.Value);
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