using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Base.Plans;
using CloudPanel.Modules.Common.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.plans
{
    public partial class company : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetAllPlans();
            }
        }

        protected void ddlCompanyPlans_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCompanyPlans.SelectedIndex > 0)
            {
                int value = int.Parse(ddlCompanyPlans.SelectedItem.Value);
                GetPlan(value);
            }
            else
                ClearValues();
        }

        private void GetPlan(int planID)
        {
            PlanCompanyViewModel viewModel = new PlanCompanyViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            CompanyPlanObject plan = viewModel.GetPlan(planID);
            if (plan != null)
            {
                hfSelectedPlanID.Value = plan.CompanyPlanID.ToString();
                txtDisplayName.Text = plan.CompanyPlanName;
                txtMaxUsers.Text = plan.MaxUser.ToString();
                txtMaxDomains.Text = plan.MaxDomains.ToString();
                txtMaxMailboxes.Text = plan.MaxExchangeMailboxes.ToString();
                txtMaxContacts.Text = plan.MaxExchangeContacts.ToString();
                txtMaxGroups.Text = plan.MaxExchangeDistributionGroups.ToString();
                txtMaxResourceMailboxes.Text = plan.MaxExchangeResourceMailboxes.ToString();
                txtMaxMailPublicFolders.Text = plan.MaxExchangeMailPublicFolders.ToString();
            }
        }

        private void GetAllPlans()
        {
            PlanCompanyViewModel viewModel = new PlanCompanyViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            List<CompanyPlanObject> plans = viewModel.GetAllPlans();
            if (plans != null)
            {
                ddlCompanyPlans.Items.Clear();
                ddlCompanyPlans.Items.Add(
                    new ListItem()
                    {
                        Value = "0",
                        Text = "--- Add New ---"
                    });

                foreach (CompanyPlanObject p in plans)
                {
                    ListItem item = new ListItem();
                    item.Value = p.CompanyPlanID.ToString();
                    item.Text = p.CompanyPlanName;
                    ddlCompanyPlans.Items.Add(item);
                }
            }

            ClearValues();
        }

        private void ClearValues()
        {
            hfSelectedPlanID.Value = string.Empty;

            if (ddlCompanyPlans.Items.Count > 0)
                ddlCompanyPlans.SelectedIndex = 0;

            foreach (Control ctrl in panelPlan.Controls)
            {
                if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
            }
        }

        private void UpdatePlan(int? planID)
        {
            CompanyPlanObject newPlan = new CompanyPlanObject();
            newPlan.CompanyPlanName = txtDisplayName.Text;

            int maxUsers = 1;
            int.TryParse(txtMaxUsers.Text, out maxUsers);
            newPlan.MaxUser = maxUsers;

            int maxDomains = 1;
            int.TryParse(txtMaxDomains.Text, out maxDomains);
            newPlan.MaxDomains = maxDomains;

            int maxMailboxes = 0;
            int.TryParse(txtMaxMailboxes.Text, out maxMailboxes);
            newPlan.MaxExchangeMailboxes = maxMailboxes;

            int maxContacts = 0;
            int.TryParse(txtMaxContacts.Text, out maxContacts);
            newPlan.MaxExchangeContacts = maxContacts;

            int maxGroups = 0;
            int.TryParse(txtMaxGroups.Text, out maxGroups);
            newPlan.MaxExchangeDistributionGroups = maxGroups;

            int maxResourceMailboxes = 0;
            int.TryParse(txtMaxResourceMailboxes.Text, out maxResourceMailboxes);
            newPlan.MaxExchangeResourceMailboxes = maxResourceMailboxes;

            int maxEmailPublicFolders = 0;
            int.TryParse(txtMaxMailPublicFolders.Text, out maxEmailPublicFolders);
            newPlan.MaxExchangeMailPublicFolders = maxEmailPublicFolders;

            // Update or Create
            PlanCompanyViewModel viewModel = new PlanCompanyViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            if (planID == null)
            {
                viewModel.Create(newPlan);
            }
            else
            {
                newPlan.CompanyPlanID = (int)planID;
                viewModel.Update(newPlan);
            }

            // Refresh
            GetAllPlans();
        }

        private void DeletePlan(int planID)
        {
            PlanCompanyViewModel viewModel = new PlanCompanyViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            viewModel.Delete(planID);

            GetAllPlans();
        }

        #region Events

        void viewModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
            switch (errorID)
            {
                case Modules.Base.Enumerations.AlertID.PLAN_IN_USE:
                    alertmessage.SetMessage(AlertID.WARNING, Resources.LocalizedText.PlanCompany_PlanInUse);
                    break;
                default:
                    alertmessage.SetMessage(errorID, message);
                    break;
            }
        }

        #endregion

        #region Button Click Events

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfSelectedPlanID.Value))
            {
                int planID = 0;
                int.TryParse(hfSelectedPlanID.Value, out planID);

                if (planID > 0)
                    DeletePlan(planID);
                else
                    GetAllPlans();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(hfSelectedPlanID.Value))
                UpdatePlan(null);
            else
            {
                int planID = 0;
                int.TryParse(hfSelectedPlanID.Value, out planID);

                if (planID > 0)
                    UpdatePlan(planID);
                else
                    alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.FAILED, Resources.LocalizedText.PlanCompany_UpdatePlanNotFound);
            }
        }

        #endregion
    }
}