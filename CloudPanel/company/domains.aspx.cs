using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Common.ViewModel;
using CloudPanel.Modules.Common.GlobalActions;
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

            // Do not show exchange if company is not enabled
            if (CompanyChecks.IsExchangeEnabled(WebSessionHandler.SelectedCompanyCode))
            {
                cbIsAcceptedDomain.Checked = domain.IsAcceptedDomain;
                divExchangeDomain.Visible = true;

                switch (domain.TypeOfDomain)
                {
                    case DomainType.AuthoritativeDomain:
                        cbAuthoritative.Checked = true;
                        break;
                    case DomainType.InternalRelayDomain:
                        cbInternalRelay.Checked = true;
                        break;
                    case DomainType.ExternalRelayDomain:
                        cbExternalRelay.Checked = true;
                        break;
                    default:
                        cbAuthoritative.Checked = true;
                        break;
                }
            }
            else
            {
                cbIsAcceptedDomain.Checked = false;
                divExchangeDomain.Visible = false;
            }

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
            else if (e.CommandName == "Delete")
            {
                // Delete domain
                DomainsViewModel viewModel = new DomainsViewModel();
                viewModel.ViewModelEvent += viewModel_ViewModelEvent;
                viewModel.DeleteDomain(e.CommandArgument.ToString(), WebSessionHandler.SelectedCompanyCode);

                // Audit
                AuditGlobal.AddAudit(WebSessionHandler.SelectedCompanyCode, HttpContext.Current.User.Identity.Name, ActionID.DeleteDomain, e.CommandArgument.ToString(), null);

                GetDomains();
            }
        }

        #region Events
        void viewModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            switch (errorID)
            {
                case Modules.Base.Enumerations.AlertID.DOMAIN_IN_USE:
                    alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.WARNING, Resources.LocalizedText.Domains_DomainInUse);
                    break;
                case Modules.Base.Enumerations.AlertID.DOMAIN_ALREADY_EXISTS:
                    alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.WARNING, Resources.LocalizedText.Domains_AlreadyExists);
                    break;
                default:
                    alertmessage.SetMessage(errorID, message);
                    break;
            }
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
            // Edit an existing domain
            string domain = txtDomainName.Text.Trim();
            bool isDefault = cbIsDefaultDomain.Checked;

            DomainsViewModel viewModel = new DomainsViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            if (string.IsNullOrEmpty(hfDomainID.Value))
            {
                if (cbIsAcceptedDomain.Checked && divExchangeDomain.Visible)
                    viewModel.AddDomain(domain, WebSessionHandler.SelectedCompanyCode, isDefault, true, GetAcceptedDomainType());
                else
                    viewModel.AddDomain(domain, WebSessionHandler.SelectedCompanyCode, isDefault, false, GetAcceptedDomainType());

                // Audit
                AuditGlobal.AddAudit(WebSessionHandler.SelectedCompanyCode, HttpContext.Current.User.Identity.Name, ActionID.AddDomain, domain, null);
            }
            else
            {
                if (cbIsAcceptedDomain.Checked && divExchangeDomain.Visible)
                    viewModel.UpdateDomain(domain, WebSessionHandler.SelectedCompanyCode, isDefault, cbIsAcceptedDomain.Checked, GetAcceptedDomainType());
                else
                    viewModel.UpdateDomain(domain, WebSessionHandler.SelectedCompanyCode, isDefault, false, GetAcceptedDomainType());

                // Audit
                AuditGlobal.AddAudit(WebSessionHandler.SelectedCompanyCode, HttpContext.Current.User.Identity.Name, ActionID.UpdateDomain, domain, null);
            }

            // Refresh domain list
            GetDomains();
        }

        private DomainType GetAcceptedDomainType()
        {
            if (!cbIsAcceptedDomain.Checked)
                return DomainType.BasicDomain;
            else if (cbAuthoritative.Checked)
                return DomainType.AuthoritativeDomain;
            else if (cbInternalRelay.Checked)
                return DomainType.InternalRelayDomain;
            else if (cbExternalRelay.Checked)
                return DomainType.ExternalRelayDomain;
            else
                return DomainType.Unknown;
            
        }

        #endregion
    }
}