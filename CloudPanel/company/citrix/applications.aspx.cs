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
                GetCitrixApps();
        }

        /// <summary>
        /// Gets the list of Citrix applications that the company has access to
        /// </summary>
        private void GetCitrixApps()
        {
           
        }
    }
}