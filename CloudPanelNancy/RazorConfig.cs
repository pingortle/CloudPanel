using Nancy.ViewEngines.Razor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanelNancy
{
    public class RazorConfig : IRazorConfiguration
    {
        public IEnumerable<string> GetAssemblyNames()
        {
            yield return "CloudPanel.Modules.Common";
            yield return "CloudPanel.Modules.Base";
        }

        public IEnumerable<string> GetDefaultNamespaces()
        {
            yield return "Nancy.Validation";
            yield return "System.Globalization";
            yield return "System.Collections.Generic";
            yield return "System.Linq";
            yield return "CloudPanel.Modules.Base.Companies";
            yield return "CloudPanel.Modules.Common.ViewModel";
        }

        public bool AutoIncludeModelNamespace
        {
            get { return true; }
        }
    }
}