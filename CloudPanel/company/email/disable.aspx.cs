using CloudPanel.Modules.Common.Other;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CloudPanel.company.email
{
    public partial class disable : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lbRandomCharacters.Text = Randoms.LettersAndNumbers();
        }
    }
}