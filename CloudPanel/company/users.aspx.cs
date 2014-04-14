using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Base.Exchange;
using CloudPanel.Modules.Base.Plans;
using CloudPanel.Modules.Base.Users;
using CloudPanel.Modules.Common.GlobalActions;
using CloudPanel.Modules.Common.Settings;
using CloudPanel.Modules.Common.ViewModel;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.UI.WebControls;

namespace CloudPanel.company
{
    public partial class users : System.Web.UI.Page
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected bool _isExchangeEnabled = false;
        protected int _currentMailboxSize = 0;

        private List<MailAliasObject> emailAliases;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                emailAliases = new List<MailAliasObject>();
                ViewState["CPEmailAliases"] = emailAliases;

                // Populate all the static fields that maintain viewstate
                PopulateStaticFields();

                // Check if we have a session of search result to load a particular user
                if (Session["CPSearchResultUser"] != null)
                {
                    EditUser(Session["CPSearchResultUser"].ToString());
                    Session["CPSearchResultUser"] = null;
                }
                else
                    PopulateUsersListView();
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

        private void PopulateStaticFields()
        {
            UsersViewModel viewModel = new UsersViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            List<DomainsObject> foundDomains = viewModel.GetDomains(WebSessionHandler.SelectedCompanyCode);
            ddlLoginDomain.DataSource = foundDomains;
            ddlLoginDomain.DataBind();
            
            // Don't populate Exchange information if the company isn't enabled for Exchange
            if (CompanyChecks.IsExchangeEnabled(WebSessionHandler.SelectedCompanyCode))
            {
                // Get list of users
                List<UsersObject> users = viewModel.GetUsers(WebSessionHandler.SelectedCompanyCode);

                //
                // User Permissions dropdown boxes
                //
                // Filter only users with sAMAccountName
                List<UsersObject> permissionUsers = users.FindAll(x => !string.IsNullOrEmpty(x.sAMAccountName));

                // Bind to list
                ddlEditMailboxFullAccess.DataSource = permissionUsers;
                ddlEditMailboxSendAs.DataSource = permissionUsers;
                ddlEditMailboxSendOnBehalf.DataSource = permissionUsers;

                ddlEditMailboxFullAccess.DataBind();
                ddlEditMailboxSendAs.DataBind();
                ddlEditMailboxSendOnBehalf.DataBind();

                //
                // Populate the forward to list
                // 
                ddlEditMailboxForwardTo.Items.Clear();
                ddlEditMailboxForwardTo.Items.Add("Not Selected");

                //
                // Populate the accepted domain fields
                //
                var acceptedDomains = from a in foundDomains where a.IsAcceptedDomain select a;

                // If we found accepted domains then we can show the email section
                if (acceptedDomains != null && acceptedDomains.Count() > 0)
                {
                    ddlEditMailboxDomain.DataSource = acceptedDomains;
                    ddlEditMailboxDomain.DataBind();

                    ddlEditAddEmailAliasDomain.DataSource = acceptedDomains;
                    ddlEditAddEmailAliasDomain.DataBind();
                }
            }
        }

        private void PopulateUsersListView()
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

