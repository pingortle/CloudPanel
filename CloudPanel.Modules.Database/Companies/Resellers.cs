using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Database.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Database.Companies
{
    public class Resellers
    {
        /// <summary>
        /// Gets information about a particular reseller
        /// </summary>
        /// <param name="resellerCode"></param>
        /// <returns></returns>
        public static ResellerObject GetReseller(string resellerCode)
        {
            DB database = null;

            try
            {
                database = new DB();

                var resellers = (from r in database.Companies
                                where r.IsReseller
                                where r.CompanyCode == resellerCode
                                select new ResellerObject
                                {
                                    CompanyID = r.CompanyId,
                                    CompanyName = r.CompanyName,
                                    CompanyCode = r.CompanyCode,
                                    Street = r.Street,
                                    City = r.City,
                                    State = r.State,
                                    ZipCode = r.ZipCode,
                                    Country = r.Country,
                                    Telephone = r.PhoneNumber,
                                    AdminName = r.AdminName,
                                    AdminEmail = r.AdminEmail,
                                    DistinguishedName = r.DistinguishedName,
                                    Created = r.Created
                                }).FirstOrDefault();

                return resellers;
            }
            catch (Exception ex)
            {
                // TODO: LOG ERROR
                throw;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        /// <summary>
        /// Gets a list of resellers
        /// </summary>
        /// <returns></returns>
        public static List<ResellerObject> GetResellers()
        {
            DB database = null;

            try
            {
                database = new DB();

                var resellers = from r in database.Companies
                                 where r.IsReseller
                                 orderby r.CompanyName
                                 select new ResellerObject
                                 {
                                    CompanyID = r.CompanyId,
                                    CompanyName = r.CompanyName,
                                    CompanyCode = r.CompanyCode,
                                    Street = r.Street,
                                    City = r.City,
                                    State = r.State,
                                    ZipCode = r.ZipCode,
                                    Country = r.Country,
                                    Telephone = r.PhoneNumber,
                                    AdminName = r.AdminName,
                                    AdminEmail = r.AdminEmail,
                                    DistinguishedName = r.DistinguishedName,
                                    Created = r.Created
                                 };

                return resellers.ToList();
            }
            catch (Exception ex)
            {
                // TODO: LOG ERROR
                throw;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        /// <summary>
        /// Finds the total number of companies for a particular reseller
        /// </summary>
        /// <param name="resellersCompanyCode"></param>
        /// <returns></returns>
        public static int GetResellersCompanyCount(string resellersCompanyCode)
        {
            DB database = null;

            try
            {
                database = new DB();

                var companyCount = (from c in database.Companies
                                    where c.ResellerCode == resellersCompanyCode
                                    select c).Count();

                return companyCount;
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
