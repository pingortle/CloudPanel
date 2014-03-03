using CloudPanel.Modules.ActiveDirectory.Groups;
using CloudPanel.Modules.ActiveDirectory.OrganizationalUnits;
using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Common.Database;
using CloudPanel.Modules.Common.Other;
using CloudPanel.Modules.Common.Rollback;
using CloudPanel.Modules.Common.Settings;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class CompanyViewModel : IViewModel
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Gets a list of companies from the database
        /// </summary>
        /// <returns></returns>
        public List<CompanyObject> GetCompanies(string resellerCode)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var companyDb = from r in database.Companies
                                where !r.IsReseller
                                where r.ResellerCode == resellerCode
                                orderby r.CompanyName
                                select r;

                List<CompanyObject> companies = new List<CompanyObject>();

                if (companyDb != null)
                {
                    foreach (var company in companyDb)
                    {
                        var domainsDb = from d in database.Domains
                                        where d.CompanyCode == company.CompanyCode
                                        select d.Domain1;

                        CompanyObject tmp = new CompanyObject();
                        tmp.CompanyID = company.CompanyId;
                        tmp.CompanyName = company.CompanyName;
                        tmp.CompanyCode = company.CompanyCode;
                        tmp.Street = company.Street;
                        tmp.City = company.City;
                        tmp.State = company.State;
                        tmp.ZipCode = company.ZipCode;
                        tmp.Country = company.Country;
                        tmp.Telephone = company.PhoneNumber;
                        tmp.Description = company.Description;
                        tmp.AdminName = company.AdminName;
                        tmp.AdminEmail = company.AdminEmail;
                        tmp.DistinguishedName = company.DistinguishedName;
                        tmp.Created = company.Created;
                        tmp.Domains = domainsDb.ToArray();

                        companies.Add(tmp);
                    }
                }

                return companies;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving companies for reseller " + resellerCode, ex);

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
        /// Gets a specific company
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public CompanyObject GetCompany(string companyCode)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var company = (from c in database.Companies
                                where !c.IsReseller
                                where c.CompanyCode == companyCode
                                orderby c.CompanyName
                                select c).FirstOrDefault();

                CompanyObject tmp = new CompanyObject();
                tmp.CompanyID = company.CompanyId;
                tmp.CompanyName = company.CompanyName;
                tmp.CompanyCode = company.CompanyCode;
                tmp.Street = company.Street;
                tmp.City = company.City;
                tmp.State = company.State;
                tmp.ZipCode = company.ZipCode;
                tmp.Country = company.Country;
                tmp.Telephone = company.PhoneNumber;
                tmp.Description = company.Description;
                tmp.AdminName = company.AdminName;
                tmp.AdminEmail = company.AdminEmail;
                tmp.DistinguishedName = company.DistinguishedName;
                tmp.Created = company.Created;

                return tmp;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving company " + companyCode, ex);

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
        /// Creates a new company
        /// </summary>
        /// <param name="company"></param>
        /// <param name="resellerCode"></param>
        public void NewCompany(CompanyObject company, string resellerCode)
        {
            CPDatabase database = null;

            // Rollback class in case something goes wrong we can undo changes
            CloudPanelTransaction events = new CloudPanelTransaction();

            try
            {
                database = new CPDatabase();

                // Generate the company code
                string companyCode = CompanyObject.GenerateCompanyCode(company.CompanyName, company.UseCompanyNameInsteadofCompanyCode);

                // Loop and make sure one does exist or find one that does
                int count = 0;
                for (int i = 0; i < 1000; i++)
                {
                    if (Validation.DoesCompanyCodeExist(companyCode))
                    {
                        this.logger.Info("Tried to create a new company with company code " + companyCode + " but it already existed... trying another code");

                        companyCode = companyCode + count.ToString();
                        count++;
                    }
                    else
                    {
                        company.CompanyCode = companyCode; // Assign company code to object
                        break;
                    }
                }

                // Get the resellers distinguished name
                var resellerDistinguishedName = (from r in database.Companies
                                                 where r.IsReseller
                                                 where r.CompanyCode == resellerCode
                                                 select r.DistinguishedName).First();

                #region Create Organizational Units

                //
                // Create organizational units
                //
                ADOrganizationalUnit adOrg = new ADOrganizationalUnit(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);

                // Check if resellers are enabled and create the organizational unit in the correct place
                string newCompanyDistinguishedName = string.Empty;
                if (!StaticSettings.ResellersEnabled)
                    newCompanyDistinguishedName = adOrg.CreateCompany(StaticSettings.HostingOU, company);
                else
                    newCompanyDistinguishedName = adOrg.CreateCompany(resellerDistinguishedName, company);
                events.NewOrganizationalUnitEvent(newCompanyDistinguishedName);
                adOrg.RemoveAuthUsersRights(newCompanyDistinguishedName); // Removes authenticated users from the OU;

                // Create the Exchange OU
                string exchangeOU = adOrg.CreateOU(newCompanyDistinguishedName, "Exchange", company.Domains[0]);
                events.NewOrganizationalUnitEvent(exchangeOU);
                adOrg.RemoveAuthUsersRights(exchangeOU); // Removes authenticated users from the OU;

                // Create the Applications OU
                string applicationsOU = adOrg.CreateOU(newCompanyDistinguishedName, "Applications", company.Domains[0]);
                events.NewOrganizationalUnitEvent(applicationsOU);
                adOrg.RemoveAuthUsersRights(applicationsOU); // Removes authenticated users from the OU;

                // Create the custom users OU if there is one
                string customUsersOU = string.Empty;
                if (!string.IsNullOrEmpty(StaticSettings.UsersOU))
                {
                    customUsersOU = adOrg.CreateOU(newCompanyDistinguishedName, StaticSettings.UsersOU, company.Domains[0]);
                    events.NewOrganizationalUnitEvent(customUsersOU);
                    adOrg.RemoveAuthUsersRights(customUsersOU); // Removes authenticated users from the OU;
                }
                #endregion

                #region Create Security Groups
                //
                // Create Security Groups
                //
                ADGroup adGroup = new ADGroup(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);

                // Create the Admins security group
                adGroup.Create(newCompanyDistinguishedName, "Admins@" + companyCode, "", true, false);
                events.NewSecurityGroup("Admins@" + companyCode);

                // Create the All Users security group
                adGroup.Create(newCompanyDistinguishedName, "AllUsers@" + companyCode, "", true, false);
                events.NewSecurityGroup("AllUsers@" + companyCode);

                // Create the AllTSUsers security groups
                adGroup.Create(newCompanyDistinguishedName, "AllTSUsers@" + companyCode, "", true, false);
                events.NewSecurityGroup("AllTSUsers@" + companyCode);

                // Add AllTSUsers to the AllTSUsers@Hosting group
                adGroup.AddMember("AllTSUsers@Hosting", "AllTSUsers@" + companyCode, "name");

                // Check the GPOAccess and see if we are using resellers or not. Then add the group to the GPOAccess security group
                if (StaticSettings.ResellersEnabled)
                {
                    adGroup.AddMember("GPOAccess@"+ resellerCode, "AllTSUsers@" + companyCode, "name");
                }
                else
                {
                    adGroup.AddMember("GPOAccess@Hosting", "AllTSUsers@" + companyCode, "name");
                }
                #endregion

                #region Add read rights to organizational units
                //
                // Now add read rights
                // 
                adOrg.AddAccessRights(newCompanyDistinguishedName, "AllUsers@ " + companyCode);
                adOrg.AddAccessRights(exchangeOU, "AllUsers@" + companyCode);
                adOrg.AddAccessRights(applicationsOU, "AllUsers@" + companyCode);

                if (!string.IsNullOrEmpty(StaticSettings.UsersOU))
                    adOrg.AddAccessRights(customUsersOU, "AllUsers@" + companyCode);

                #endregion
                
                // Insert into database
                Company newCompanyDb = new Company();
                newCompanyDb.IsReseller = false;
                newCompanyDb.CompanyName = company.CompanyName;
                newCompanyDb.CompanyCode = company.CompanyCode;
                newCompanyDb.ResellerCode = resellerCode;
                newCompanyDb.Street = company.Street;
                newCompanyDb.City = company.City;
                newCompanyDb.State = company.State;
                newCompanyDb.ZipCode = company.ZipCode;
                newCompanyDb.Country = company.Country;
                newCompanyDb.PhoneNumber = company.Telephone;
                newCompanyDb.AdminName = company.AdminName;
                newCompanyDb.AdminEmail = company.AdminEmail;
                newCompanyDb.DistinguishedName = newCompanyDistinguishedName;
                newCompanyDb.Created = DateTime.Now;

                database.Companies.Add(newCompanyDb);
                database.SaveChanges();
                events.InsertCompanyToDatabase(newCompanyDb.CompanyCode);

                // Insert domain into database
                Domain newDomain = new Domain();
                newDomain.CompanyCode = newCompanyDb.CompanyCode;
                newDomain.Domain1 = company.Domains[0];
                newDomain.IsAcceptedDomain = false;
                newDomain.IsLyncDomain = false;
                newDomain.IsSubDomain = false;

                database.Domains.Add(newDomain);
                database.SaveChanges();

                // Notify success
                ThrowEvent(AlertID.SUCCESS_NEW_COMPANY, newCompanyDb.CompanyName);
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
    }
}
