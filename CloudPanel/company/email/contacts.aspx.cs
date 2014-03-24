using CloudPanel.Modules.Base.Exchange;
using CloudPanel.Modules.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.company.email
{
    public partial class contacts : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                GetContacts();
        }

        private void GetContacts()
        {
            ExchangeContactViewModel viewModel = new ExchangeContactViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            List<MailContactObject> contacts = viewModel.GetContacts(WebSessionHandler.SelectedCompanyCode);
            repeaterContactList.DataSource = contacts;
            repeaterContactList.DataBind();

            panelContactList.Visible = true;
            panelEditContact.Visible = false;
        }

        private void CreateContact()
        {
            MailContactObject newContact = new MailContactObject();
            newContact.DisplayName = txtDisplayName.Text;
            newContact.Email = txtEmail.Text;
            newContact.Hidden = cbHidden.Checked;

            // Create new contact
            ExchangeContactViewModel viewModel = new ExchangeContactViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            viewModel.NewContact(WebSessionHandler.SelectedCompanyCode, newContact);

            GetContacts();
        }

        private void GetContact(string distinguishedName)
        {
            ExchangeContactViewModel viewModel = new ExchangeContactViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            MailContactObject contact = viewModel.GetContact(distinguishedName);
            hfContactDistinguishedName.Value = distinguishedName;
            txtDisplayName.Text = contact.DisplayName;
            txtEmail.Text = contact.Email;
            cbHidden.Checked = contact.Hidden;

            txtEmail.ReadOnly = true;

            panelContactList.Visible = false;
            panelEditContact.Visible = true;
        }

        private void UpdateContact(string distinguishedName)
        {
            MailContactObject updateContact = new MailContactObject();
            updateContact.DisplayName = txtDisplayName.Text;
            updateContact.Hidden = cbHidden.Checked;

            // Create new contact
            ExchangeContactViewModel viewModel = new ExchangeContactViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            viewModel.UpdateContact(distinguishedName, updateContact);

            GetContacts();
        }

        private void DeleteContact(string distinguishedName)
        {
            // Create new contact
            ExchangeContactViewModel viewModel = new ExchangeContactViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            viewModel.DeleteContact(distinguishedName, WebSessionHandler.SelectedCompanyCode);

            GetContacts();
        }

        #region Events

        void viewModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            alertmessage.SetMessage(errorID, message);
        }

        #endregion

        #region Button Click events

        protected void btnAddContact_Click(object sender, EventArgs e)
        {
            hfContactDistinguishedName.Value = string.Empty;
            txtEmail.ReadOnly = false;
            txtDisplayName.Text = string.Empty;
            cbHidden.Checked = false;

            txtEmail.ReadOnly = false;

            panelContactList.Visible = false;
            panelEditContact.Visible = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfContactDistinguishedName.Value))
            {
                // Create new contact
                CreateContact();
            }
            else
            {
                // Update existing contact
                UpdateContact(hfContactDistinguishedName.Value);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            GetContacts();
        }

        #endregion

        protected void repeaterContactList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                GetContact(e.CommandArgument.ToString());
            }
            else if (e.CommandName == "Delete")
            {
                DeleteContact(e.CommandArgument.ToString());
            }
        }
    }
}