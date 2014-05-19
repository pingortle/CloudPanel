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
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Bootstrapper;
using Nancy.Session;
using Nancy.TinyIoc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CloudPanelNancy
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            try
            {
                XDocument xDoc = XDocument.Load(HttpContext.Current.Server.MapPath("~/Config/settings.xml"));

                /* Load CloudPanel Settings */
                var settings = from s in xDoc.Elements("Settings")
                               select s;

                foreach (var s in settings)
                {
                    Settings.HostingOU = s.Element("HostingOU").Value;
                    Settings.PrimaryDC = s.Element("DomainController").Value;
                    Settings.Username = s.Element("Username").Value;
                    Settings.Password = s.Element("Password").Value;
                    Settings.SuperAdmins = s.Element("SuperAdmins").Value;
                    Settings.BillingAdmins = s.Element("BillingAdmins").Value;
                }

                /* Load Exchange Settings */
                var exchange = from s in xDoc.Elements("Exchange")
                               select s;

                foreach (var s in exchange)
                {
                    Settings.ExchangeServer = s.Element("Server").Value;
                    Settings.ExchangePFServer = s.Element("PFServer").Value;

                    int defaultVersion = 2013;
                    int.TryParse(s.Element("Version").Value, out defaultVersion);
                    Settings.Version = defaultVersion;

                    bool defaultBool = true;
                    bool.TryParse(s.Element("SSL").Value, out defaultBool);
                    Settings.ExchangeSSL = defaultBool;

                    Settings.ExchangeConnection = s.Element("Connection").Value;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.ToString());
            }


            CookieBasedSessions.Enable(pipelines);

            base.ApplicationStartup(container, pipelines);

            /*var authenticationConfiguration =
                new FormsAuthenticationConfiguration
                {
                    RedirectUrl = "~/login",
                    UserMapper = container.Resolve<IUserMapper>(),
                };

            FormsAuthentication.Enable(pipelines, authenticationConfiguration);*/
        }

        protected override void ConfigureRequestContainer(TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);

            // Here we register our user mapper as a per-request singleton.
            // As this is now per-request we could inject a request scoped
            // database "context" or other request scoped services.
            container.Register<IUserMapper, UserMapper>();
            container.Register<CloudPanelContext>((x, y) => string.IsNullOrEmpty(Settings.ConnectionString) ? null : new CloudPanelContext(Settings.ConnectionString));
        }

        protected override void RequestStartup(TinyIoCContainer requestContainer, IPipelines pipelines, NancyContext context)
        {
            // At request startup we modify the request pipelines to
            // include forms authentication - passing in our now request
            // scoped user name mapper.
            //
            // The pipelines passed in here are specific to this request,
            // so we can add/remove/update items in them as we please.
            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
                {
                    RedirectUrl = "~/login",
                    UserMapper = requestContainer.Resolve<IUserMapper>(),
                };

            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }
    }

    public static class SerializationMixins
    {
        public static void Serialize<TK, TV>(this IDictionary<TK, TV> This, TextWriter writer)
        {
            new XmlSerializer(typeof(List<Entry>)).Serialize(
                writer,
                This.Select(x => new Entry(x.Key, x.Value)).ToList());
        }

        public static IDictionary<TK, TV> Deserialize<TK, TV>(this TextReader This)
        {
            List<Entry> list = new List<Entry>();
            try
            {
                list = new XmlSerializer(typeof(List<Entry>)).Deserialize(This) as List<Entry>;
            }
            catch (InvalidOperationException)
            { }

            return list.ToDictionary(x => (TK)x.Key, x => (TV)x.Value);
        }
    }

    public class Entry
    {
        public object Key { get; set; }
        public object Value { get; set; }

        public Entry()
        {
        }

        public Entry(object key, object value)
        {
            Key = key;
            Value = value;
        }
    }
}