using CloudPanel.Modules.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.company.email
{
    public partial class groups : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        private void GetAllUsersTest()
        {
            DB database = null;

            try
            {
                database = new DB();

                var users = (from u in database.Users
                             where !string.IsNullOrEmpty(u.Email)
                             select u).ToList().Take(10);

                repeaterOwners.DataSource = users;
                repeaterOwners.DataBind();

                repeaterMembers.DataSource = users;
                repeaterMembers.DataBind();

                ddlDeliveryManagement.DataTextField = "DisplayName";
                ddlDeliveryManagement.DataValueField = "UserPrincipalName";
                ddlDeliveryManagement.DataSource = users;
                ddlDeliveryManagement.DataBind();

                ddlGroupModerators.DataTextField = "DisplayName";
                ddlGroupModerators.DataValueField = "UserPrincipalName";
                ddlGroupModerators.DataSource = users;
                ddlGroupModerators.DataBind();

                ddlSendersDontRequireApproval.DataTextField = "DisplayName";
                ddlSendersDontRequireApproval.DataValueField = "UserPrincipalName";
                ddlSendersDontRequireApproval.DataSource = users;
                ddlSendersDontRequireApproval.DataBind();

                hfGroupMembers.Value = String.Join("||", users.Select(x => x.Email));
                hfGroupOwners.Value = String.Join("||", users.Select(x => x.Email));
            }
            catch (Exception ex)
            {
                alertmessage.SetMessage(Modules.Base.Enumerations.AlertType.ERROR, ex.ToString());
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        #region Button Click Events

        protected void btnAddGroup_Click(object sender, EventArgs e)
        {
            panelGroupList.Visible = false;
            panelEditGroup.Visible = true;

            // LAB
            GetAllUsersTest();
        }

        #endregion
    }
}