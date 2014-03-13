using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Common.Database;
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
                                         IsDefault = d.IsDefault
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
                                          IsDefault = d.IsDefault
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
    }
}
