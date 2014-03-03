using log4net;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CloudPanel.Modules.ActiveDirectory.Groups
{
    public class ADGroup
    {
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        // Information for connecting
        private string username;
        private string password;
        private string domainController;

        public ADGroup(string username, string password, string domainController)
        {
            this.username = username;
            this.password = password;
            this.domainController = domainController;

            this.logger.Debug(string.Format("Connecting to Active Directory groups using the following parameters: {0}, {1}", username, domainController));
        }

        /// <summary>
        /// Creates a new security group
        /// </summary>
        /// <param name="oupath"></param>
        /// <param name="groupname"></param>
        /// <param name="description"></param>
        /// <param name="isSecurityGroup"></param>
        /// <param name="isUniversal"></param>
        public void Create(string oupath, string groupname, string description, bool isSecurityGroup, bool isUniversal)
        {
            PrincipalContext pc = null;
            GroupPrincipal gp = null;

            try
            {
                // Remove all whitespaces
                groupname = groupname.Replace(" ", string.Empty);
                this.logger.Debug("Attempting to create new group named " + groupname + " on path " + oupath);

                pc = new PrincipalContext(ContextType.Domain, this.domainController, oupath, this.username, this.password);
                gp = GroupPrincipal.FindByIdentity(pc, IdentityType.Name, groupname);
                if (gp == null)
                {
                    this.logger.Debug("Group " + groupname + " does not exist... so we can continue...");

                    gp = new GroupPrincipal(pc, groupname);
                    gp.IsSecurityGroup = isSecurityGroup;
                    gp.GroupScope = isUniversal ? GroupScope.Universal : GroupScope.Global;

                    gp.Save();
                    this.logger.Info("Successfully created new group " + groupname);
                }
                else
                    throw new Exception("Group " + groupname + " already exists. Please delete this group before continuing.");
            }
            catch (Exception ex)
            {
                this.logger.Error("Error creating new group " + groupname, ex);

                throw;
            }
            finally
            {
                if (gp != null)
                    gp.Dispose();

                if (pc != null)
                    pc.Dispose();
            }
        }

        /// <summary>
        /// Deletes a group from Active Directory
        /// </summary>
        /// <param name="groupname"></param>
        public void Delete(string groupname)
        {
            PrincipalContext pc = null;
            GroupPrincipal gp = null;

            try
            {
                // Remove all whitespaces
                groupname = groupname.Replace(" ", string.Empty);

                this.logger.Debug("Deleting group " + groupname);

                pc = new PrincipalContext(ContextType.Domain, this.domainController, this.username, this.password);
                gp = GroupPrincipal.FindByIdentity(pc, IdentityType.Name, groupname);

                if (gp != null)
                {
                    gp.Delete();
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (gp != null)
                    gp.Dispose();

                if (pc != null)
                    pc.Dispose();
            }
        }

        /// <summary>
        /// Adds a member to a group
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="objectToAdd"></param>
        public void AddMember(string groupName, string objectToAdd, string objectIdentityType)
        {
            PrincipalContext pc = null;
            GroupPrincipal gp = null;

            try
            {
                // Replace whitespaces
                groupName = groupName.Replace(" ", string.Empty);
                objectToAdd = objectToAdd.Replace(" ", string.Empty);

                this.logger.Debug("Attempting to add " + objectToAdd + " to group " + groupName);

                pc = new PrincipalContext(ContextType.Domain, this.domainController, this.username, this.password);
                gp = GroupPrincipal.FindByIdentity(pc, IdentityType.Name, groupName);
                if (gp != null)
                {
                    this.logger.Debug("Checking to see if " + groupName + " already contains " + objectToAdd + " as a member.");
                    bool isMember = gp.Members.Contains(pc, GetIdentityType(objectIdentityType), objectToAdd);

                    if (!isMember)
                    {
                        gp.Members.Add(pc, GetIdentityType(objectIdentityType), objectToAdd);
                        gp.Save();

                        this.logger.Info("Successfully added " + objectToAdd + " to group " + groupName);
                    }
                    else
                        this.logger.Debug("Object " + objectToAdd + " is already a member of group " + groupName);
                }
                else
                    throw new Exception("Unable to find the group " + groupName + " in the system.");

            }
            catch (Exception ex)
            {
                this.logger.Error("Error adding member " + objectToAdd + " to group " + groupName, ex);

                throw;
            }
            finally
            {
                if (pc != null)
                    pc.Dispose();

                if (gp != null)
                    gp.Dispose();
            }
        }

        /// <summary>
        /// Gets the identity type from a string
        /// </summary>
        /// <param name="identityType"></param>
        /// <returns></returns>
        private IdentityType GetIdentityType(string identityType)
        {
            switch (identityType.ToLower().Trim())
            {
                case "userprincipalname":
                case "upn":
                    return IdentityType.UserPrincipalName;
                case "name":
                    return IdentityType.Name;
                case "guid":
                    return IdentityType.Guid;
                case "distinguishedname":
                case "dn":
                    return IdentityType.DistinguishedName;
                case "samaccountname":
                    return IdentityType.SamAccountName;
                default:
                    return IdentityType.UserPrincipalName;
            }
        }
    }
}
