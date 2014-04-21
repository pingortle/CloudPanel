using CloudPanel.Modules.Base.Auditing;
using CloudPanel.Modules.Common.GlobalActions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CloudPanel
{
    public static class AuditGlobalization
    {
        /// <summary>
        /// Gets a list of audits or returns null if it couldn't find any
        /// </summary>
        /// <param name="companyCode"></param>
        /// <returns></returns>
        public static List<Audits> GetAuditing(string companyCode)
        {
            List<Audits> audits = AuditGlobal.RetrieveAudits(companyCode);
            if (audits != null)
            {
                foreach (Audits a in audits)
                {
                    switch (a.Action)
                    {
                        case Modules.Base.Enumerations.ActionID.CreateUser:
                            a.ActionIDGlobalization = "Created new user";
                            break;
                        case Modules.Base.Enumerations.ActionID.DeleteUser:
                            a.ActionIDGlobalization = "Deleted user";
                            break;
                        case Modules.Base.Enumerations.ActionID.UpdateUser:
                            a.ActionIDGlobalization = "Updated user";
                            break;
                        case Modules.Base.Enumerations.ActionID.CreateCompany:
                            a.ActionIDGlobalization = "Created new company";
                            break;
                        case Modules.Base.Enumerations.ActionID.CreateReseller:
                            a.ActionIDGlobalization = "Created new reseller";
                            break;
                        case Modules.Base.Enumerations.ActionID.SaveSettings:
                            a.ActionIDGlobalization = "Saved settings";
                            break;
                        case Modules.Base.Enumerations.ActionID.UpdateCompany:
                            a.ActionIDGlobalization = "Updated company";
                            break;
                        case Modules.Base.Enumerations.ActionID.UpdateReseller:
                            a.ActionIDGlobalization = "Updated reseller";
                            break;
                        case Modules.Base.Enumerations.ActionID.ResetPassword:
                            a.ActionIDGlobalization = "Reset password";
                            break;
                        default:
                            break;
                    }
                }
            }

            return audits;
        }
    }
}