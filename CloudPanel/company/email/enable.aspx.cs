using CloudPanel.Modules.Common.GlobalActions;
using CloudPanel.Modules.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.company.email
{
    public partial class enable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                CheckCompany();
        }

        private void CheckCompany()
        {
            bool isEnabled = CompanyChecks.IsExchangeEnabled(WebSessionHandler.SelectedCompanyCode);
            if (isEnabled)
            {
                Response.Redirect("~/company/email/disable.aspx");
            }
        }

        protected void btnEnableExchange_Click(object sender, EventArgs e)
        {
            ExchangeEnableViewModel viewModel = new ExchangeEnableViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;
            viewModel.EnableExchange(WebSessionHandler.SelectedCompanyCode);

            // Check the company to see if it was updated
            CheckCompany();
        }

        void viewModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            alertmessage.SetMessage(errorID, message);
        }
    }
}