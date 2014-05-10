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
using Nancy.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy.Modules
{
    public class ResellerModule : NancyModule
    {
        public ResellerModule()
        {
            this.RequiresAuthentication();

            Get["/Resellers"] = _ =>
                {
                    List<ResellerObject> resellers = null; // Get all resellers

                    return View["Admin/ResellerList.cshtml", resellers];
                };

            Get["/Resellers/Add"] = parameters =>
            {
                ResellerObject newReseller = new ResellerObject();

                return View["Admin/ResellerEdit.cshtml", newReseller];
            };

            Get["/Resellers/{ResellerCode}/Edit"] = parameters =>
                {
                    ResellerObject reseller = new ResellerObject();
                    reseller.CompanyID = 0;
                    reseller.CompanyCode = parameters.ResellerCode;
                    reseller.CompanyName = "Reseller";
                    reseller.Street = "300 Simpson Rd";
                    reseller.City = "Vilonia";
                    reseller.State = "AR";
                    reseller.ZipCode = "72173";
                    reseller.Created = DateTime.Now;
                    reseller.AdminName = "";
                    reseller.AdminEmail = "";
                    reseller.Telephone = "";

                    return View["Admin/ResellerEdit.cshtml", reseller];
                };

            Get["/Resellers/{CompanyCode}/Companies"] = parameters =>
            {
                if (this.Context.CurrentUser != null)
                {
                    var user = this.Context.CurrentUser as AuthenticatedUser;
                    user.SelectedResellerCode = parameters.CompanyCode;
                }

                return View["Admin/CompanyList", parameters.CompanyCode];
            };

            Post["/Resellers/Edit"] = parameters =>
            {
                ViewBag.Testing = "NEW RESELLER";

                var test = this.Bind<ResellerObject>(); // Gather data from form

                                

                return View["Admin/ResellerEdit", test];
            };

            Post["/Resellers/Edit/{ResellerCode}"] = parameters =>
                {
                    ViewBag.Testing = "EDIT RESELLER";

                    var test = this.Bind<ResellerObject>();  // Gather data from form

                    return View["Admin/ResellerEdit", test];
                };
        }
    }
}