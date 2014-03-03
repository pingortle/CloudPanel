using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanel
{
    public static class WebSessionHandler
    {
        public static bool IsSuperAdmin
        {
            get
            {
                if (HttpContext.Current.Session["CP_IsSuperAdmin"] != null)
                    return (bool)HttpContext.Current.Session["CP_IsSuperAdmin"];
                else
                    return false;
            }
            set
            {
                HttpContext.Current.Session["CP_IsSuperAdmin"] = value;
            }
        }

        public static bool IsResellerAdmin
        {
            get
            {
                if (HttpContext.Current.Session["CP_IsResellerAdmin"] != null)
                    return (bool)HttpContext.Current.Session["CP_IsResellerAdmin"];
                else
                    return false;
            }
            set
            {
                HttpContext.Current.Session["CP_IsResellerAdmin"] = value;
            }
        }

        public static bool IsCompanyAdmin
        {
            get
            {
                if (HttpContext.Current.Session["CP_IsCompanyAdmin"] != null)
                    return (bool)HttpContext.Current.Session["CP_IsCompanyAdmin"];
                else
                    return false;
            }
            set
            {
                HttpContext.Current.Session["CP_IsCompanyAdmin"] = value;
            }
        }

        public static string DisplayName
        {
            get
            {
                if (HttpContext.Current.Session["CP_DisplayName"] != null)
                    return HttpContext.Current.Session["CP_DisplayName"].ToString();
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["CP_DisplayName"] = value;
            }
        }

        public static string SelectedResellerCode
        {
            get
            {
                if (HttpContext.Current.Session["CP_SelectedResellerCode"] != null)
                    return HttpContext.Current.Session["CP_SelectedResellerCode"].ToString();
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["CP_SelectedResellerCode"] = value;
            }
        }

        public static string SelectedCompanyCode
        {
            get
            {
                if (HttpContext.Current.Session["CP_SelectedCompanyCode"] != null)
                    return HttpContext.Current.Session["CP_SelectedCompanyCode"].ToString();
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["CP_SelectedCompanyCode"] = value;
            }
        }

        public static string Username
        {
            get
            {
                if (HttpContext.Current.User.Identity != null)
                    return HttpContext.Current.User.Identity.Name;
                else
                    return "Not logged in user";
            }
        }
    }
}