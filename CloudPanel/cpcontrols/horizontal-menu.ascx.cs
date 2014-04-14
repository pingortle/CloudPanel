using CloudPanel.Modules.Base.Users;
using CloudPanel.Modules.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.cpcontrols
{
    public partial class horizontal_menu : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            ProcessPlaceHolders();
        }

        private void ProcessPlaceHolders()
        {
            bool isSuperAdmin = WebSessionHandler.IsSuperAdmin;
            bool isResellerAdmin = WebSessionHandler.IsResellerAdmin;
            string selectedCompanyCode = WebSessionHandler.SelectedCompanyCode;

            PlaceHolderResellersMenu.Visible = isSuperAdmin;
            PlaceHolderSelectedCompanyMenu.Visible = !string.IsNullOrEmpty(selectedCompanyCode);
            PlaceHolderCompaniesMenu.Visible = (isSuperAdmin || isResellerAdmin) && !string.IsNullOrEmpty(selectedCompanyCode);

            if (!string.IsNullOrEmpty(selectedCompanyCode))
            {
                bool isExchangeEnabled = CloudPanel.Modules.Common.GlobalActions.CompanyChecks.IsExchangeEnabled(selectedCompanyCode);

                PlaceHolderExchangeDisabled.Visible = !isExchangeEnabled;
                PlaceHolderExchangeEnabled.Visible = isExchangeEnabled;
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            SearchViewModel viewModel = new SearchViewModel();

            UsersObject[] foundResults = viewModel.Search(txtSearch.Text);
            if (foundResults != null)
            {
                UsersObject found = foundResults[0];
                WebSessionHandler.SelectedResellerCode = found.ResellerCode;
                WebSessionHandler.SelectedCompanyCode = found.CompanyCode;
                WebSessionHandler.SelectedCompanyName = found.CompanyName;

                Session["CPSearchResultUser"] = found.UserPrincipalName;

                Server.Transfer("~/company/users.aspx");
            }
            else
                txtSearch.Text = "NO RESULTS FOUND";
        }
    }
}