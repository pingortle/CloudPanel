using CloudPanel.Modules.ActiveDirectory.OrganizationalUnits;
using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Common.Database;
using CloudPanel.Modules.Common.Rollback;
using CloudPanel.Modules.Common.Settings;
using CloudPanel.Modules.Exchange;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
    public class DomainsViewModel : IViewModel
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public List<DomainsObject> GetDomains(string companyCode)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var companyDomains = from d in database.Domains
                                     where d.CompanyCode == companyCode
                                     select new DomainsObject()
                                     {
                                         DomainID = d.DomainID,
                                         CompanyCode = d.CompanyCode,
                                         DomainName = d.Domain1,
                                         IsDefault = d.IsDefault,
                                         IsAcceptedDomain = d.IsAcceptedDomain,
                                         TypeOfDomain = d.DomainType == null ? DomainType.BasicDomain : (DomainType)d.DomainType
                                     };

                if (companyDomains != null)
                {
                    return companyDomains.ToList();
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving domains for company" + companyCode, ex);
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public DomainsObject GetDomain(int domainID)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var companyDomain = (from d in database.Domains
                                      where d.DomainID == domainID
                                      select new DomainsObject()
                                      {
                                          DomainID = d.DomainID,
                                          CompanyCode = d.CompanyCode,
                                          DomainName = d.Domain1,
                                          IsDefault = d.IsDefault,
                                          IsAcceptedDomain = d.IsAcceptedDomain,
                                          TypeOfDomain = d.DomainType == null ? DomainType.BasicDomain : (DomainType)d.DomainType
                                      }).First();

                return companyDomain;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error retrieving domain id " + domainID.ToString(), ex);
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
                return null;
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public void AddDomain(string domainName, string companyCode, bool isDefault, bool isExchangeEnabled, DomainType domainType)
        {
            CPDatabase database = null;
            ADOrganizationalUnit adOrg = null;
            ExchangePowershell powershell = null;

            CloudPanelTransaction transaction = new CloudPanelTransaction();
            try
            {
                // Get company distinguished name
                database = new CPDatabase();
                var dn = (from d in database.Companies
                          where !d.IsReseller
                          where d.CompanyCode == companyCode
                          select d.DistinguishedName).First();

                // Remove any whitespace characters at the beginning and end
                domainName = domainName.Trim();

                // Check if domain is already in database
                bool alreadyExist = IsDomainInUse(domainName);
                if (alreadyExist)
                    ThrowEvent(Base.Enumerations.AlertID.FAILED, "Domain already exists");
                else
                {
                    // Add domain to Active Directory
                    adOrg = new ADOrganizationalUnit(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                    adOrg.AddDomain(dn, domainName);
                    transaction.NewDomain(dn, domainName);

                    // If it is default we need to remove default from all others
                    if (isDefault)
                    {
                        var defaultDomains = from d in database.Domains
                                             where d.CompanyCode == companyCode
                                             select d;

                        foreach (Domain d in defaultDomains)
                        {
                            if (d.IsDefault)
                                d.IsDefault = false;
                        }
                    }

                    //
                    // Check if it is Exchange enabled
                    //
                    if (isExchangeEnabled)
                    {
                        powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                        powershell.NewDomain(domainName, domainType);
                        transaction.NewExchangeDomain(domainName);
                    }

                    // Add new domain
                    Domain newDomain = new Domain();
                    newDomain.IsDefault = isDefault;
                    newDomain.CompanyCode = companyCode;
                    newDomain.Domain1 = domainName;
                    newDomain.IsSubDomain = false;
                    newDomain.IsAcceptedDomain = isExchangeEnabled;
                    newDomain.IsLyncDomain = false;
                    newDomain.DomainType = (int)domainType;
                    database.Domains.Add(newDomain);

                    // Save all changes
                    database.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to add domain " + domainName + " to company " + companyCode, ex);

                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);

                // Rollback
                transaction.RollBack();
            }
            finally
            {
                if (powershell != null)
                    powershell.Dispose();

                if (database != null)
                    database.Dispose();
            }
        }

        public void  UpdateDomain(string domainName, string companyCode, bool isDefault, bool isExchangeEnabled, DomainType domainType)
        {
            CPDatabase database = null;
            ExchangePowershell powershell = null;

            try
            {
                // Remove any whitespace characters at the beginning and end
                domainName = domainName.Trim();

                database = new CPDatabase();
                var defaultDomains = from d in database.Domains
                                     where d.CompanyCode == companyCode
                                     select d;

                foreach (Domain d in defaultDomains)
                {
                    if (d.Domain1.Equals(domainName, StringComparison.CurrentCultureIgnoreCase))
                    {
                        // This is the domain we are updating
                        d.IsDefault = isDefault;
                        d.DomainType = (int)domainType;

                        // Check if it wasn't an Exchange domain and we are making it an Exchange domain
                        if (!d.IsAcceptedDomain && isExchangeEnabled)
                        {
                            // Create accepted domain
                            powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                            powershell.NewDomain(domainName, domainType);
                            d.IsAcceptedDomain = true;
                        }
                        else if (d.IsAcceptedDomain && !isExchangeEnabled)
                        {
                            // Delete accepted domain
                            powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                            powershell.DeleteDomain(domainName);
                            d.IsAcceptedDomain = false;
                        }
                        else if (d.IsAcceptedDomain && isExchangeEnabled)
                        {
                            // Update accepted domain
                            powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                            powershell.UpdateDomain(domainName, domainType);
                            d.IsAcceptedDomain = true;
                        }
                    }
                    else
                    {
                        if (isDefault)
                            d.IsDefault = false;
                    }
                }

                database.SaveChanges();
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to update domain " + domainName + " for company " + companyCode, ex);
                ThrowEvent(AlertID.FAILED, ex.Message);
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        public void DeleteDomain(string domainName, string companyCode)
        {
            CPDatabase database = null;
            ADOrganizationalUnit adOrg = null;
            ExchangePowershell powershell = null;

            try
            {
                database = new CPDatabase();

                // Make sure no users groups or anything is using this domain
                var usersUsing = (from u in database.Users
                                  where (u.UserPrincipalName.EndsWith("@" + domainName) || u.Email.EndsWith("@" + domainName))
                                  select u).Count();

                if (usersUsing > 0)
                    ThrowEvent(AlertID.FAILED, "The domain is in use " + domainName);
                else
                {
                    // Make sure no groups are using this domain
                    var groupsUsing = (from g in database.DistributionGroups
                                       where g.Email.EndsWith("@" + domainName)
                                       select g).Count();

                    if (groupsUsing > 0)
                        ThrowEvent(AlertID.FAILED, "The domain is in use " + domainName);
                    else
                    {
                        // Since users & groups are not using this domain we can continue and remove it

                        // Get company distinguished name
                        var dn = (from d in database.Companies
                                  where !d.IsReseller
                                  where d.CompanyCode == companyCode
                                  select d.DistinguishedName).First();

                        // Delete domain from Active Directory
                        adOrg = new ADOrganizationalUnit(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                        adOrg.RemoveDomain(dn, domainName);

                        // Get domain from SQL
                        var domain = (from d in database.Domains
                                      where d.Domain1 == domainName
                                      where d.CompanyCode == companyCode
                                      select d).First();

                        // Check if it was enabled for Exchange
                        if (domain.IsAcceptedDomain)
                        {
                            powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                            powershell.DeleteDomain(domain.Domain1);
                        }

                        database.Domains.Remove(domain);
                        database.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to remove domain " + domainName + " from company " + companyCode, ex);
                ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
            }
            finally
            {
                if (powershell != null)
                    powershell.Dispose();

                if (database != null)
                    database.Dispose();
            }
        }

        private bool IsDomainInUse(string domainName)
        {
            CPDatabase database = null;

            try
            {
                database = new CPDatabase();

                var domainCount = (from d in database.Domains
                                   where d.Domain1 == domainName
                                   select d).Count();

                if (domainCount > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                this.logger.Error("Error checking if domain " + domainName + " is in use.", ex);

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
