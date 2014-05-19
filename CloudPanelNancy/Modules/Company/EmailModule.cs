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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nancy;
using Nancy.Security;
using CloudPanel.Modules.Persistence.EntityFramework;

namespace CloudPanelNancy.Modules.Company
{
    public class EmailModule : NancyModule
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(EmailModule));

        public EmailModule() : base("Company")
        {
            //this.RequiresAuthentication();
            //this.RequiresAnyClaim(new[] { "SuperAdmin", "ResellerAdmin", "CompanyAdmin" });

            Get["{CompanyCode}/Email/Status"] = parameters =>
                {
                    CloudPanelContext ctx = null;
                    try
                    {
                        ctx = new CloudPanelContext(Settings.ConnectionString);

                        string companyCode = parameters.CompanyCode;
                        var company = (from c in ctx.Companies
                                       where !c.IsReseller
                                       where c.CompanyCode == companyCode
                                       select c).FirstOrDefault();

                        if (company.ExchEnabled)
                        {
                            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                            var random = new Random();
                            var result = new string(
                                Enumerable.Repeat(chars, 8)
                                          .Select(s => s[random.Next(s.Length)])
                                          .ToArray());

                            return View["Company/Email/Disable.cshtml", result];
                        }
                        else
                        {
                            return View["Company/Email/Enable.cshtml"];
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error checking Exchange status for " + parameters.CompanyCode, ex);
                        return View["Company/Overview.cshtml"];
                    }
                    finally
                    {
                        ctx.Dispose();
                    }
                };

            Post["{CompanyCode}/Email/Enable"] = parameters =>
                {
                    // If successful take them to the disable Exchange page
                    // If they are NOT successful take them back to the enable Exchange page and display an error message why it wasn't successful
                    return Response.AsRedirect("Status");
                };

            Post["{CompanyCode}/Email/Disable"] = parameters =>
                {
                    // If we successfully disable email then redirect to enable page
                    // Otherwise redirect to same page displaying the error message
                    return Response.AsRedirect("Status");
                };
        }
    }
}