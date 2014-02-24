using CloudPanel.Modules.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.company.email
{
    public partial class mailboxes : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            UserTest();
        }

        private void UserTest()
        {
            List<CloudPanel.Modules.Database.Entity.User> users = new List<Modules.Database.Entity.User>();

            for (int i = 0; i < 2000; i++)
            {
                CloudPanel.Modules.Database.Entity.User tmp = new User();
                tmp.DisplayName = "Jacob Dixon";
                tmp.Firstname = "Jacob";
                tmp.Lastname = "Dixon";
                tmp.UserPrincipalName = "jdixon@compsysar.com";

                users.Add(tmp);
            }
            

            repeaterNonMailboxes.DataSource = users;
            repeaterNonMailboxes.DataBind();
        }
    }
}