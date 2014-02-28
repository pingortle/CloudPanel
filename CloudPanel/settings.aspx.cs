using CloudPanel.Modules.Base.Security;
using CloudPanel.Modules.Common.Settings;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel
{
    public partial class settings : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Get currencies in the system
                PopulateCurrencies();

                // Get current settings
                PopulateSettings();
            }
        }

        /// <summary>
        /// Populates the list of currencies
        /// </summary>
        private void PopulateCurrencies()
        {
            ddlCurrencySymbol.Items.Clear();

            foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
            {
                if (!ci.IsNeutralCulture)
                {
                    RegionInfo region = new RegionInfo(ci.LCID);

                    ListItem item = new ListItem();
                    item.Value = region.CurrencySymbol;
                    item.Text = string.Format("{0} [{1}]", region.CurrencyNativeName, region.CurrencySymbol);

                    ddlCurrencySymbol.Items.Add(item);
                }
            }

            // Select the current culture
            RegionInfo currentRegion = new RegionInfo(CultureInfo.CurrentCulture.LCID);
            if (currentRegion != null)
            {
                ListItem item = ddlCurrencySymbol.Items.FindByValue(currentRegion.CurrencySymbol);
                if (item != null)
                    ddlCurrencySymbol.SelectedValue = item.Value;
            }
        }

        /// <summary>
        /// Populates a list of settings
        /// </summary>
        private void PopulateSettings()
        {
            try
            {
                // Retrieve updated settings
                StaticSettings.GetSettings(ConfigurationManager.AppSettings["Key"]);

                #region General
                
                txtCompanyName.Text = StaticSettings.HostersName;
                cbEnableResellers.Checked = StaticSettings.ResellersEnabled;

                #endregion

                #region Active Directory

                txtHostingOU.Text = StaticSettings.HostingOU;
                txtUsersOU.Text = StaticSettings.UsersOU;
                txtDomainController.Text = StaticSettings.PrimaryDC;
                txtUsername.Text = StaticSettings.Username;
                txtPassword.Text = StaticSettings.Password;

                #endregion

                #region Security Groups

                txtSuperAdmins.Text = StaticSettings.SuperAdmins;
                txtBillingAdmins.Text = StaticSettings.BillingAdmins;

                #endregion

                #region Exchange

                ListItem item = ddlExchConnectionType.Items.FindByValue(StaticSettings.ExchangeConnectionType);
                if (item != null)
                    ddlExchConnectionType.SelectedValue = item.Value;
                else
                    ddlExchConnectionType.SelectedValue = "Basic";

                ListItem item2 = ddlExchVersion.Items.FindByValue(StaticSettings.ExchangeVersion.ToString());
                if (item2 != null)
                    ddlExchVersion.SelectedValue = item2.Value;
                else
                    ddlExchVersion.SelectedValue = "2013";

                txtExchServer.Text = StaticSettings.ExchangeServer;
                txtExchPFServer.Text = StaticSettings.ExchangePublicFolderServer;
                txtExchDatabases.Text = StaticSettings.ExchangeDatabases;

                cbExchPFEnabled.Checked = StaticSettings.PublicFoldersEnabled;
                cbExchSSLEnabled.Checked = StaticSettings.ExchangeSSLEnabled;

                #endregion

                #region Modules

                cbEnableExchange.Checked = true;
                cbEnableCitrix.Checked = StaticSettings.CitrixEnabled;
                cbEnableLync.Checked = StaticSettings.LyncEnabled;

                #endregion

                #region Notifications

                #endregion

                #region Other

                cbAdvOnlySuperAdmins.Checked = StaticSettings.LockdownEnabled;
                cbAdvCustomNameAttrib.Checked = StaticSettings.AllowCustomNameAttribute;
                cbAdvIPBlockingEnabled.Checked = StaticSettings.IPBlockingEnabled;

                txtAdvIPFailedCount.Text = StaticSettings.IPBlockingFailedCount.ToString();
                txtAdvIPBlockingLockout.Text = StaticSettings.IPBlockingLockedInMinutes.ToString();

                #endregion
            }
            catch (Exception ex)
            {
                alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.FAILED, "Failed to load settings: " + ex.ToString());
            }
        }

        /// <summary>
        /// Saves the new settings to the database
        /// </summary>
        private void SaveSettings()
        {
            try
            {
                #region General

                StaticSettings.HostersName = txtCompanyName.Text;
                StaticSettings.ResellersEnabled = cbEnableResellers.Checked;

                #endregion

                #region Active Directory

                StaticSettings.HostingOU = txtHostingOU.Text;
                StaticSettings.UsersOU = txtUsersOU.Text;
                StaticSettings.PrimaryDC = txtDomainController.Text;
                StaticSettings.Username = txtUsername.Text;
                StaticSettings.Password = DataProtection.Encrypt(txtPassword.Text, ConfigurationManager.AppSettings["Key"]);

                #endregion

                #region Security Groups

                StaticSettings.SuperAdmins = txtSuperAdmins.Text;
                StaticSettings.BillingAdmins = txtBillingAdmins.Text;

                #endregion

                #region Billing


                #endregion

                #region Exchange

                StaticSettings.ExchangeConnectionType = ddlExchConnectionType.SelectedValue;
                StaticSettings.ExchangeVersion = int.Parse(ddlExchVersion.SelectedValue);
                StaticSettings.ExchangeServer = txtExchServer.Text;
                StaticSettings.ExchangePublicFolderServer = txtExchPFServer.Text;
                StaticSettings.ExchangeDatabases = txtExchDatabases.Text;
                StaticSettings.PublicFoldersEnabled = cbExchPFEnabled.Checked;
                StaticSettings.ExchangeSSLEnabled = cbExchSSLEnabled.Checked;

                #endregion

                #region Modules

                StaticSettings.CitrixEnabled = cbEnableCitrix.Checked;
                StaticSettings.LyncEnabled = cbEnableLync.Checked;

                #endregion

                #region Other

                StaticSettings.LockdownEnabled = cbAdvOnlySuperAdmins.Checked;
                StaticSettings.AllowCustomNameAttribute = cbAdvCustomNameAttrib.Checked;
                StaticSettings.IPBlockingEnabled = cbAdvIPBlockingEnabled.Checked;

                int ipBlockFailedCount = 0;
                int.TryParse(txtAdvIPFailedCount.Text, out ipBlockFailedCount);
                StaticSettings.IPBlockingFailedCount = ipBlockFailedCount;

                int ipBlockLockout = 0;
                int.TryParse(txtAdvIPBlockingLockout.Text, out ipBlockLockout);
                StaticSettings.IPBlockingLockedInMinutes = ipBlockLockout;

                #endregion

                // Commit changes
                StaticSettings.CommitSettings(ConfigurationManager.AppSettings["Key"]);

                // Show Success
                alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.SUCCESS, "Successfully saved settings");
            }
            catch (Exception ex)
            {
                alertmessage.SetMessage(Modules.Base.Enumerations.AlertID.FAILED, ex.Message);
            }
        }

        #region Button Clicks

        protected void btnSubmit_Click(object sender, EventArgs e)
        {            
            SaveSettings();
        }

        #endregion
    }
}