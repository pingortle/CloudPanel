using CloudPanel.Modules.ActiveDirectory.OrganizationalUnits;
using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class UsersViewModel
    {
        public delegate void UserEventHandler(ErrorID errorID, string message);
        public event UserEventHandler UserEvent;

        public void CreateNewUser(CompanyObject company)
        {
            ADOrganizationalUnit org = null;

            try
            {
                org = new ADOrganizationalUnit("username", "password", "domaincontroller");

                if (string.IsNullOrEmpty(company.CompanyName))
                    UserEvent(ErrorID.WARNING, "User's firstname must be supplied.");
                else
                {
                    org.CreateReseller(null, null);
                    //transaction.Add(Enum.CreateReselleROU, company);

                    transaction.Add(() => CreateReseller(null, null), () => DeleteReseller(null, null));

                    // ENTITY INSERT INTO DATABASE
                    // database.Insert(userobject);
                    // transaction.Add(Enum.AddSQL);

                    // GOOD T OGO
                    if (UserEvent != null)
                        UserEvent(ErrorID.SUCCESS, "Successfully created new user.");
                }
            }
            catch (Exception ex)
            {
                if (UserEvent != null)
                    UserEvent(ErrorID.WARNING, ex.Message);

                //transcation.RollBack();
            }
        }
    }
}
