using Nancy.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy
{
    public class AuthenticatedUser : IUserIdentity
    {
        public Guid UserGuid { get; set; }

        public string UserName { get; set; }

        public string SelectedResellerCode { get; set; }

        public string SelectedCompanyCode { get; set; }

        public string CompanyCode { get; set; }

        public IEnumerable<string> Claims { get; set; }
    }
}