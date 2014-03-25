using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Base.Users;
using CloudPanel.Modules.Common.Database;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class SearchViewModel : IViewModel
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UsersObject[] Search(string searchResult)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                // Compile a list of companies
                var companies = from c in database.Companies
                                select c;

                // Compile a list of users
                var users = from u in database.Users
                            from c in database.Companies.Where(cc => cc.CompanyCode == u.CompanyCode).DefaultIfEmpty()
                            where u.UserPrincipalName.Contains(searchResult) || u.Firstname.Contains(searchResult) || u.Lastname.Contains(searchResult)
                            select new UsersObject
                            {
                                UserPrincipalName = u.UserPrincipalName,
                                Firstname = u.Firstname,
                                Lastname = u.Lastname,
                                CompanyCode = u.CompanyCode,
                                CompanyName = c.CompanyName,
                                ResellerCode = c.ResellerCode
                            };

                if (users != null)
                    return users.ToArray();
                else
                    return null;
            }
            catch (Exception ex)
            {
                ThrowEvent(AlertID.FAILED, ex.Message);
                this.logger.Error("Error searching for " + searchResult, ex);

                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }
    }
}
