using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Base.Exchange;
using CloudPanel.Modules.Base.Plans;
using CloudPanel.Modules.Base.Users;
using CloudPanel.Modules.Common.GlobalActions;
using CloudPanel.Modules.Common.Settings;
using CloudPanel.Modules.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.company
{
    public partial class users : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetUsers();
            }
        }

        private void GetDomains()
        {
            UsersViewModel viewModel = new UsersViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            List<Domains> foundDomains = viewModel.GetDomains(WebSessionHandler.SelectedCompanyCode);
            if (foundDomains != null)
            {
                ddlLoginDomain.DataSource = foundDomains;
                ddlLoginDomain.DataBind();

                // If Exchange is enabled then populate the accepted domains fields
                if (StaticSettings.ExchangeEnabled && CompanyChecks.IsExchangeEnabled(WebSessionHandler.SelectedCompanyCode))
                {
                    var acceptedDomains = from a in foundDomains where a.IsAcceptedDomain select a;

                    // If we found accepted domains then we can show the email section
                    if (acceptedDomains != null && acceptedDomains.Count() > 0)
                    {
                        ddlAcceptedDomains.DataSource = acceptedDomains;
                        ddlAcceptedDomains.DataBind();

                        cbEnableMailbox.Checked = false;

                        // Get email plans. This will show the Exchange panel if successful
                        GetMailboxPlans();
                    }
                    else
                    {
                        // No accepted domains were found so hide the email panel
                        panelEmail.Visible = false;
                        cbEnableMailbox.Checked = false;
                    }
                }
                else
                {
                    panelEmail.Visible = false;
                    cbEnableMailbox.Checked = false;
                }
            }
            else
            {
                alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.WARNING, Resources.LocalizedText.Users_Messages_NoDomainsFound);
                btnSubmit.Enabled = false;
            }
        }

        private void GetUsers()
        {
            UsersViewModel viewModel = new UsersViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            List<UsersObject> users = viewModel.GetUsers(WebSessionHandler.SelectedCompanyCode);
            repeaterUsers.DataSource = users;
            repeaterUsers.DataBind();

            panelEditCreateUser.Visible = false;
            panelUserList.Visible = true;
        }

        private void GetMailboxPlans()
        {
            if (CompanyChecks.IsExchangeEnabled(WebSessionHandler.SelectedCompanyCode))
            {
                UsersViewModel viewModel = new UsersViewModel();
                viewModel.ViewModelEvent += viewModel_ViewModelEvent;

                List<MailboxPlanObject> mailboxPlans = viewModel.GetMailboxPlans(WebSessionHandler.SelectedCompanyCode);

                ddlMailboxPlan.Items.Clear();
                if (mailboxPlans != null && mailboxPlans.Count > 0)
                {
                    foreach (MailboxPlanObject o in mailboxPlans)
                    {
                        ListItem item = new ListItem();
                        item.Value = o.MailboxPlanID.ToString();
                        item.Text = o.MailboxPlanName;
                        item.Attributes.Add("Description", o.MailboxPlanDescription);
                        item.Attributes.Add("Min", o.MailboxSizeInMB.ToString());
                        item.Attributes.Add("Max", o.MaxMailboxSizeInMB.ToString());
                        item.Attributes.Add("Price", o.Price);
                        item.Attributes.Add("Extra", o.AdditionalGBPrice);

                        ddlMailboxPlan.Items.Add(item);
                    }

                    ddlMailboxPlan.SelectedIndex = 0;
                    panelEmail.Visible = true;
                }
                else
                {
                    panelEmail.Visible = false;
                }
            }
            else
                panelEmail.Visible = false;
        }

        private void GetUser(string userPrincipalName)
        {
            UsersViewModel viewModel = new UsersViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            UsersObject user = viewModel.GetUser(userPrincipalName);

            if (user != null)
            {
                hfEditUserPrincipalName.Value = user.UserPrincipalName;
                txtFirstName.Text = user.Firstname;
                txtMiddleName.Text = user.Middlename;
                txtLastname.Text = user.Lastname;
                txtDisplayName.Text = user.DisplayName;
                txtDepartment.Text = user.Department;
                txtLoginName.Text = user.UserPrincipalName.Split('@')[0];

                // User Rights
                if (user.IsCompanyAdmin)
                {
                    cbCompanyAdmin.Checked = true;

                    // Set permissions checkbox
                    cbEnableExchange.Checked = user.EnableExchangePerm;
                    cbDisableExchange.Checked = user.DisableExchangePerm;
                    cbAddDomain.Checked = user.AddDomainPerm;
                    cbDeleteDomain.Checked = user.DeleteDomainPerm;
                    cbEnableAcceptedDomain.Checked = user.EnableAcceptedDomainPerm;
                    cbDisableAcceptedDomain.Checked = user.DisableAcceptedDomainPerm;
                }
                else
                    cbCompanyAdmin.Checked = false;

                if (user.IsResellerAdmin)
                    cbResellerAdmin.Checked = true;
                else
                    cbResellerAdmin.Checked = false;

                // Populate the domains field
                List<Domains> foundDomains = viewModel.GetDomains(WebSessionHandler.SelectedCompanyCode);
                if (foundDomains != null)
                {
                    ddlLoginDomain.DataSource = foundDomains;
                    ddlLoginDomain.DataBind();

                    // Populate domain field
                    ListItem item = ddlLoginDomain.Items.FindByText(user.UserPrincipalName.Split('@')[1]);
                    if (item != null)
                        ddlLoginDomain.SelectedValue = item.Value;
                }

                // Disable login name and domain because this cannot be changed
                txtLoginName.Enabled = false;
                ddlLoginDomain.Enabled = false;

                // Change panels
                panelEditCreateUser.Visible = true;
                panelUserList.Visible = false;
                panelEmail.Visible = false; // Can't edit email settings from the users section. Does this from the mailbox section
            }
        }

        private void CheckUserRights()
        {
            if (WebSessionHandler.IsSuperAdmin || WebSessionHandler.IsResellerAdmin)
                panelUserRights.Enabled = true;
            else
                panelUserRights.Enabled = false;
        }

        private void SaveNewUser()
        {
            UsersObject newUser = new UsersObject();
            newUser.CompanyCode = WebSessionHandler.SelectedCompanyCode;
            newUser.Firstname = txtFirstName.Text;
            newUser.Middlename = txtMiddleName.Text;
            newUser.Lastname = txtLastname.Text;
            newUser.DisplayName = txtDisplayName.Text;
            newUser.Department = txtDepartment.Text;
            newUser.UserPrincipalName = string.Format("{0}@{1}", txtLoginName.Text, ddlLoginDomain.SelectedItem.Text);
            newUser.Password = txtPassword1.Text;
            newUser.PasswordNeverExpires = cbPasswordNeverExpires.Checked;
            newUser.IsCompanyAdmin = cbCompanyAdmin.Checked;
            newUser.IsResellerAdmin = cbResellerAdmin.Checked;

            if (newUser.IsCompanyAdmin)
            {
                newUser.EnableExchangePerm = cbEnableExchange.Checked;
                newUser.DisableExchangePerm = cbDisableExchange.Checked;
                newUser.AddDomainPerm = cbAddDomain.Checked;
                newUser.DeleteDomainPerm = cbDeleteDomain.Checked;
                newUser.EnableAcceptedDomainPerm = cbEnableAcceptedDomain.Checked;
                newUser.DisableAcceptedDomainPerm = cbDisableAcceptedDomain.Checked;
            }

            UsersViewModel viewModel = new UsersViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            // Create new user
            viewModel.CreateUser(newUser);

            // Audit
            AuditGlobal.AddAudit(WebSessionHandler.SelectedCompanyCode, HttpContext.Current.User.Identity.Name, ActionID.CreateUser, newUser.UserPrincipalName, null);

            // Refresh
            GetUsers();
        }

        private void UpdateUser()
        {
            UsersObject existingUser = new UsersObject();
            existingUser.CompanyCode = WebSessionHandler.SelectedCompanyCode;
            existingUser.Firstname = txtFirstName.Text;
            existingUser.Middlename = txtMiddleName.Text;
            existingUser.Lastname = txtLastname.Text;
            existingUser.DisplayName = txtDisplayName.Text;
            existingUser.Department = txtDepartment.Text;
            existingUser.UserPrincipalName = hfEditUserPrincipalName.Value;
            existingUser.PasswordNeverExpires = cbPasswordNeverExpires.Checked;
            existingUser.IsCompanyAdmin = cbCompanyAdmin.Checked;
            existingUser.IsResellerAdmin = cbResellerAdmin.Checked;

            if (existingUser.IsCompanyAdmin)
            {
                existingUser.EnableExchangePerm = cbEnableExchange.Checked;
                existingUser.DisableExchangePerm = cbDisableExchange.Checked;
                existingUser.AddDomainPerm = cbAddDomain.Checked;
                existingUser.DeleteDomainPerm = cbDeleteDomain.Checked;
                existingUser.EnableAcceptedDomainPerm = cbEnableAcceptedDomain.Checked;
                existingUser.DisableAcceptedDomainPerm = cbDisableAcceptedDomain.Checked;
            }

            if (!string.IsNullOrEmpty(txtPassword1.Text))
                existingUser.Password = txtPassword1.Text;

            UsersViewModel viewModel = new UsersViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            // Create new user
            viewModel.UpdateUser(existingUser, WebSessionHandler.IsSuperAdmin || WebSessionHandler.IsResellerAdmin);

            // Audit
            AuditGlobal.AddAudit(WebSessionHandler.SelectedCompanyCode, HttpContext.Current.User.Identity.Name, ActionID.UpdateUser, existingUser.UserPrincipalName, null);

            // Refresh
            GetUsers();
        }

        private void DeleteUser(string userPrincipalName)
        {
            UsersViewModel viewModel = new UsersViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            // Create new user
            viewModel.DeleteUser(userPrincipalName);

            // Audit
            AuditGlobal.AddAudit(WebSessionHandler.SelectedCompanyCode, HttpContext.Current.User.Identity.Name, ActionID.DeleteUser, userPrincipalName);

            // Refresh
            GetUsers();
        }

        #region Events

        void viewModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            if (errorID == AlertID.USER_ALREADY_EXISTS)
                alertmessage.SetMessage(AlertID.WARNING, string.Format("{0}: {1}", Resources.LocalizedText.Users_UserAlreadyExist, message));
            else if (errorID == AlertID.USER_UNKNOWN)
                alertmessage.SetMessage(AlertID.FAILED, string.Format("{0}: {1}", Resources.LocalizedText.Users_CouldNotFindUser, message));
            else
                alertmessage.SetMessage(errorID, message);
        }

        #endregion

        #region Button Click Events

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            // Get Domains
            GetDomains();

            // Check if the user has rights to modify the user rights section
            CheckUserRights();

            // Enable login name and domain
            txtLoginName.Enabled = true;
            ddlLoginDomain.Enabled = true;

            // Show the panels
            panelUserList.Visible = false;
            panelEditCreateUser.Visible = true;

            // Clear the hidden field since we are adding a new user
            hfEditUserPrincipalName.Value = string.Empty;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfEditUserPrincipalName.Value))
            {
                SaveNewUser();
            }
            else
            {
                UpdateUser();
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // GET LIST OF USERS
            panelUserList.Visible = true;
            panelEditCreateUser.Visible = false;
        }

        #endregion

        #region Other

        protected void repeaterUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
                GetUser(e.CommandArgument.ToString());
            else if (e.CommandName == "Delete")
                DeleteUser(e.CommandArgument.ToString());
        }

        #endregion
    }
}