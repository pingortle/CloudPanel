using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Common.ViewModel;
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
            if (!IsPostBack)
            {
                GetDomains();
            }
        }

        private void GetDomains()
        {
            panelEditCreateDomain.Visible = false;
            panelDomainList.Visible = true;

            DomainsViewModel viewModel = new DomainsViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            List<DomainsObject> domains = viewModel.GetDomains(WebSessionHandler.SelectedCompanyCode);
            if (domains != null)
            {
                repeaterDomains.DataSource = domains;
                repeaterDomains.DataBind();
            }
        }

        private void ModifyDomain(int domainID)
        {
            hfDomainID.Value = domainID.ToString();

            DomainsViewModel viewModel = new DomainsViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            DomainsObject domain = viewModel.GetDomain(domainID);
            txtDomainName.Text = domain.DomainName;
            cbIsDefaultDomain.Checked = domain.IsDefault;

            txtDomainName.ReadOnly = true;
            panelDomainList.Visible = false;
            panelEditCreateDomain.Visible = true;
        }

        protected void repeaterDomains_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                int domainID = 0;
                int.TryParse(e.CommandArgument.ToString(), out domainID);

                if (domainID > 0)
                    ModifyDomain(domainID);
            }
        }

        #region Events
        void viewModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            alertmessage.SetMessage(errorID, message);
        }
        #endregion

        #region Button Click Events

        protected void btnAddDomain_Click(object sender, EventArgs e)
        {
            panelDomainList.Visible = false;
            panelEditCreateDomain.Visible = true;

            // Clear hidden field since we are creating a new domain
            hfDomainID.Value = string.Empty;

            // Clear domain field and checkbox field
            txtDomainName.ReadOnly = false;
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