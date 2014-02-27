using CloudPanel.Modules.ActiveDirectory.OrganizationalUnits;
using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Common.Codes;
using CloudPanel.Modules.Common.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Resellers.Create
{
    public class NewReseller
    {
        public static void Create(ResellerObject reseller)
        {
            ADOrganizationalUnit org = new ADOrganizationalUnit(StaticSettings.Username, StaticSettings.Password, StaticSettings.PrimaryDC);

            try
            {
            }
            catch (Exception ex)
            {

            }
        }
    }
}
