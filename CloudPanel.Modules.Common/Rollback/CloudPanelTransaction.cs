using CloudPanel.Modules.ActiveDirectory.Groups;
using CloudPanel.Modules.ActiveDirectory.OrganizationalUnits;
using CloudPanel.Modules.ActiveDirectory.Users;
using CloudPanel.Modules.Common.Database;
using CloudPanel.Modules.Common.Settings;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.Common.Rollback
{
    public class CloudPanelTransaction
    {
        private readonly List<CloudPanelEvent> Events;

        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public CloudPanelTransaction()
        {
            Events = new List<CloudPanelEvent>();
        }

        #region Active Directory

        /// <summary>
        /// Registers a new event for creating an organizational unit
        /// </summary>
        /// <param name="distinguishedName"></param>
        public void NewOrganizationalUnitEvent(string distinguishedName)
        {
            Events.Add(new CloudPanelEvent()
            {
                 EventType = CloudPanelEventType.Create_OrganizationalUnit,
                 DistinguishedName = distinguishedName
            });
        }

        /// <summary>
        /// Registers a new event for creating a security group
        /// </summary>
        /// <param name="groupName"></param>
        public void NewSecurityGroup(string groupName)
        {
            Events.Add(new CloudPanelEvent()
            {
                EventType = CloudPanelEventType.Create_SecurityGroup,
                GroupName = groupName
            });
        }

        public void InsertCompanyToDatabase(string companyCode)
        {
            Events.Add(new CloudPanelEvent()
            {
                EventType = CloudPanelEventType.Insert_Company_Into_Database,
                CompanyCode = companyCode
            });
        }

        public void NewUser(string userPrincipalName)
        {
            Events.Add(new CloudPanelEvent()
                {
                    EventType = CloudPanelEventType.Create_NewUser,
                    UserPrincipalName = userPrincipalName
                });
        }

        #endregion

        #region RollBack

        public void RollBack()
        {
            // Roll back all CloudPanelEvents that have occurred
            Events.Reverse();

            foreach (CloudPanelEvent e in Events)
            {
                switch (e.EventType)
                {
                    case CloudPanelEventType.Create_OrganizationalUnit:
                        // Delete Organizational Unit
                        Delete_OrganizationalUnit(e.DistinguishedName);
                        break;
                    case CloudPanelEventType.Create_SecurityGroup:
                        // Delete Security Group
                        Delete_Group(e.GroupName);
                        break;
                    case CloudPanelEventType.Insert_Company_Into_Database:
                        // Delete the company from the database
                        Delete_CompanyFromDatabase(e.CompanyCode);
                        break;
                    case CloudPanelEventType.Create_NewUser:
                        // Delete the user
                        Delete_UserFromAD(e.UserPrincipalName);
                        break;
                    default:
                        break;
                }
            }
        }

        #endregion

        #region Roll Back Actions

        private void Delete_OrganizationalUnit(string distinguishedName)
        {
            ADOrganizationalUnit org = null;

            try
            {
                this.logger.Warn("Rolling back action... Deleting organizational unit " + distinguishedName);

                org = new ADOrganizationalUnit(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                org.DeleteOU(distinguishedName);

                this.logger.Warn("Successfully deleted " + distinguishedName);
            }
            catch(Exception ex)
            {
                this.logger.Error("Failed to roll back action... Deleting organizational unit " + distinguishedName, ex);
            }
        }

        private void Delete_Group(string groupname)
        {
            ADGroup group = null;

            try
            {
                this.logger.Warn("Rolling back action... Deleting group " + groupname);

                group = new ADGroup(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                group.Delete(groupname);

                this.logger.Warn("Successfully deleted " + groupname);
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to roll back action... Deleting group " + groupname, ex);
            }
        }

        private void Delete_CompanyFromDatabase(string companyCode)
        {
            CPDatabase database = null;
            
            try
            {
                database = new CPDatabase();

                var foundCompany = (from c in database.Companies
                                    where !c.IsReseller
                                    where c.CompanyCode == companyCode
                                    select c).FirstOrDefault();

                if (foundCompany != null)
                {
                    database.Companies.Remove(foundCompany);
                    database.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to roll back action... Deleting company from database " + companyCode, ex);
            }
            finally
            {
                if (database != null)
                    database.Dispose();
            }
        }

        private void Delete_UserFromAD(string userPrincipalName)
        {
            ADUser user = null;

            try
            {
                user = new ADUser(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                user.DeleteUser(userPrincipalName);
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to roll back action... Deleting user from Active Directory " + userPrincipalName, ex);
            }
            finally
            {
                if (user != null)
                    user.Dispose();
            }
        }

        #endregion
    }
}
