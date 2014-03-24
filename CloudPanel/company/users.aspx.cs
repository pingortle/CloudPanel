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
        protected bool _isExchangeEnabled = true;

        private List<MailAliasObject> emailAliases;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                emailAliases = new List<MailAliasObject>();

                GetUsers();
            }
            else
            {
                if (panelEditUser.Visible)
                {
                    emailAliases = (List<MailAliasObject>)ViewState["CPEmailAliases"];
                    gridEmailAliases.DataSource = emailAliases;
                    gridEmailAliases.DataBind();
                }
            }
        }

        private void GetDomains()
        {
            UsersViewModel viewModel = new UsersViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            List<DomainsObject> foundDomains = viewModel.GetDomains(WebSessionHandler.SelectedCompanyCode);
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
                        ddlEditMailboxDomain.DataSource = acceptedDomains;
                        ddlEditMailboxDomain.DataBind();

                        ddlEditAddEmailAliasDomain.DataSource = acceptedDomains;
                        ddlEditAddEmailAliasDomain.DataBind();

                        // Get email plans. This will show the Exchange panel if successful
                        GetMailboxPlans();
                    }
                }
            }
            else
            {
                alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.WARNING, Resources.LocalizedText.Users_Messages_NoDomainsFound);
                btnEditSave.Enabled = false;
            }
        }

        private void GetUsers()
        {
            UsersViewModel viewModel = new UsersViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            List<UsersObject> users = viewModel.GetUsers(WebSessionHandler.SelectedCompanyCode);
            repeaterUsers.DataSource = users;
            repeaterUsers.DataBind();

            panelCreateUser.Visible = false;
            panelEditUser.Visible = false;
            panelUserList.Visible = true;
        }

        private void GetUsersForPermissions()
        {
            UsersViewModel viewModel = new UsersViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            List<UsersObject> users = viewModel.GetUsers(WebSessionHandler.SelectedCompanyCode);
            
            // Filter only users with sAMAccountName
            users = users.FindAll(x => !string.IsNullOrEmpty(x.sAMAccountName));

            // Bind to list
            ddlEditMailboxFullAccess.DataSource = users;
            ddlEditMailboxFullAccess.DataBind();

            ddlEditMailboxSendAs.DataSource = users;
            ddlEditMailboxSendAs.DataBind();
        }

        private void GetMailboxPlans()
        {
            if (CompanyChecks.IsExchangeEnabled(WebSessionHandler.SelectedCompanyCode))
            {
                UsersViewModel viewModel = new UsersViewModel();
                viewModel.ViewModelEvent += viewModel_ViewModelEvent;

                List<MailboxPlanObject> mailboxPlans = viewModel.GetMailboxPlans(WebSessionHandler.SelectedCompanyCode);

                ddlEditMailboxPlan.Items.Clear();
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

                        ddlEditMailboxPlan.Items.Add(item);
                    }
                }
            }
        }

        private void GetForwardToList()
        {
            ddlEditMailboxForwardTo.Items.Clear();
            ddlEditMailboxForwardTo.Items.Add("Not Selected");
        }

        private void EditUser(string userPrincipalName)
        {
            UsersViewModel viewModel = new UsersViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            //                      //
            // GET USER INFORMATION //
            //                      //
            UsersObject user = viewModel.GetUser(userPrincipalName);
            if (user != null)
            {
                lbProfileDisplayName.Text = user.DisplayName;
                lbProfileUsername.Text = user.UserPrincipalName;
                lbProfileSamAccountName.Text = user.sAMAccountName;

                hfEditUserPrincipalName.Value = user.UserPrincipalName;
                txtEditFirstName.Text = user.Firstname;
                txtEditMiddleName.Text = user.Middlename;
                txtEditLastname.Text = user.Lastname;
                txtEditDisplayName.Text = user.DisplayName;
                txtEditDepartment.Text = user.Department;

                // Get the user photo
                imgUserPhoto.ImageUrl = string.Format("services/UserPhotoHandler.ashx?id={0}", user.UserPrincipalName);
            }

            //                          //
            // GET MAILBOX INFORMATION  //
            //                          //
            if (CompanyChecks.IsExchangeEnabled(WebSessionHandler.SelectedCompanyCode))
            {
                // Get list of accepted domains
                GetDomains();

                // Get list of mailbox plans
                GetMailboxPlans();

                // Get users that can be granted full access and send as permissions
                GetUsersForPermissions();

                // Get mailbox information
                if (user != null)
                {
                    if (user.MailboxPlan > 0)
                    {
                        ListItem item = ddlEditMailboxPlan.Items.FindByValue(user.MailboxPlan.ToString());
                        if (item != null)
                            ddlEditMailboxPlan.SelectedValue = item.Value;
                    }

                    UsersObject mailboxUser = viewModel.GetUserMailbox(userPrincipalName);
                    if (mailboxUser != null)
                    {
                        string[] primaryEmailAddress = mailboxUser.PrimarySmtpAddress.Split('@');

                        // Populate email information
                        txtEditMailboxEmail.Text = primaryEmailAddress[0];
                        ListItem item = ddlEditMailboxDomain.Items.FindByText(primaryEmailAddress[1]);
                        if (item != null)
                            ddlEditMailboxDomain.SelectedValue = item.Value;

                        // Populate email aliases
                        emailAliases = new List<MailAliasObject>();
                        foreach (string s in mailboxUser.EmailAliases)
                        {
                            emailAliases.Add(new MailAliasObject() { Email = s });
                        }
                        ViewState["CPEmailAliases"] = emailAliases;
                        gridEmailAliases.DataSource = emailAliases;
                        gridEmailAliases.DataBind();

                        // Populate forwarding
                        if (!string.IsNullOrEmpty(mailboxUser.ForwardingTo))
                        {
                            ListItem fItem = ddlEditMailboxForwardTo.Items.FindByValue(mailboxUser.ForwardingTo);
                            if (fItem != null)
                                ddlEditMailboxForwardTo.SelectedValue = fItem.Value;
                            else
                                ddlEditMailboxForwardTo.SelectedIndex = -1;
                        }
                        cbEditMailboxForwardBoth.Checked = mailboxUser.DeliverToMailboxAndForward;

                        // Populate permissions
                        foreach (string fullAccess in mailboxUser.FullAccessUsers)
                        {
                            ListItem fullItem = ddlEditMailboxFullAccess.Items.FindByValue(fullAccess);
                            if (fullItem != null)
                                fullItem.Selected = true;
                        }

                        foreach (string sendAs in mailboxUser.SendAsUsers)
                        {
                            ListItem sendAsItem = ddlEditMailboxSendAs.Items.FindByValue(sendAs);
                            if (sendAsItem != null)
                                sendAsItem.Selected = true;
                        }

                    }
                }
            }

            // Change panels
            panelCreateUser.Visible = false;
            panelUserList.Visible = false;
            panelEditUser.Visible = true;
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

            // Show the panels
            panelUserList.Visible = false;
            panelEditUser.Visible = false;
            panelCreateUser.Visible = true;
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            SaveNewUser();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            // GET LIST OF USERS
            panelUserList.Visible = true;
            panelEditUser.Visible = false;
            panelCreateUser.Visible = false;
        }

        protected void btnEditInsertEmailAlias_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtEditAddEmailAlias.Text) || string.IsNullOrEmpty(ddlEditAddEmailAliasDomain.SelectedValue))
                return;
            else
            {
                string enteredEmail = Server.HtmlEncode(txtEditAddEmailAlias.Text.Replace(" ", string.Empty));
                string enteredDomain = ddlEditAddEmailAliasDomain.SelectedItem.Text;

                string formattedEmail = string.Format("{0}@{1}", enteredEmail, enteredDomain);

                // Don't add if it already exists
                if (emailAliases.Find(x => x.Email.Equals(formattedEmail, StringComparison.CurrentCultureIgnoreCase)) == null)
                {
                    // Add to our list
                    emailAliases.Add(new MailAliasObject()
                        {
                            Email = formattedEmail
                        });

                    // Set the viewstate
                    ViewState["CPEmailAliases"] = emailAliases;

                    // Rebind the grid view
                    gridEmailAliases.DataSource = emailAliases;
                    gridEmailAliases.DataBind();
                }
            }
        }

        #endregion

        #region Other

        protected void repeaterUsers_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                // Clear list box selected items
                ddlEditMailboxFullAccess.ClearSelection();
                ddlEditMailboxSendAs.ClearSelection();

                EditUser(e.CommandArgument.ToString());
            }
            else if (e.CommandName == "Delete")
                DeleteUser(e.CommandArgument.ToString());
        }

        #endregion

        #region Gridview

        protected void gridEmailAliases_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Delete")
            {
                string selectedEmail = e.CommandArgument.ToString();

                // Remove from our list
                emailAliases.RemoveAll(x => x.Email.Equals(selectedEmail, StringComparison.CurrentCultureIgnoreCase));

                // Set view state
                ViewState["CPEmailAliases"] = emailAliases;

                // Rebind
                gridEmailAliases.DataBind();
            }
        }

        protected void gridEmailAliases_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            TableCell cell = gridEmailAliases.Rows[e.RowIndex].Cells[0];

            // Remove from our list
            emailAliases.RemoveAll(x => x.Email.Equals(cell.Text, StringComparison.CurrentCultureIgnoreCase));

            // Set view state
            ViewState["CPEmailAliases"] = emailAliases;

            // Rebind
            gridEmailAliases.DataBind();
        }

        protected void gridEmailAliases_PreRender(object sender, EventArgs e)
        {
            if (gridEmailAliases.Rows.Count > 0)
            {
                gridEmailAliases.UseAccessibleHeader = true;
                gridEmailAliases.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        #endregion
    }
}