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

using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy.Security;
using Nancy.Authentication.Forms;
using CloudPanel.Modules.Base.Companies;
using CloudPanel.Modules.Persistence.EntityFramework;

namespace CloudPanelNancy.Modules
{
    public class CompanyModule : NancyModule
    {
        public CompanyModule() : base("Company")
        {
            this.RequiresAuthentication();
            this.RequiresAnyClaim(new[] { "SuperAdmin", "ResellerAdmin", "CompanyAdmin" });

            Get["{CompanyCode}"] = parameters =>
                {
                    string companyCode = parameters.CompanyCode;

                    var user = this.Context.CurrentUser as AuthenticatedUser;
                    user.SelectedCompanyCode = companyCode;

                    // Get company information
                    var companyData = new CompanyObject();
                    using (var ctx = new CloudPanelContext(Settings.ConnectionString))
                    {
                        companyData = (from c in ctx.Companies
                                       where !c.IsReseller
                                       where c.CompanyCode == companyCode
                                       select new CompanyObject()
                                       {
                                           CompanyCode = companyCode,
                                           CompanyName = c.CompanyName,
                                           AdminName = c.AdminName,
                                           Telephone = c.PhoneNumber,
                                           CompanyPlanID = c.OrgPlanID == null ? 0 : (int)c.OrgPlanID
                                       }).FirstOrDefault();

                        user.SelectedCompanyName = companyData.CompanyName;
                    }

                    return View["Company/Overview", companyData];
                };
        }
    }
}