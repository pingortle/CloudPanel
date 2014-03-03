using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Common.GlobalActions;
using CloudPanel.Modules.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel
{
    public partial class companies : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                PopulateCompanies();
            }
        }

        /// <summary>
        /// Gets a list of companies from the database
        /// </summary>
        private void PopulateCompanies()
        {
            CompanyViewModel companyModel = new CompanyViewModel();
            companyModel.ViewModelEvent += companyModel_ViewModelEvent;

            List<CompanyObject> foundCompanies = companyModel.GetCompanies(WebSessionHandler.SelectedResellerCode);
            if (foundCompanies != null)
            {
                repeaterCompanies.DataSource = foundCompanies;
                repeaterCompanies.DataBind();
            }

            // Show the panel no matter what
            panelEditCreateCompany.Visible = false;
            panelCompanyList.Visible = true;
        }

        /// <summary>
        /// Populates an existing company
        /// </summary>
        /// <param name="companyCode"></param>
        private void PopulateCompany(string companyCode)
        {
            CompanyViewModel companyModel = new CompanyViewModel();
            companyModel.ViewModelEvent += companyModel_ViewModelEvent;

            CompanyObject company = companyModel.GetCompany(companyCode);

            hfCompanyCode.Value = companyCode;
            txtName.Text = company.CompanyCode;
            txtContactsName.Text = company.AdminName;
            txtEmail.Text = company.AdminEmail;
            txtTelephone.Text = company.Telephone;
            txtStreetAddress.Text = company.Street;
            txtCity.Text = company.City;
            txtState.Text = company.State;
            txtZipCode.Text = company.ZipCode;

            ListItem item = ddlCountry.Items.FindByValue(company.Country);
            if (item != null)
            {
                ddlCountry.SelectedValue = item.Value;
            }

            // Show the edit panel and readonly the textbox
            panelEditCreateCompany.Visible = true;
            panelCompanyList.Visible = false;

            // Disable the domain textbox
            txtDomainName.Text = string.Empty;
            txtDomainName.ReadOnly = true;
        }

        private void AddNewCompany()
        {
            CompanyObject newCompany = new CompanyObject();
            newCompany.CompanyName = txtName.Text;
            newCompany.AdminName = txtContactsName.Text;
            newCompany.AdminEmail = txtEmail.Text;
            newCompany.Telephone = txtTelephone.Text;
            newCompany.Street = txtStreetAddress.Text;
            newCompany.City = txtCity.Text;
            newCompany.State = txtState.Text;
            newCompany.ZipCode = txtZipCode.Text;
            newCompany.ResellerCode = WebSessionHandler.SelectedResellerCode;
            newCompany.Domains = new string[] { txtDomainName.Text.Trim() };
            newCompany.UseCompanyNameInsteadofCompanyCode = cbUseCompanyName.Checked;
            
            if (ddlCountry.SelectedIndex > 0)
                newCompany.Country = ddlCountry.SelectedValue;
            else
                newCompany.Country = string.Empty;

            // Validate
            if (newCompany.CompanyName.Length < 3)
                alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.WARNING, "The company name must be three characters or more");
            else
            {
                //
                // Create new company
                //
                CompanyViewModel companyModel = new CompanyViewModel();
                companyModel.ViewModelEvent += companyModel_ViewModelEvent;
                companyModel.NewCompany(newCompany, newCompany.ResellerCode);
            }
        }

        protected void repeaterCompanies_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
                PopulateCompany(e.CommandName.ToString());
        }

        #region Events

        void companyModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            if (errorID == Modules.Base.Enumerations.AlertID.SUCCESS_NEW_COMPANY)
            {
                string successMessage = string.Format("{0} {1}", Resources.LocalizedText.Audits_NewCompany, message);
                AuditGlobal.AddAudit(WebSessionHandler.SelectedCompanyCode, WebSessionHandler.Username, successMessage);

                // Set the success message
                alertmessage.SetMessage(errorID, successMessage);
            }
            else
                alertmessage.SetMessage(errorID, message);


            // Repopulate companies
            PopulateCompanies();
        }

        #endregion

        #region Button Click Events

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfCompanyCode.Value))
                AddNewCompany();
        }

        protected void btnAddCompany_Click(object sender, EventArgs e)
        {
            // Set the hidden field to null because we are creating a new company
            hfCompanyCode.Value = string.Empty;

            panelEditCreateCompany.Visible = true;
            panelCompanyList.Visible = false;

            // Clear values
            foreach (Control ctrl in panelEditCreateCompany.Controls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
                else if (ctrl is DropDownList)
                    ddlCountry.SelectedIndex = -1;
            }

            // Enable the domain textbox
            txtDomainName.ReadOnly = false;
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            PopulateCompanies();
        }

        #endregion

    }
}