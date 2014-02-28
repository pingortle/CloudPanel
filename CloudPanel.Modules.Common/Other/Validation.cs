using CloudPanel.Modules.Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Common.Other
{
    public class Validation
    {
        /// <summary>
        /// Checks if a company code exists in the database (reseller or company)
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public static bool DoesCompanyCodeExist(string companyCode)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var exists = (from c in database.Companies
                              where c.CompanyCode == companyCode
                              select c.CompanyCode).FirstOrDefault();

                if (!string.IsNullOrEmpty(exists))
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }
    }
}
