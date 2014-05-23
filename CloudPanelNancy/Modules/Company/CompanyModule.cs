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
using CloudPanel.Modules.Persistence.EntityFramework.Models;

namespace CloudPanelNancy.Modules
{
    public class ContactsList : List<Contact>
    {
        public ContactsList(string companyCode, IEnumerable<Contact> source = null)
        {
            CompanyCode = companyCode;
            this.AddRange(source ?? new List<Contact>());
        }

        public string CompanyCode { get; private set; }
    }
    
    public class CompanyModule : NancyModule
    {
        public CompanyModule(CloudPanelContext db) : base("Company")
        {
            this.RequiresAuthentication();
            this.RequiresAnyClaim(new[] { "SuperAdmin", "ResellerAdmin", "CompanyAdmin" });

            Get["{CompanyCode}"] = parameters =>
                {
                    string companyCode = parameters.CompanyCode;

                    var user = this.Context.CurrentUser as AuthenticatedUser;
                    user.SelectedCompanyCode = companyCode;

                    // Get company information
                    var companyData = (from c in db.Companies
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

                    return View["Company/Overview", companyData];
                };

            Get["{CompanyCode}/Contacts"] = parameters =>
                {
                    string cc = parameters.CompanyCode;
                    var contacts = db.Contacts.Where(x => x.CompanyCode == cc);
                    var model = new ContactsList(cc, contacts);

                    return View["Company/Email/Contacts", model];
                };

            Get["{CompanyCode}/Contacts/{ContactId:int}"] = parameters =>
                {
                    string cc = parameters.CompanyCode;
                    int id = parameters.ContactId;

                    var contact = db.Contacts.FirstOrDefault(x => x.CompanyCode == cc && x.Id == id);
                    if (contact == null)
                        return Context.Response.WithStatusCode(HttpStatusCode.NotFound);

                    return View["Company/Email/EditContact", contact];
                };

            Put["{CompanyCode}/Contacts/{ContactId:int}"] = parameters =>
                {
                    string cc = parameters.CompanyCode;
                    int id = parameters.ContactId;

                    var contact = db.Contacts.FirstOrDefault(x => x.CompanyCode == cc && x.Id == id);
                    if (contact == null)
                        return Context.Response.WithStatusCode(HttpStatusCode.NotFound);

                    // This is somewhat simplistic.  Needs more thought.
                    contact.DisplayName = Request.Form.displayName;
                    contact.DistinguishedName = Request.Form.distinguishedName;
                    contact.Email = Request.Form.email;
                    contact.Hidden = Request.Form.hidden;

                    db.SaveChanges();

                    return Response.AsRedirect(string.Format("Company/{0}/Contacts", cc));
                };

            Post["{CompanyCode}/Contacts"] = parameters =>
                {
                    string cc = parameters.CompanyCode;

                    db.Contacts.Add(new Contact
                    {
                        CompanyCode = cc,
                        DisplayName = Request.Form.displayName,
                        DistinguishedName = Request.Form.distinguishedName,
                        Email = Request.Form.email,
                        Hidden = Request.Form.hidden,
                    });

                    db.SaveChanges();

                    return Response.AsRedirect(string.Format("Company/{0}/Contacts", cc));
                };

            Delete["{CompanyCode}/Contacts/{ContactId:int}"] = parameters =>
                {
                    string cc = parameters.CompanyCode;
                    int id = parameters.ContactId;

                    var contact = db.Contacts.FirstOrDefault(x => x.CompanyCode == cc && x.Id == id);
                    if (contact == null)
                        return Context.Response.WithStatusCode(HttpStatusCode.NotFound);

                    db.Contacts.Remove(contact);

                    db.SaveChanges();

                    return Response.AsRedirect(string.Format("Company/{0}/Contacts", cc));
                };
        }
    }
}