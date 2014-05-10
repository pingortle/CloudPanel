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

using CloudPanel.Modules.Base.Companies;
using Nancy;
using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules.AJAX
{
    public class ajaxReseller : NancyModule
    {
        public ajaxReseller() : base("/AJAX")
        {
            //this.RequiresAuthentication();
            //this.RequiresAnyClaim(new[] { "SuperAdmin", "ResellerAdmin" });

            Get["/Resellers/{ResellerCode}/Companies/GetAll"] = parameters => GetCompanies(parameters.ResellerCode);
        }

        public Response GetCompanies(string resellerCode)
        {
            //CompanyViewModel viewModel = new CompanyViewModel();
                   // List<CompanyObject> companies = viewModel.GetCompanies(parameters.ResellerCode);
            List<CompanyObject> companies = new List<CompanyObject>();
            for (int i = 0; i < 1000; i++)
            {
                companies.Add(new CompanyObject()
                {
                    CompanyID = i,
                    CompanyCode = "Company" + i.ToString(),
                    CompanyName = "Company" + i.ToString(),
                    Street = "300 Simpson Rd",
                    City = "Vilonia",
                    State = "AR",
                    ZipCode = "72173",
                    Created = DateTime.Now
                });
            }

            if (companies != null)
            {
                        int start = Convert.ToInt32(Request.Query.iDisplayStart.ToString());
                        int length = Convert.ToInt32(Request.Query.iDisplayLength.ToString());
                        var totalRecords = companies.Count();
                        var secho = Request.Query.sEcho;
                        var sorting = Request.Query.sSortDir_0;

                        if (sorting == "asc")
                        {
                            return Response.AsJson(new { aaData = companies.OrderBy(x => x.CompanyName).Skip(start).Take(length), sEcho = secho, iTotalRecords = totalRecords, iTotalDisplayRecords = totalRecords });
                        }
                        else
                        {
                            return Response.AsJson(new { aaData = companies.OrderByDescending(x => x.CompanyName).Skip(start).Take(length), sEcho = secho.ToString(), iTotalRecords = totalRecords, iTotalDisplayRecords = totalRecords });
                        }
              }
              else
                return Response.AsJson(new { aaData = new List<CompanyObject>(), iTotalRecords = 0, iTotalDisplayRecords = 0 }, HttpStatusCode.NoContent);
        }
    }
}