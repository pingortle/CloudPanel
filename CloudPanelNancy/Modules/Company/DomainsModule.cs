using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules
{
    public class DomainsModule : NancyModule
    {
        public DomainsModule() : base("Company")
        {
            Get["{CompanyCode}/Domains"] = parameters =>
                {
                    List<DomainTest> domains = new List<DomainTest>();
                    domains.Add(new DomainTest() { ID = 1, DomainName = "compsysar.com" });
                    domains.Add(new DomainTest() { ID = 2, DomainName = "compsyscloud.com" });

                    return View["Company/Domains/DomainList.cshtml", domains];
                };

            Get["{CompanyCode}/Domains/Add"] = parameters =>
                {
                    // We are adding a domain
                    return View["Company/Domains/DomainList.cshtml"];
                };

            Get["{CompanyCode}/Domains/{DomainName}/Edit"] = parameters =>
                {
                    // We are editing a domain
                    return View["Company/Domains/DomainList.cshtml"];
                };

            Post["{CompanyCode}/Domains/{DomainName}/Delete"] = parameters =>
                {
                    // Check that domain belongs to this company
                    // Delete domain

                    string returnUrl = string.Format("~/Company/{0}/Domains", parameters.CompanyCode);
                    return Response.AsRedirect(returnUrl); // Return to list of domains which will repopulate data
                };
        }
    }

    #region Test Classes

    public class DomainTest
    {
        public int ID { get; set; }
        public string DomainName { get; set; }
    }

    #endregion
}