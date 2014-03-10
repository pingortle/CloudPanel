using CloudPanel.Modules.Base.Citrix;
using CloudPanel.Modules.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.company.citrix
{
    public partial class applications : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                GetCitrixApplications();
        }

        /// <summary>
        /// Gets the list of Citrix applications that the company has access to
        /// </summary>
        private void GetCitrixApplications()
        {
            CompanyCitrixViewModel viewModel = new CompanyCitrixViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            List<ApplicationsObject> foundApplications = viewModel.GetCitrixApplications(WebSessionHandler.SelectedCompanyCode);
            if (foundApplications != null)
            {
                var apps = from a in foundApplications where !a.IsServer select a;
                var svrs = from s in foundApplications where s.IsServer select s;

                if (apps != null)
                {
                    repeaterApps.DataSource = apps;
                    repeaterApps.DataBind();
                }

                if (svrs != null)
                {
                    repeaterServers.DataSource = svrs;
                    repeaterServers.DataBind();
                }
            }
        }

        void viewModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            alertmessage.SetMessage(errorID, message);
        }
    }
}