using CloudPanel.Modules.ActiveDirectory.Groups;
using CloudPanel.Modules.ActiveDirectory.OrganizationalUnits;
using CloudPanel.Modules.ActiveDirectory.Users;
using CloudPanel.Modules.Common.Database;
using CloudPanel.Modules.Common.Settings;
using CloudPanel.Modules.Exchange;
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

        public void NewDomain(string companyOU, string domainName)
        {
            Events.Add(new CloudPanelEvent()
                {
                    EventType = CloudPanelEventType.Add_Domain,
                    DomainName = domainName
                });
        }

        #endregion

        #region Exchange

        public void NewExchangeDomain(string domainName)
        {
            Events.Add(new CloudPanelEvent()
                {
                    EventType = CloudPanelEventType.Create_AcceptedDomain,
                    DomainName = domainName
                });
        }

        public void NewExchangeAddressList(string name)
        {
            Events.Add(new CloudPanelEvent()
            {
                EventType = CloudPanelEventType.Create_ExchangeAddressList,
                Name = name
            });
        }

        public void NewExchangeGAL(string name)
        {
            Events.Add(new CloudPanelEvent()
            {
                EventType = CloudPanelEventType.Create_GlobalAddressList,
                Name = name
            });
        }

        public void NewExchangeOAB(string name)
        {
            Events.Add(new CloudPanelEvent()
            {
                EventType = CloudPanelEventType.Create_OfflineAddressBook,
                Name = name
            });
        }

        public void NewExchangeABP(string name)
        {
            Events.Add(new CloudPanelEvent()
            {
                EventType = CloudPanelEventType.Create_AddressBookPolicy,
                Name = name
            });
        }

        public void NewExchangeGroup(string name)
        {
            Events.Add(new CloudPanelEvent()
            {
                EventType = CloudPanelEventType.Create_DistributionGroup,
                Name = name
            });
        }

        public void NewContact(string distinguishedName)
        {
            Events.Add(new CloudPanelEvent()
                {
                    EventType = CloudPanelEventType.Create_Contact,
                    DistinguishedName = distinguishedName
                });
        }

        public void NewMailbox(string userPrincipalName)
        {
            Events.Add(new CloudPanelEvent()
                {
                    EventType = CloudPanelEventType.Create_Mailbox,
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
                    case CloudPanelEventType.Create_AddressBookPolicy:
                        // Delete the address book policy from Exchange
                        Delete_AddressBookPolicy(e.Name);
                        break;
                    case CloudPanelEventType.Create_ExchangeAddressList:
                        // Delete the address list from Exchange
                        Delete_AddressList(e.Name);
                        break;
                    case CloudPanelEventType.Create_OfflineAddressBook:
                        // Delete offline address book from Exchange
                        Delete_OfflineAddressBook(e.Name);
                        break;
                    case CloudPanelEventType.Create_GlobalAddressList:
                        // Delete global address list from Exchange
                        Delete_GlobalAddressList(e.Name);
                        break;
                    case CloudPanelEventType.Create_DistributionGroup:
                        // Delete distribution group from Exchange
                        Delete_DistributionGroup(e.Name);
                        break;
                    case CloudPanelEventType.Create_AcceptedDomain:
                        // Delete the accepted domain
                        Delete_AcceptedDomain(e.DomainName);
                        break;
                    case CloudPanelEventType.Create_Contact:
                        // Delete the Exchange contact
                        Delete_Contact(e.DistinguishedName);
                        break;
                    case CloudPanelEventType.Create_Mailbox:
                        // Delete the Exchange mailbox
                        Delete_Mailbox(e.UserPrincipalName);
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

        private void Delete_Domain(string distinguishedName, string domainName)
        {
            ADOrganizationalUnit org = null;

            try
            {
                this.logger.Warn("Rolling back action... Deleting domain from " + distinguishedName);

                org = new ADOrganizationalUnit(StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.PrimaryDC);
                org.RemoveDomain(distinguishedName, domainName);

                this.logger.Warn("Successfully removed domain " + domainName + " from " + distinguishedName + " and all child OU's");
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to roll back action... Deleting domain from " + distinguishedName, ex);
            }
        }

        private void Delete_AcceptedDomain(string domainName)
        {
            ExchangePowershell powershell = null;

            try
            {
                this.logger.Warn("Rolling back action... Deleting accepted domain " + domainName);

                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                powershell.DeleteAddressList(domainName);

                this.logger.Warn("Successfully removed accepted domain " + domainName);
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to roll back action... Deleting accepted domain " + domainName, ex);
            }
            finally
            {
                if (powershell != null)
                    powershell.Dispose();
            }
        }

        private void Delete_AddressList(string name)
        {
            ExchangePowershell powershell = null;

            try
            {
                this.logger.Warn("Rolling back action... Deleting address list " + name);

                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                powershell.DeleteAddressList(name);

                this.logger.Warn("Successfully removed address list " + name);
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to roll back action... Deleting address list " + name, ex);
            }
            finally
            {
                if (powershell != null)
                    powershell.Dispose();
            }
        }

        private void Delete_GlobalAddressList(string name)
        {
            ExchangePowershell powershell = null;

            try
            {
                this.logger.Warn("Rolling back action... Deleting address list " + name);

                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                powershell.DeleteGlobalAddressList(name);

                this.logger.Warn("Successfully removed address list " + name);
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to roll back action... Deleting address list " + name, ex);
            }
            finally
            {
                if (powershell != null)
                    powershell.Dispose();
            }
        }

        private void Delete_OfflineAddressBook(string name)
        {
            ExchangePowershell powershell = null;

            try
            {
                this.logger.Warn("Rolling back action... Deleting address list " + name);

                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                powershell.DeleteOfflineAddressBook(name);

                this.logger.Warn("Successfully removed address list " + name);
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to roll back action... Deleting address list " + name, ex);
            }
            finally
            {
                if (powershell != null)
                    powershell.Dispose();
            }
        }

        private void Delete_AddressBookPolicy(string name)
        {
            ExchangePowershell powershell = null;

            try
            {
                this.logger.Warn("Rolling back action... Deleting address list " + name);

                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                powershell.DeleteAddressBookPolicy(name);

                this.logger.Warn("Successfully removed address list " + name);
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to roll back action... Deleting address list " + name, ex);
            }
            finally
            {
                if (powershell != null)
                    powershell.Dispose();
            }
        }

        private void Delete_DistributionGroup(string name)
        {
            ExchangePowershell powershell = null;

            try
            {
                this.logger.Warn("Rolling back action... Deleting distribution group " + name);

                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                powershell.DeleteDistributionGroup(name);

                this.logger.Warn("Successfully removed distribution group " + name);
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to roll back action... Deleting distribution group " + name, ex);
            }
            finally
            {
                if (powershell != null)
                    powershell.Dispose();
            }
        }

        private void Delete_Contact(string distinguishedName)
        {
            ExchangePowershell powershell = null;

            try
            {
                this.logger.Warn("Rolling back action... Deleting Exchange contact " + distinguishedName);

                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                powershell.DeleteContact(distinguishedName);

                this.logger.Warn("Successfully removed Exchange contact " + distinguishedName);
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to roll back action... Deleting Exchange contact " + distinguishedName, ex);
            }
            finally
            {
                if (powershell != null)
                    powershell.Dispose();
            }
        }

        private void Delete_Mailbox(string userPrincipalName)
        {
            ExchangePowershell powershell = null;

            try
            {
                this.logger.Warn("Rolling back action... Deleting Exchange mailbox " + userPrincipalName);

                powershell = new ExchangePowershell(StaticSettings.ExchangeURI, StaticSettings.Username, StaticSettings.DecryptedPassword, StaticSettings.ExchangeUseKerberos, StaticSettings.PrimaryDC);
                powershell.DeleteMailbox(userPrincipalName);

                this.logger.Warn("Successfully removed Exchange mailbox " + userPrincipalName);
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to roll back action... Deleting Exchange mailbox " + userPrincipalName, ex);
            }
            finally
            {
                if (powershell != null)
                    powershell.Dispose();
            }
        }

        #endregion
    }
}
