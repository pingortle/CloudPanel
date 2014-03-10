using CloudPanel.Modules.Base.Citrix;
using CloudPanel.Modules.Common.Database;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.Common.ViewModel
{
   public class CompanyCitrixViewModel : IViewModel
   {
       private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

       public List<ApplicationsObject> GetCitrixApplications(string companyCode)
       {
           CPDatabase database = null;

           try
           {
               database = new CPDatabase();

               var foundApplications = (from a in database.Plans_Citrix
                                        where a.CompanyCode == null || a.CompanyCode == companyCode
                                        orderby a.Name
                                        select new ApplicationsObject()
                                        {
                                            CitrixPlanID = a.CitrixPlanID,
                                            DisplayName = a.Name,
                                            GroupName = a.GroupName,
                                            Description = a.Description,
                                            IsServer = a.IsServer,
                                            CompanyCode = a.CompanyCode,
                                            Price = a.Price,
                                            Cost = a.Cost,
                                            PictureURL = a.PictureURL
                                        });

               if (foundApplications != null)
                   return foundApplications.ToList();
               else
                   return null;
           }
           catch (Exception ex)
           {
               this.logger.Error("Error retrieving Citrix applications for company " + companyCode, ex);
               ThrowEvent(Base.Enumerations.AlertID.FAILED, ex.Message);
               return null;
           }
           finally
           {
               if (database != null)
                   database.Dispose();
           }
       }
   }
}
