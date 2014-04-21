using CloudPanel.Modules.Base.Companies;
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
    public partial class mailbox : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RePopulate();
            }
        }

        private void RePopulate()
        {
            ClearAll();
            GetPlans();
            GetCompanies();
        }

        private void GetPlans()
        {
            PlanMailboxViewModel viewModel = new PlanMailboxViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            List<MailboxPlanObject> plans = viewModel.GetPlans();
            ddlMailboxPlans.Items.Add("--- Create New ---");

            if (plans != null && plans.Count > 0)
            {
                foreach (MailboxPlanObject obj in plans)
                {
                    ListItem item = new ListItem();
                    item.Value = obj.MailboxPlanID.ToString();
                    item.Text = obj.MailboxPlanName;
                    ddlMailboxPlans.Items.Add(item);
                }
            }
        }

        private void GetCompanies()
        {
            PlanMailboxViewModel viewModel = new PlanMailboxViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            List<CompanyObject> companies = viewModel.GetCompanies();
            ddlCompanies.Items.Add("--- Select a Company ---");

            if (companies != null && companies.Count > 0)
            {
                foreach (CompanyObject obj in companies)
                {
                    ListItem item = new ListItem();
                    item.Value = obj.CompanyCode;
                    item.Text = "[" + obj.CompanyCode + "] " + obj.CompanyName;
                    ddlCompanies.Items.Add(item);
                }
            }
        }

        private void ClearAll()
        {
            foreach (Control ctrl in panelPlan.Controls)
            {
                if (ctrl is DropDownList)
                    ((DropDownList)ctrl).Items.Clear();
                else if (ctrl is TextBox)
                    ((TextBox)ctrl).Text = string.Empty;
                else if (ctrl is CheckBox)
                    ((CheckBox)ctrl).Checked = false;
            }
        }

        protected void ddlMailboxPlans_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMailboxPlans.SelectedIndex > 0)
            {
                PlanMailboxViewModel viewModel = new PlanMailboxViewModel();
                viewModel.ViewModelEvent += viewModel_ViewModelEvent;

                MailboxPlanObject obj = viewModel.GetPlan(int.Parse(ddlMailboxPlans.SelectedItem.Value));
                if (obj != null && obj.MailboxPlanID > 0)
                {
                    ListItem item = ddlCompanies.Items.FindByValue(obj.CompanyCode);
                    if (item != null)
                        ddlCompanies.SelectedValue = item.Value;
                    else
                        ddlCompanies.SelectedIndex = 0;

                    txtDisplayName.Text = obj.MailboxPlanName;
                    txtDescription.Text = obj.MailboxPlanDescription;
                    txtMaxRecipients.Text = obj.MaxRecipients.ToString();
                    txtMaxKeepDeletedItemsInDays.Text = obj.MaxKeepDeletedItemsInDays.ToString();
                    txtMinMailboxSize.Text = obj.MailboxSizeInMB.ToString();
                    txtMaxMailboxSize.Text = obj.MaxMailboxSizeInMB.ToString();
                    txtMaxSendSize.Text = obj.MaxSendInKB.ToString();
                    txtMaxReceiveSize.Text = obj.MaxReceiveInKB.ToString();

                    cbEnablePOP3.Checked = obj.EnablePOP3;
                    cbEnableIMAP.Checked = obj.EnableIMAP;
                    cbEnableOWA.Checked = obj.EnableOWA;
                    cbEnableMAPI.Checked = obj.EnableMAPI;
                    cbEnableAS.Checked = obj.EnableAS;
                    cbEnableECP.Checked = obj.EnableECP;

                    txtCostPerMailbox.Text = obj.Cost;
                    txtPricePerMailbox.Text = obj.Price;
                    txtPricePerGB.Text = obj.AdditionalGBPrice;
                }
                else
                    alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.FAILED, "Unable to find plan");
            }
            else
            {
                RePopulate();
            }
        }

        #region Events

        void viewModel_ViewModelEvent(Modules.Base.Enumerations.AlertID errorID, string message)
        {
           alertmessage.SetMessage(errorID, message);
        }

        #endregion

        #region Button Clicks

        protected void btnSave_Click(object sender, EventArgs e)
        {
            PlanMailboxViewModel viewModel = new PlanMailboxViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            MailboxPlanObject obj = new MailboxPlanObject();
            obj.MailboxPlanName = txtDisplayName.Text;
            obj.MailboxPlanDescription = txtDescription.Text;
            obj.MaxRecipients = int.Parse(txtMaxRecipients.Text);
            obj.MaxKeepDeletedItemsInDays = int.Parse(txtMaxKeepDeletedItemsInDays.Text);
            obj.MailboxSizeInMB = int.Parse(txtMinMailboxSize.Text);
            obj.MaxMailboxSizeInMB = int.Parse(txtMaxMailboxSize.Text);
            obj.MaxSendInKB = int.Parse(txtMaxSendSize.Text);
            obj.MaxReceiveInKB = int.Parse(txtMaxReceiveSize.Text);
            obj.EnablePOP3 = cbEnablePOP3.Checked;
            obj.EnableIMAP = cbEnableIMAP.Checked;
            obj.EnableOWA = cbEnableOWA.Checked;
            obj.EnableMAPI = cbEnableMAPI.Checked;
            obj.EnableAS = cbEnableAS.Checked;
            obj.EnableECP = cbEnableECP.Checked;
            obj.Cost = txtCostPerMailbox.Text;
            obj.Price = txtPricePerMailbox.Text;
            obj.AdditionalGBPrice = txtPricePerGB.Text;

            if (ddlCompanies.SelectedIndex > 0)
                obj.CompanyCode = ddlCompanies.SelectedItem.Value;
            else
                obj.CompanyCode = string.Empty;

            bool success = false;
            if (ddlMailboxPlans.SelectedIndex > 0)
            {
                obj.MailboxPlanID = int.Parse(ddlMailboxPlans.SelectedItem.Value);
                success = viewModel.UpdatePlan(obj);
            }
            else
                success = viewModel.CreatePlan(obj);

            if (success)
                alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.SUCCESS, "Successfully saved mailbox plan");

            RePopulate();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            PlanMailboxViewModel viewModel = new PlanMailboxViewModel();
            viewModel.ViewModelEvent += viewModel_ViewModelEvent;

            if (ddlMailboxPlans.SelectedIndex > 0)
            {
                int planId = int.Parse(ddlMailboxPlans.SelectedItem.Value);
                viewModel.DeletePlan(planId);
            }

            RePopulate();
        }

        #endregion
    }
}