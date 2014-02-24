using CloudPanel.Modules.Database.Entity;
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

        private void PopulateCompanyData()
        {
            DB database = null;
            try
            {
                database = new DB();

                var domains = (from d in database.Domains
                               select d).ToList();

                // Bind to domain field
                ddlLoginDomain.DataTextField = "Domain1";
                ddlLoginDomain.DataValueField = "DomainID";
                ddlLoginDomain.DataSource = domains;
                ddlLoginDomain.DataBind();

                // Get accepted domains
                var acceptedDomains = (from d in domains
                                       where d.IsAcceptedDomain
                                       select d).ToList();

                // Bind to accepted domain field
                ddlAcceptedDomains.DataTextField = "Domain1";
                ddlAcceptedDomains.DataValueField = "DomainID";
                ddlAcceptedDomains.DataSource = acceptedDomains;
                ddlAcceptedDomains.DataBind();

                foreach (var d in domains)
                {
                    if (d.IsDefault)
                    {
                        ddlLoginDomain.SelectedValue = d.DomainID.ToString();
                        ddlAcceptedDomains.SelectedValue = d.DomainID.ToString();
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                alertmessage.SetMessage(Modules.Base.Enumerations.AlertType.ERROR, ex.ToString());
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        #region Button Click Events

        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            panelUserList.Visible = false;
            panelEditCreateUser.Visible = true;

            // Clear the hidden field since we are adding a new user
            hfEditUserPrincipalName.Value = string.Empty;

            // Populate company data
            PopulateCompanyData();
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            alertmessage.SetMessage(Modules.Base.Enumerations.AlertType.INFO, cbCompanyAdmin.Checked.ToString());
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