        private void PopulateEditUserView()
        {
            UsersViewModel viewModel = new UsersViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            // Don't populate Exchange information if the company isn't enabled for Exchange
            if (CompanyChecks.IsExchangeEnabled(WebSessionHandler.SelectedCompanyCode))
            {
                //
                // Populate the mailbox plans
                //
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
            PopulateUsersListView();
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
            PopulateUsersListView();
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

                cbEditIsCompanyAdmin.Checked = user.IsCompanyAdmin;
                cbEditIsResellerAdmin.Checked = user.IsResellerAdmin;
                cbEditEnableUser.Checked = user.IsEnabled;

                cbEditAddDomain.Checked = user.AddDomainPerm;
                cbEditDeleteDomain.Checked = user.DeleteDomainPerm;
                cbEditDisableAcceptedDomain.Checked = user.DisableAcceptedDomainPerm;
                cbEditDisableExchange.Checked = user.DisableExchangePerm;
                cbEditEnableAcceptedDomain.Checked = user.EnableAcceptedDomainPerm;
                cbEditEnableExchange.Checked = user.EnableExchangePerm;

                cbEditMailboxEnableArchiving.Checked = user.ArchivePlan > 0 ? true : false;

                // Get the user photo
                imgUserPhoto.ImageUrl = string.Format("services/UserPhotoHandler.ashx?id={0}", user.UserPrincipalName);

                // Set view state
                ViewState["CPCurrentEditUser"] = user;
            }

            //                          //
            // GET MAILBOX INFORMATION  //
            //                          //
            _isExchangeEnabled = CompanyChecks.IsExchangeEnabled(WebSessionHandler.SelectedCompanyCode);
            if (_isExchangeEnabled)
            {
                // Get list of accepted domains
                PopulateEditUserView();

                // Get mailbox information
                if (user != null)
                {
                    if (user.MailboxPlan > 0)
                    {
                        cbEditIsMailboxEnabled.Checked = true;

                        MailboxPlanObject mailboxPlan = viewModel.GetMailboxPlan(user.MailboxPlan);
                        _currentMailboxSize = mailboxPlan.MailboxSizeInMB + user.AdditionalMB;

                        ListItem item = ddlEditMailboxPlan.Items.FindByValue(mailboxPlan.MailboxPlanID.ToString());
                        if (item != null)
                            ddlEditMailboxPlan.SelectedValue = item.Value;

                        UsersObject mailboxUser = viewModel.GetUserMailbox(userPrincipalName);
                        if (mailboxUser != null)
                        {
                            string[] primaryEmailAddress = mailboxUser.PrimarySmtpAddress.Split('@');

                            // Populate email information
                            txtEditMailboxEmail.Text = primaryEmailAddress[0];
                            ListItem item2 = ddlEditMailboxDomain.Items.FindByText(primaryEmailAddress[1]);
                            if (item2 != null)
                                ddlEditMailboxDomain.SelectedValue = item2.Value;

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
                            if (mailboxUser.FullAccessUsers != null)
                            {
                                foreach (string fullAccess in mailboxUser.FullAccessUsers)
                                {
                                    ListItem fullItem = ddlEditMailboxFullAccess.Items.FindByValue(fullAccess);
                                    if (fullItem != null)
                                        fullItem.Selected = true;
                                }
                            }

                            if (mailboxUser.SendAsUsers != null)
                            {
                                foreach (string sendAs in mailboxUser.SendAsUsers)
                                {
                                    ListItem sendAsItem = ddlEditMailboxSendAs.Items.FindByValue(sendAs);
                                    if (sendAsItem != null)
                                        sendAsItem.Selected = true;
                                }
                            }

                            if (mailboxUser.SendOnBehalf != null)
                            {
                                foreach (string sendOnBehalf in mailboxUser.SendOnBehalf)
                                {
                                    ListItem sendOnBehalfItem = ddlEditMailboxSendOnBehalf.Items.FindByValue(sendOnBehalf);
                                    if (sendOnBehalfItem != null)
                                        sendOnBehalfItem.Selected = true;
                                }
                            }

                            // Populate litigation hold
                            cbEditMailboxEnableLitigationHold.Checked = mailboxUser.LitigationHoldEnabled;
                            txtEditMailboxLitigationHoldURL.Text = mailboxUser.LitigationHoldUrl;
                            txtEditMailboxLitigationHoldComments.Text = mailboxUser.LitigationHoldComment;

                            if (mailboxUser.LitigationHoldDuration > 0)
                            {
                                DateTime now = DateTime.Now.AddDays(mailboxUser.LitigationHoldDuration);
                                txtEditMailboxLitigationHoldDuration.Text = now.ToShortDateString();
                            }
                            else
                                txtEditMailboxLitigationHoldDuration.Text = string.Empty;

                            // Populate archive
                            if (user.ArchivePlan > 0)
                            {
                                txtEditMailboxArchiveName.Text = mailboxUser.ArchiveName;
                            }
                            else
                                txtEditMailboxArchiveName.Text = string.Empty;
                            

                            ViewState["CPCurrentEditMailbox"] = mailboxUser;
                        }
                    }
                    else
                    {
                        cbEditIsMailboxEnabled.Checked = false;
                    }
                }
            }

            // Change panels
            panelCreateUser.Visible = false;
            panelUserList.Visible = false;
            panelEditUser.Visible = true;
        }

