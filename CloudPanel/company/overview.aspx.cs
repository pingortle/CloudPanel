using CloudPanel.Modules.Base.Auditing;
using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Base.Plans;
using CloudPanel.Modules.Base.Statistics;
using CloudPanel.Modules.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.company
{
    public partial class overview : System.Web.UI.Page
    {
        public string Address;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadCompany();
            }
        }

        private void LoadCompany()
        {
            PopulateCompanyPlans();

            PopulateCompanyDetails();

            PopulateRecentActions();
        }

        private void PopulateCompanyPlans()
        {
            CompanyOverviewViewModel viewModel = new CompanyOverviewViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            List<CompanyPlanObject> companyPlans = viewModel.GetCompanyPlans(WebSessionHandler.SelectedCompanyCode);
            if (companyPlans != null)
            {
                ddlCurrentPlan.Items.Clear();
                ddlCurrentPlan.Items.Add(new ListItem()
                    {
                        Value = "0",
                        Text = "--- Plan Not Set ---"
                    });

                foreach (CompanyPlanObject o in companyPlans)
                {
                    ListItem item = new ListItem();
                    item.Value = o.CompanyPlanID.ToString();
                    item.Text = o.CompanyPlanName;
                    ddlCurrentPlan.Items.Add(item);
                }

            }
        }

        private void PopulateCompanyDetails()
        {
            CompanyOverviewViewModel viewModel = new CompanyOverviewViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            CompanyObject company = viewModel.GetCompany(WebSessionHandler.SelectedCompanyCode);
            if (company != null)
            {
                lbCompanyName.Text = company.CompanyName;
                lbCompanyCode.Text = company.CompanyCode;
                lbContactsName.Text = company.AdminName;
                lbTelephone.Text = company.Telephone;
                lbWhenCreated.Text = company.Created.ToString();

                Address = string.Format("{0}, {1}, {2} {3}", company.Street, company.City, company.State, company.ZipCode);

                // Set the plan
                ListItem item = ddlCurrentPlan.Items.FindByValue(company.CompanyPlanID.ToString());
                if (item != null)
                    ddlCurrentPlan.SelectedValue = item.Value;
                else
                {
                    alertmessage.SetMessage(AlertID.WARNING, Resources.LocalizedText.Overview_PlanNotSetWarning);
                    ddlCurrentPlan.SelectedIndex = 0;
                }

                if (company.CompanyPlanObject != null)
                {
                    // Get product details
                    OverallStats statistics = viewModel.GetCompanyStats(WebSessionHandler.SelectedCompanyCode);
                    if (statistics != null)
                    {
                        lbUsers.Text = string.Format("({0} / {1})", statistics.TotalUsers, company.CompanyPlanObject.MaxUser);
                        progBarUsers.Style.Add("width", string.Format("{0}%", (int)Math.Round((double)(100 * statistics.TotalUsers) / company.CompanyPlanObject.MaxUser)));

                        lbDomains.Text = string.Format("({0} / {1})", statistics.TotalDomains, company.CompanyPlanObject.MaxDomains);
                        progBarDomains.Style.Add("width", string.Format("{0}%", (int)Math.Round((double)(100 * statistics.TotalDomains) / company.CompanyPlanObject.MaxDomains)));

                        lbTotalMailboxes.Text = string.Format("({0} / {1})", statistics.TotalMailboxes, company.CompanyPlanObject.MaxExchangeMailboxes);
                        progBarMailboxes.Style.Add("width", string.Format("{0}%", (int)Math.Round((double)(100 * statistics.TotalMailboxes) / company.CompanyPlanObject.MaxExchangeMailboxes)));
                    }
                }
            }
        }

        private void PopulateRecentActions()
        {
            var audits = AuditGlobalization.GetAuditing(WebSessionHandler.SelectedCompanyCode);

            if (audits != null)
            {
                // Select only the top 10
                IEnumerable<Audits> top10Audits = audits.Take(10);

                repeaterAudits.DataSource = top10Audits;
                repeaterAudits.DataBind();
            }
        }

        void viewModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            alertmessage.SetMessage(errorID, message);
        }

        protected void ddlCurrentPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCurrentPlan.SelectedIndex > 0)
            {
                int planID = 0;
                int.TryParse(ddlCurrentPlan.SelectedItem.Value, out planID);

                if (planID > 0)
                {
                    CompanyOverviewViewModel viewModel = new CompanyOverviewViewModel();
                    viewModel.ViewModelEvent += viewModel_ViewModelEvent;
                    viewModel.UpdateCompanyPlan(WebSessionHandler.SelectedCompanyCode, planID);

                    // Load the company again
                    LoadCompany();
                }
            }
        }
    }
}