using CloudPanel.Modules.Base.Dashboard;
using CloudPanel.Modules.Database.Statistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetEnvironmentOverview();
            }
        }

        private void GetEnvironmentOverview()
        {
            OverviewStats stats = DashboardStats.GetDashboardStatistics();
            lbTotalUsers.Text = stats.TotalUsers.ToString();
            lbTotalResellers.Text = stats.Resellers.ToString();
            lbTotalCompanies.Text = stats.Companies.ToString();

            // Percent
            lbTotalMailboxes.Text = string.Format("({0})", stats.Mailboxes);
            progBarMailboxes.Style.Add("width", string.Format("{0}%", stats.MailboxPercent));

            lbTotalCitrixUsers.Text = string.Format("({0})", stats.CitrixUsers);
            progBarCitrix.Style.Add("width", string.Format("{0}%", stats.CitrixPercent));

            lbTotalLyncUsers.Text = string.Format("({0})", stats.LyncUsers);
            progBarLync.Style.Add("width", string.Format("{0}%", stats.LyncPercent));
        }
    }
}