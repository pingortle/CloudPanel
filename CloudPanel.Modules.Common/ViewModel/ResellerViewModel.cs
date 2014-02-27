using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Common.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class ResellerViewModel : IViewModel
    {
        /// <summary>
        /// Gets a list of resellers from the database
        /// </summary>
        /// <returns></returns>
        public List<ResellerObject> GetResellers()
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var resellersDb = from r in database.Companies
                                  where r.IsReseller
                                  orderby r.CompanyName
                                  select r;

                List<ResellerObject> resellers = new List<ResellerObject>();

                foreach (var reseller in resellersDb)
                {
                    ResellerObject tmp = new ResellerObject();
                    tmp.CompanyID = reseller.CompanyId;
                    tmp.CompanyName = reseller.CompanyName;
                    tmp.CompanyCode = reseller.CompanyCode;
                    tmp.Street = reseller.Street;
                    tmp.City = reseller.City;
                    tmp.State = reseller.State;
                    tmp.ZipCode = reseller.ZipCode;
                    tmp.Country = reseller.Country;
                    tmp.Telephone = reseller.PhoneNumber;
                    tmp.Description = reseller.Description;
                    tmp.AdminName = reseller.AdminName;
                    tmp.AdminEmail = reseller.AdminEmail;
                    tmp.DistinguishedName = reseller.DistinguishedName;
                    tmp.Created = reseller.Created;

                    resellers.Add(tmp);
                }

                return resellers;
            }
            catch (Exception ex)
            {
                ThrowEvent(AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        /// <summary>
        /// Gets a specific reseller
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public ResellerObject GetReseller(string companyCode)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var reseller = (from r in database.Companies
                                where r.IsReseller
                                orderby r.CompanyName
                                select r).FirstOrDefault();

                ResellerObject tmp = new ResellerObject();
                tmp.CompanyID = reseller.CompanyId;
                tmp.CompanyName = reseller.CompanyName;
                tmp.CompanyCode = reseller.CompanyCode;
                tmp.Street = reseller.Street;
                tmp.City = reseller.City;
                tmp.State = reseller.State;
                tmp.ZipCode = reseller.ZipCode;
                tmp.Country = reseller.Country;
                tmp.Telephone = reseller.PhoneNumber;
                tmp.Description = reseller.Description;
                tmp.AdminName = reseller.AdminName;
                tmp.AdminEmail = reseller.AdminEmail;
                tmp.DistinguishedName = reseller.DistinguishedName;
                tmp.Created = reseller.Created;

                return tmp;
            }
            catch (Exception ex)
            {
                ThrowEvent(AlertID.FAILED, ex.Message);
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
