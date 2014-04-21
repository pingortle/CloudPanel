using CloudPanel.Modules.Common.GlobalActions;
using CloudPanel.Modules.Common.Other;
using CloudPanel.Modules.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.company.email
{
    public partial class disable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                lbRandomCharacters.Text = Randoms.LettersAndNumbers();
        }

        private void CheckCompany()
        {
            bool isEnabled = CompanyChecks.IsExchangeEnabled(WebSessionHandler.SelectedCompanyCode);
            if (!isEnabled)
            {
                btnDisableExchange.Enabled = false;
                Response.Redirect("~/company/email/enable.aspx");
            }
            else
                btnDisableExchange.Enabled = true;
        }

        private void DisableExchange()
        {
            ExchangeDisableViewModel viewModel = new ExchangeDisableViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;
            viewModel.DisableExchange(WebSessionHandler.SelectedCompanyCode);

            // Check company and see if we need to redirect after disabling Exchange
            CheckCompany();
        }

        void viewModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            alertmessage.SetMessage(errorID, message);
        }

        protected void btnDisableExchange_Click(object sender, EventArgs e)
        {
            if (txtRandomCharacters.Text != lbRandomCharacters.Text)
            {
                alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.WARNING, "You did not supply the correct code. Please try again.");
                lbRandomCharacters.Text = Randoms.LettersAndNumbers();
            }
            else
                DisableExchange();
        }
    }
}