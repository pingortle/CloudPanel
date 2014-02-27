using CloudPanel.Modules.ActiveDirectory.OrganizationalUnits;
using CloudPanel.Modules.Base.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class Resellers
    {
        public void CreateNewReseller(CompanyObject newReseller)
        {
            ADOrganizationalUnit org = null;

            try
            {
                org = new ADOrganizationalUnit("username", "password", "domain controller");

                //string distinguishedName = org.CreateOU(newReseller);
                //org.CreateSecurityGroup(newReseller.CompanyCode, distinguishedName);

                // InsertResellerIntoSQL(newReseller);
            }
            catch (Exception ex)
            {
                // WHAT DO I RETURN?
            }
        }
    }
}
