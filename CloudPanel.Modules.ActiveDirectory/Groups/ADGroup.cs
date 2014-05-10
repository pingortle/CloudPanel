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

        // Disposing information
        private bool disposed = false;

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
        /// Removes a member from a group
        /// </summary>
        /// <param name="groupName"></param>
        /// <param name="objectToRemove"></param>
        /// <param name="objectIdentityType"></param>
        public void RemoveMember(string groupName, string objectToRemove, string objectIdentityType)
        {
            PrincipalContext pc = null;
            GroupPrincipal gp = null;

            try
            {
                // Replace whitespaces
                groupName = groupName.Replace(" ", string.Empty);
                objectToRemove = objectToRemove.Replace(" ", string.Empty);

                this.logger.Debug("Attempting to remove " + objectToRemove + " from group " + groupName);

                pc = new PrincipalContext(ContextType.Domain, this.domainController, this.username, this.password);
                gp = GroupPrincipal.FindByIdentity(pc, IdentityType.Name, groupName);
                if (gp != null)
                {
                    this.logger.Debug("Checking to see if " + groupName + " already contains " + objectToRemove + " as a member.");
                    bool isMember = gp.Members.Contains(pc, GetIdentityType(objectIdentityType), objectToRemove);

                    if (isMember)
                    {
                        gp.Members.Remove(pc, GetIdentityType(objectIdentityType), objectToRemove);
                        gp.Save();

                        this.logger.Info("Successfully removed " + objectToRemove + " from group " + groupName);
                    }
                    else
                        this.logger.Debug("Object " + objectToRemove + " is already not a member of group " + groupName);
                }
                else
                    throw new Exception("Unable to find the group " + groupName + " in the system.");

            }
            catch (Exception ex)
            {
                this.logger.Error("Error removing member " + objectToRemove + " to group " + groupName, ex);

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

        #region Dispose
        /// <summary>
        /// Disposing
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    username = null;
                    password = null;
                    domainController = null;
                }

                disposed = true;
            }
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
