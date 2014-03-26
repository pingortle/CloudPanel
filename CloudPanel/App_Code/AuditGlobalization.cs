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
                            a.ActionIDGlobalization = Resources.LocalizedText.Audit_NewUser;
                            break;
                        case Modules.Base.Enumerations.ActionID.DeleteUser:
                            a.ActionIDGlobalization = Resources.LocalizedText.Audit_DeleteUser;
                            break;
                        case Modules.Base.Enumerations.ActionID.UpdateUser:
                            a.ActionIDGlobalization = Resources.LocalizedText.Audit_UpdateUser;
                            break;
                        case Modules.Base.Enumerations.ActionID.CreateCompany:
                            a.ActionIDGlobalization = Resources.LocalizedText.Audit_NewCompany;
                            break;
                        case Modules.Base.Enumerations.ActionID.CreateReseller:
                            a.ActionIDGlobalization = Resources.LocalizedText.Audit_NewReseller;
                            break;
                        case Modules.Base.Enumerations.ActionID.SaveSettings:
                            a.ActionIDGlobalization = Resources.LocalizedText.Audit_SavedSettings;
                            break;
                        case Modules.Base.Enumerations.ActionID.UpdateCompany:
                            a.ActionIDGlobalization = Resources.LocalizedText.Audit_UpdateCompany;
                            break;
                        case Modules.Base.Enumerations.ActionID.UpdateReseller:
                            a.ActionIDGlobalization = Resources.LocalizedText.Audit_UpdateReseller;
                            break;
                        case Modules.Base.Enumerations.ActionID.ResetPassword:
                            a.ActionIDGlobalization = Resources.LocalizedText.Audit_ResetPassword;
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