        #region Events

        void viewModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            if (errorID == AlertID.USER_ALREADY_EXISTS)
                alertmessage.SetMessage(AlertID.WARNING, string.Format("{0}: {1}", Resources.LocalizedText.Users_UserAlreadyExist, message));
            else if (errorID == AlertID.USER_UNKNOWN)
                alertmessage.SetMessage(AlertID.FAILED, string.Format("{0}: {1}", Resources.LocalizedText.Users_CouldNotFindUser, message));
            else if (errorID == AlertID.PASSWORDS_DO_NOT_MATCH)
                alertmessage.SetMessage(AlertID.WARNING, string.Format("{0}: {1}", Resources.LocalizedText.Users_PasswordsDoNotMatch, message));
            else
                alertmessage.SetMessage(errorID, message);
        }

        #endregion

        #region Button Click Events

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
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
                    gridEmailAliases.DataBind();
                }
            }
        }

        protected void btnEditSave_Click(object sender, EventArgs e)
        {
            // Check for changes values in the users section
            UpdateUserSection();

            // Check for changed values int he mailbox settings section
            UpdateExchangeSection();

            // Refresh to users section
            PopulateUsersListView();
        }

        protected void btnResetPwd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtResetPwd1.Text) && !string.IsNullOrEmpty(txtResetPwd2.Text))
            {
                if (txtResetPwd1.Text != txtResetPwd2.Text)
                    alertmessage.SetMessage(AlertID.WARNING, Resources.LocalizedText.Users_PasswordsDoNotMatch);
                else
                {
                    UsersViewModel viewModel = new UsersViewModel();
                    viewModel.ViewModelEvent += viewModel_ViewModelEvent;
                    viewModel.ResetPassword(hfResetPwdHiddenField.Value, txtResetPwd2.Text, WebSessionHandler.SelectedCompanyCode);

                    AuditGlobal.AddAudit(WebSessionHandler.SelectedCompanyCode, WebSessionHandler.Username, ActionID.ResetPassword, hfResetPwdHiddenField.Value);
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
            else if (e.CommandName == "ResetPassword")
            {

            }
            else if (e.CommandName == "Delete")
            {
                DeleteUser(e.CommandArgument.ToString());
            }
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

        #region Update User Actions

        internal void UpdateUserSection()
        {
            object userObject = ViewState["CPCurrentEditUser"];
            if (userObject == null)
                alertmessage.SetMessage(AlertID.FAILED, Resources.LocalizedText.Users_ViewStateNull);
            else
            {

                UsersObject original = userObject as UsersObject;

                bool valuesHaveBeenUpdated = false;

                if (txtEditFirstName.Text != original.Firstname)
                {
                    valuesHaveBeenUpdated = true;
                    this.logger.Debug(string.Format("{0}: Found new value. Old Value: {0}, New Value: {1}", original.UserPrincipalName, original.Firstname, txtEditFirstName.Text));
                }

                if (txtEditMiddleName.Text != original.Middlename)
                {
                    valuesHaveBeenUpdated = true;
                    this.logger.Debug(string.Format("{0}: Found new value. Old Value: {0}, New Value: {1}", original.UserPrincipalName, original.Middlename, txtEditMiddleName.Text));
                }

                if (txtEditLastname.Text != original.Lastname)
                {
                    valuesHaveBeenUpdated = true;
                    this.logger.Debug(string.Format("{0}: Found new value. Old Value: {0}, New Value: {1}", original.UserPrincipalName, original.Lastname, txtEditLastname.Text));
                }

                if (txtEditDisplayName.Text != original.DisplayName)
                {
                    valuesHaveBeenUpdated = true;
                    this.logger.Debug(string.Format("{0}: Found new value. Old Value: {0}, New Value: {1}", original.UserPrincipalName, original.DisplayName, txtEditDisplayName.Text));
                }

                if (txtEditDepartment.Text != original.Department)
                {
                    valuesHaveBeenUpdated = true;
                    this.logger.Debug(string.Format("{0}: Found new value. Old Value: {0}, New Value: {1}", original.UserPrincipalName, original.Department, txtEditDepartment.Text));
                }

                if (cbEditEnableUser.Checked != original.IsEnabled)
                    valuesHaveBeenUpdated = true;

                // Only update these if reseller or super admin
                if (WebSessionHandler.IsSuperAdmin || WebSessionHandler.IsResellerAdmin)
                {
                    if (cbEditIsCompanyAdmin.Checked != original.IsCompanyAdmin)
                        valuesHaveBeenUpdated = true;

                    if (cbEditEnableExchange.Checked != original.EnableExchangePerm)
                        valuesHaveBeenUpdated = true;

                    if (cbEditDisableExchange.Checked != original.DisableExchangePerm)
                        valuesHaveBeenUpdated = true;

                    if (cbEditAddDomain.Checked != original.AddDomainPerm)
                        valuesHaveBeenUpdated = true;

                    if (cbEditDeleteDomain.Checked != original.DeleteDomainPerm)
                        valuesHaveBeenUpdated = true;

                    if (cbEditEnableAcceptedDomain.Checked != original.EnableAcceptedDomainPerm)
                        valuesHaveBeenUpdated = true;

                    if (cbEditDisableAcceptedDomain.Checked != original.DisableAcceptedDomainPerm)
                        valuesHaveBeenUpdated = true;

                    if (WebSessionHandler.IsSuperAdmin)
                    {
                        if (cbEditIsResellerAdmin.Checked != original.IsResellerAdmin)
                            valuesHaveBeenUpdated = true;
                    }
                }

                // Update user if values have changed
                if (valuesHaveBeenUpdated)
                {
                    UsersObject updateUser = new UsersObject();
                    updateUser.UserPrincipalName = hfEditUserPrincipalName.Value;
                    updateUser.Firstname = txtEditFirstName.Text;
                    updateUser.Middlename = txtEditMiddleName.Text;
                    updateUser.Lastname = txtEditLastname.Text;
                    updateUser.DisplayName = txtEditDisplayName.Text;
                    updateUser.Department = txtEditDepartment.Text;
                    updateUser.IsEnabled = cbEditEnableUser.Checked;
                    updateUser.IsResellerAdmin = cbEditIsResellerAdmin.Checked;
                    updateUser.IsCompanyAdmin = cbEditIsCompanyAdmin.Checked;
                    updateUser.EnableExchangePerm = cbEditEnableExchange.Checked;
                    updateUser.DisableExchangePerm = cbEditDisableExchange.Checked;
                    updateUser.AddDomainPerm = cbEditAddDomain.Checked;
                    updateUser.DeleteDomainPerm = cbEditDeleteDomain.Checked;
                    updateUser.EnableAcceptedDomainPerm = cbEditEnableAcceptedDomain.Checked;
                    updateUser.DisableAcceptedDomainPerm = cbEditDisableAcceptedDomain.Checked;

                    UsersViewModel viewModel = new UsersViewModel();
                    viewModel.ViewModelEvent += viewModel_ViewModelEvent;
                    viewModel.UpdateUser(updateUser, WebSessionHandler.IsSuperAdmin || WebSessionHandler.IsResellerAdmin);
                }
            }
        }

        internal void UpdateExchangeSection()
        {
            object userObject = ViewState["CPCurrentEditUser"];
            object mailboxObject = ViewState["CPCurrentEditMailbox"];
            if (userObject == null)
                alertmessage.SetMessage(AlertID.FAILED, Resources.LocalizedText.Users_ViewStateNull);
            else
            {
                UsersViewModel viewModel = new UsersViewModel();
                viewModel.ViewModelEvent += viewModel_ViewModelEvent;

                UsersObject user = userObject as UsersObject;
                if (!cbEditIsMailboxEnabled.Checked && user.MailboxPlan > 0)
                {
                    //
                    // We are disabling the mailbox for this user
                    //
                    viewModel.DisableMailbox(user.UserPrincipalName);
                }
                else if (cbEditIsMailboxEnabled.Checked && user.MailboxPlan == 0)
                {
                    //
                    // We are creating a new mailbox
                    //
                    this.logger.Debug("Attempting to create new mailbox for " + user.UserPrincipalName);

                    user = new UsersObject();
                    user.CompanyCode = WebSessionHandler.SelectedCompanyCode;
                    user.UserPrincipalName = hfEditUserPrincipalName.Value;
                    user.PrimarySmtpAddress = string.Format("{0}@{1}", txtEditMailboxEmail.Text.Replace(" ", string.Empty), ddlEditMailboxDomain.SelectedItem.Text);
                    user.ActiveSyncPlan = ddlEditMailboxASPlan.SelectedIndex > 0 ? int.Parse(ddlEditMailboxASPlan.SelectedValue) : 0;
                    user.MailboxPlan = int.Parse(ddlEditMailboxPlan.SelectedValue);
                    user.SetMailboxSizeInMB = int.Parse(hfEditSelectedMailboxSize.Value);
                    user.ForwardingTo = ddlEditMailboxForwardTo.SelectedIndex > 0 ? ddlEditMailboxForwardTo.SelectedValue : string.Empty;
                    user.DeliverToMailboxAndForward = cbEditMailboxForwardBoth.Checked;

                    this.logger.Debug("Validating email addresses for " + user.UserPrincipalName);
                    user.EmailAliases = new List<string>();
                    if (emailAliases != null)
                    {
                        foreach (MailAliasObject a in emailAliases)
                        {
                            if (!a.Email.Equals(user.PrimarySmtpAddress))
                                user.EmailAliases.Add(a.Email);
                        }
                    }

                    this.logger.Debug("Validating access permissions for " + user.UserPrincipalName);
                    user.FullAccessUsers = new List<string>();
                    foreach (int i in ddlEditMailboxFullAccess.GetSelectedIndices())
                        user.FullAccessUsers.Add(ddlEditMailboxFullAccess.Items[i].Value);

                    user.SendAsUsers = new List<string>();
                    foreach (int i in ddlEditMailboxSendAs.GetSelectedIndices())
                        user.SendAsUsers.Add(ddlEditMailboxSendAs.Items[i].Value);

                    user.SendOnBehalf = new List<string>();
                    foreach (int i in ddlEditMailboxSendOnBehalf.GetSelectedIndices())
                        user.SendOnBehalf.Add(ddlEditMailboxSendOnBehalf.Items[i].Value);

                    //
                    // Archiving
                    // 
                    this.logger.Debug("Validating archiving settings for " + user.UserPrincipalName);
                    if (cbEditMailboxEnableArchiving.Checked)
                    {
                        user.ArchivingEnabled = cbEditMailboxEnableArchiving.Checked;
                        user.ArchiveName = txtEditMailboxArchiveName.Text.Trim();
                        user.ArchivePlan = ddlEditMailboxArchivePlan.SelectedIndex > 0 ? int.Parse(ddlEditMailboxArchivePlan.SelectedValue) : 0;
                    }
                    else
                        user.ArchivingEnabled = false;

                    //
                    // Litigation Hold
                    //
                    this.logger.Debug("Validating litigation hold settings for " + user.UserPrincipalName);
                    if (cbEditMailboxEnableLitigationHold.Checked)
                    {
                        user.LitigationHoldEnabled = cbEditMailboxEnableLitigationHold.Checked;
                        user.LitigationHoldUrl = txtEditMailboxLitigationHoldURL.Text;
                        user.LitigationHoldComment = txtEditMailboxLitigationHoldComments.Text;

                        if (!string.IsNullOrEmpty(txtEditMailboxLitigationHoldDuration.Text))
                        {
                            DateTime now = DateTime.Now;
                            DateTime end;

                            DateTime.TryParse(txtEditMailboxLitigationHoldDuration.Text, out end);
                            if (end != null)
                            {
                                TimeSpan duration = end - now;
                                user.LitigationHoldDuration = duration.Days;
                            }
                            else
                                user.LitigationHoldDuration = 0;
                        }
                        else
                            user.LitigationHoldDuration = 0;
                    }

                    viewModel.CreateMailbox(user);
                }
                else
                {
                    //
                    // We are updating an existing mailbox
                    //
                     
                }
            }
        }

        #endregion

        #region Get actions



        #endregion
    }
}