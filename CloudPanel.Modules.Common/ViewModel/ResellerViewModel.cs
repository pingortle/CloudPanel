using CloudPanel.Modules.ActiveDirectory.Groups;
using CloudPanel.Modules.ActiveDirectory.OrganizationalUnits;
using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Common.Database;
using CloudPanel.Modules.Common.Other;
using CloudPanel.Modules.Common.Settings;
using System.Data.Entity;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using CloudPanel.Modules.Common.Rollback;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class ResellerViewModel : IViewModel
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

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

                if (resellersDb != null)
                {
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
                                where r.CompanyCode == companyCode
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

        /// <summary>
        /// Creates a new reseller
        /// </summary>
        /// <param name="reseller"></param>
        /// <param name="baseOrganizationalUnit"></param>
        public void NewReseller(ResellerObject reseller, string baseOrganizationalUnit)
        {
            CPDatabase database = null;

            // Rollback class in case something goes wrong we can undo changes
            CloudPanelTransaction events = new CloudPanelTransaction();

            try
            {
                database = new CPDatabase();

                // Generate the company code
                string companyCode = ResellerObject.GenerateCompanyCode(reseller.CompanyName);
                
                // Loop and make sure one does exist or find one that does
                int count = 0;
                for (int i = 0; i < 1000; i++)
                {
                    if (Validation.DoesCompanyCodeExist(companyCode))
                    {
                        this.logger.Info("Tried to create a new reseller with company code " + companyCode + " but it already existed... trying another code");

                        companyCode = companyCode + count.ToString();
                        count++;
                    }
                    else
                    {
                        reseller.CompanyCode = companyCode; // Assign company code to object
                        break;
                    }
                }

                // Create the reseller in Active Directory
                ADOrganizationalUnit adOrg = new ADOrganizationalUnit(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                string distinguishedName = adOrg.CreateReseller(StaticSettings.HostingOU, reseller);
                events.NewOrganizationalUnitEvent(distinguishedName);

                // Create the security group
                ADGroup adGroup = new ADGroup(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                adGroup.Create(distinguishedName, "GPOAccess@" + companyCode, "", true, false);
                events.NewSecurityGroup("GPOAccess@" + companyCode);

                // Add the new group to the GPOAccess@Hosting group for group policy permissions
                adGroup.AddMember("GPOAccess@Hosting", "GPOAccess@" + companyCode, "name");

                // Add access rights
                adOrg.AddAccessRights(distinguishedName, "GPOAccess@" + companyCode);

                // Insert into database
                Company company = new Company();
                company.IsReseller = true;
                company.CompanyName = reseller.CompanyName;
                company.CompanyCode = companyCode;
                company.Street = reseller.Street;
                company.City = reseller.City;
                company.State = reseller.State;
                company.ZipCode = reseller.ZipCode;
                company.Country = reseller.Country;
                company.PhoneNumber = reseller.Telephone;
                company.AdminName = reseller.AdminName;
                company.AdminEmail = reseller.AdminEmail;
                company.Created = DateTime.Now;
                company.DistinguishedName = distinguishedName;

                database.Companies.Add(company);
                database.SaveChanges();

                // Notify success
                ThrowEvent(AlertID.SUCCESS_NEW_RESELLER, reseller.CompanyName);
            }
            catch (Exception ex)
            {
                ThrowEvent(AlertID.FAILED, ex.Message);

                // Rollback on error
                events.RollBack();
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        /// <summary>
        /// Updates a reseller
        /// </summary>
        /// <param name="reseller"></param>
        public void UpdateReseller(ResellerObject reseller)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var dbObj = (from r in database.Companies
                            where r.CompanyCode == reseller.CompanyCode
                            where r.IsReseller
                            select r).First();

                dbObj.CompanyName = reseller.CompanyName;
                dbObj.AdminName = reseller.AdminName;
                dbObj.AdminEmail = reseller.AdminEmail;
                dbObj.PhoneNumber = reseller.Telephone;
                dbObj.Street = reseller.Street;
                dbObj.City = reseller.City;
                dbObj.State = reseller.State;
                dbObj.ZipCode = reseller.ZipCode;
                dbObj.Country = reseller.Country;

                database.SaveChanges();
            }
            catch (Exception ex)
            {
                ThrowEvent(AlertID.FAILED, ex.Message);
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }
    }
}
