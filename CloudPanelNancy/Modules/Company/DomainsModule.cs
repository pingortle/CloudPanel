//
// Copyright (c) 2014, Jacob Dixon
// All rights reserved.
//
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
// 1. Redistributions of source code must retain the above copyright
//    notice, this list of conditions and the following disclaimer.
// 2. Redistributions in binary form must reproduce the above copyright
//    notice, this list of conditions and the following disclaimer in the
//    documentation and/or other materials provided with the distribution.
// 3. All advertising materials mentioning features or use of this software
//    must display the following acknowledgement:
//    This product includes software developed by KnowMoreIT and Compsys.
// 4. Neither the name of KnowMoreIT and Compsys nor the
//    names of its contributors may be used to endorse or promote products
//    derived from this software without specific prior written permission.
//
// THIS SOFTWARE IS PROVIDED BY Jacob Dixon ''AS IS'' AND ANY
// EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL Jacob Dixon BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//

using CloudPanel.Modules.Persistence.EntityFramework;
using CloudPanel.Modules.Persistence.EntityFramework.Models;
using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules
{
    public class DomainsModule : NancyModule
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(DomainsModule));

        public DomainsModule(CloudPanelContext db) : base("Company")
        {
            Get["{CompanyCode}/Domains"] = parameters =>
                {
                    var domains = new List<Domain>();

                    try
                    {

                        string companyCode = parameters.CompanyCode;
                        domains = (from d in db.Domains
                                   where d.CompanyCode == companyCode
                                   select d).ToList();
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error getting domains for " + parameters.CompanyCode, ex);
                    }

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