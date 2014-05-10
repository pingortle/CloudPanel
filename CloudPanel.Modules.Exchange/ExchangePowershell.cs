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

using CloudPanel.Modules.Base.Enumerations;
using CloudPanel.Modules.Base.Plans;
using CloudPanel.Modules.Base.Users;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace CloudPanel.Modules.Exchange
{
    public class ExchangePowershell
    {
        // Disposing information
        private bool disposed = false;

        // Our connection information to the Exchange server
        private WSManConnectionInfo wsConn;

        // Pipeline Information
        Runspace runspace = null;

        // Powershell object to run commands
        PowerShell powershell = null;

        // Domain Controller to communciate with
        private string domainController = string.Empty;

        // Logger
        private readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Start new object
        /// </summary>
        /// <param name="uri">Uri to Exchange server powershell directory</param>
        /// <param name="username">Username to connect</param>
        /// <param name="password">Password to connect</param>
        /// <param name="kerberos">True to use Kerberos authentication, false to use Basic authentication</param>
        /// <param name="domainController">Domain controller to communicate with</param>
        public ExchangePowershell(string uri, string username, string password, bool kerberos, string domainController)
        {
            this.wsConn = GetConnection(uri, username, password, kerberos);
            this.domainController = domainController;

            // Create our runspace
            runspace = RunspaceFactory.CreateRunspace(wsConn);

            // Open the connection
            runspace.Open();

            powershell = PowerShell.Create();
            powershell.Runspace = runspace;
        }

        #region New Actions

        public void NewDomain(string domainName, DomainType domainType)
        {
            try
            {
                PSCommand cmd = new PSCommand();
                cmd.AddCommand("New-AcceptedDomain");
                cmd.AddParameter("Name", domainName);
                cmd.AddParameter("DomainName", domainName);
                cmd.AddParameter("DomainController", domainController);

                switch (domainType)
                {
                    case DomainType.InternalRelayDomain:
                        cmd.AddParameter("DomainType", "InternalRelay");
                        break;
                    case DomainType.ExternalRelayDomain:
                        cmd.AddParameter("DomainType", "ExternalRelay");
                        break;
                    default:
                        cmd.AddParameter("DomainType", "Authoritative");
                        break;
                }

                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed enable new accepted domain " + domainName, ex);
                throw;
            }
        }

        public void NewOfflineAddressBook(string name, string gal, string groupAllowedToDownloadOAB)
        {
            try
            {
                PSCommand cmd = new PSCommand();
                cmd.AddCommand("New-OfflineAddressBook");
                cmd.AddParameter("Name", name);
                cmd.AddParameter("AddressLists", gal);
                cmd.AddParameter("DomainController", domainController);
                cmd.AddCommand("Add-ADPermission");
                cmd.AddParameter("User", groupAllowedToDownloadOAB.Replace(" ", string.Empty));
                cmd.AddParameter("ExtendedRights", "MS-EXCH-DOWNLOAD-OAB");
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to create new offline address book " + name + " and add rights for " + groupAllowedToDownloadOAB, ex);
                throw;
            }
        }

        public void NewAddressBookPolicy(string name, string gal, string oab, string room, string[] addressLists)
        {
            try
            {
                PSCommand cmd = new PSCommand();
                cmd.AddCommand("New-AddressBookPolicy");
                cmd.AddParameter("Name", name);
                cmd.AddParameter("AddressLists", addressLists);
                cmd.AddParameter("GlobalAddressList", gal);
                cmd.AddParameter("OfflineAddressBook", oab);
                cmd.AddParameter("RoomList", room);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error(string.Format("Failed to create new address book policy named {0} with GAL {1}, OAB {2}, room list {3} and the following address lists: {4}", name, gal, oab, room, String.Join(",", addressLists)), ex);
                throw;
            }
        }

        public void NewAddressList(string name, string recipientFilter)
        {
            try
            {
                PSCommand cmd = new PSCommand();
                cmd.AddCommand("New-AddressList");
                cmd.AddParameter("Name", name);
                cmd.AddParameter("RecipientFilter", recipientFilter);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to create new address list " + name + " with filter " + recipientFilter, ex);
                throw;
            }
        }

        public void NewGlobalAddressList(string name, string recipientFilter)
        {
            try
            {
                PSCommand cmd = new PSCommand();
                cmd.AddCommand("New-GlobalAddressList");
                cmd.AddParameter("Name", name);
                cmd.AddParameter("RecipientFilter", recipientFilter);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to create new global address list named " + name + " with filter " + recipientFilter, ex);
                throw;
            }
        }

        public void NewSecurityDistributionGroup(string name, string memberGroup, string customAttribute, string organizationalUnit)
        {
            try
            {
                this.logger.Debug("Creating new security distribution group " + name + " with member " + memberGroup + " and custom attribute " + customAttribute + " in path " + organizationalUnit);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("New-DistributionGroup");
                cmd.AddParameter("Name", name);
                cmd.AddParameter("DisplayName", name);
                cmd.AddParameter("PrimarySmtpAddress", name);
                cmd.AddParameter("OrganizationalUnit", organizationalUnit);
                cmd.AddParameter("Members", memberGroup);
                cmd.AddParameter("Type", "Security");
                cmd.AddParameter("DomainController", this.domainController);
                cmd.AddCommand("Set-DistributionGroup");
                cmd.AddParameter("BypassSecurityGroupManagerCheck");
                cmd.AddParameter("CustomAttribute1", customAttribute);
                cmd.AddParameter("HiddenFromAddressListsEnabled", true);
                cmd.AddParameter("DomainController", this.domainController);

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to create new security distribution group " + name, ex);
                throw;
            }
        }

        public string NewContact(string displayName, string email, bool isHidden, string companyCode, string organizationalUnit)
        {
            try
            {
                this.logger.Debug("Creating new Exchange contact named " + displayName + " with email address " + email);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("New-MailContact");
                //cmd.AddParameter("Alias", email);
                cmd.AddParameter("Name", displayName);
                cmd.AddParameter("PrimarySmtpAddress", string.Format("{0}@{1}", Guid.NewGuid(), email.Split('@')[1]));
                cmd.AddParameter("ExternalEmailAddress", email);
                cmd.AddParameter("OrganizationalUnit", organizationalUnit);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;

                Collection<PSObject> obj = powershell.Invoke();
                if (!powershell.HadErrors)
                {
                    // Get the distinguished name
                    string distinguishedName = "";
                    foreach (PSObject o in obj)
                    {
                        if (o.Properties["DistinguishedName"] != null)
                            distinguishedName = o.Properties["DistinguishedName"].Value.ToString();
                    }

                    cmd = new PSCommand();
                    cmd.AddCommand("Set-MailContact");
                    cmd.AddParameter("Identity", distinguishedName);
                    cmd.AddParameter("CustomAttribute1", companyCode);
                    cmd.AddParameter("HiddenFromAddressListsEnabled", isHidden);
                    cmd.AddParameter("EmailAddressPolicyEnabled", false);
                    cmd.AddParameter("DomainController", domainController);
                    powershell.Commands = cmd;
                    powershell.Invoke();

                    return distinguishedName;
                }
                else
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to create new Exchange contact " + displayName, ex);
                throw;
            }
        }

        public void NewMailbox(UsersObject user)
        {
            try
            {
                this.logger.Debug("Creating new mailbox for "+ user.UserPrincipalName);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Enable-Mailbox");
                cmd.AddParameter("Identity", user.UserPrincipalName);
                cmd.AddParameter("PrimarySmtpAddress", user.PrimarySmtpAddress);
                cmd.AddParameter("AddressBookPolicy", user.CompanyCode + " ABP");

                this.logger.Debug("Checking activesync policy for " + user.UserPrincipalName);
                if (!string.IsNullOrEmpty(user.ActiveSyncName))
                    cmd.AddParameter("ActiveSyncMailboxPolicy", user.ActiveSyncName);

                this.logger.Debug("Checking if we are putting this new mailbox in a specific database " + user.UserPrincipalName);
                if (!string.IsNullOrEmpty(user.CurrentMailboxDatabase))
                    cmd.AddParameter("Database", user.CurrentMailboxDatabase);

                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;

                this.logger.Debug("Invoking powershell to create new mailbox for " + user.UserPrincipalName);
                powershell.Invoke();
                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to create new mailbox for " + user.UserPrincipalName, ex);
                throw;
            }
        }

        public void NewArchiveMailbox(UsersObject user)
        {
            try
            {
                this.logger.Debug("Creating new archive mailbox for " + user.UserPrincipalName);

                if (user.ArchivingEnabled && user.ArchivePlan > 0)
                {
                    PSCommand cmd = new PSCommand();
                    cmd.AddCommand("Enable-Mailbox");
                    cmd.AddParameter("Identity", user.UserPrincipalName);
                    cmd.AddParameter("Archive");

                    if (!string.IsNullOrEmpty(user.ArchiveDatabase))
                        cmd.AddParameter("ArchiveDatabase", user.ArchiveDatabase);

                    if (!string.IsNullOrEmpty(user.ArchiveName))
                        cmd.AddParameter("ArchiveName", user.ArchiveName);

                    cmd.AddParameter("Confirm", false);
                    cmd.AddParameter("DomainController", domainController);
                    powershell.Commands = cmd;

                    Collection<PSObject> obj = powershell.Invoke();
                    if (powershell.HadErrors)
                        throw powershell.Streams.Error[0].Exception;
                }
                else
                    this.logger.Debug("Unable to create archive mailbox because the plan was not set for " + user.UserPrincipalName);
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to create new archive mailbox for " + user.UserPrincipalName, ex);
                throw;
            }
        }

        public void NewLitigationHold(string userPrincipalName, string comment, string url, int? days)
        {
            try
            {
                this.logger.Debug("Enabling litigation hold for " + userPrincipalName);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Set-Mailbox");
                cmd.AddParameter("Identity", userPrincipalName);
                cmd.AddParameter("LitigationHoldEnabled", true);

                if (!string.IsNullOrEmpty(comment))
                    cmd.AddParameter("RetentionComment", comment);
                else
                    cmd.AddParameter("RetentionComment", null);

                if (!string.IsNullOrEmpty(url))
                    cmd.AddParameter("RetentionUrl", url);
                else
                    cmd.AddParameter("RetentionUrl", null);

                if (days != null)
                    cmd.AddParameter("LitigationHoldDuration", days);
                else
                    cmd.AddParameter("LitigationHoldDuration", null);

                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("Force");
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;

                Collection<PSObject> obj = powershell.Invoke();
                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to enable litigation hold for " + userPrincipalName, ex);
                throw;
            }
        }

        #endregion

        #region Update Actions

        public void UpdateMailbox(UsersObject user, MailboxPlanObject mailboxPlan)
        {
            try
            {
                this.logger.Debug("Updating mailbox for " + user.UserPrincipalName);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Set-Mailbox");
                cmd.AddParameter("Identity", user.UserPrincipalName);
                cmd.AddParameter("CustomAttribute1", user.CompanyCode);
                cmd.AddParameter("DeliverToMailboxAndForward", user.DeliverToMailboxAndForward);
                cmd.AddParameter("HiddenFromAddressListsEnabled", user.MailboxHiddenFromGAL);
                cmd.AddParameter("IssueWarningQuota", mailboxPlan.WarningSizeInMB(user.SetMailboxSizeInMB));
                cmd.AddParameter("ProhibitSendQuota", user.SetMailboxSizeInMB + "MB");
                cmd.AddParameter("ProhibitSendReceiveQuota", user.SetMailboxSizeInMB + "MB");
                cmd.AddParameter("MaxReceiveSize", mailboxPlan.MaxReceiveInKB + "KB");
                cmd.AddParameter("MaxSendSize", mailboxPlan.MaxSendInKB + "KB");
                cmd.AddParameter("RecipientLimits", mailboxPlan.MaxRecipients);
                cmd.AddParameter("RetainDeletedItemsFor", mailboxPlan.MaxKeepDeletedItemsInDays);
                cmd.AddParameter("RetainDeletedItemsUntilBackup", true);
                cmd.AddParameter("UseDatabaseQuotaDefaults", false);
                cmd.AddParameter("OfflineAddressBook", user.CompanyCode + " OAL");

                List<string> emailAddresses = new List<string>();
                emailAddresses.Add("SMTP:" + user.PrimarySmtpAddress);
                foreach (string e in user.EmailAliases)
                {
                    if (e.StartsWith("sip:", StringComparison.CurrentCultureIgnoreCase))
                        emailAddresses.Add(e);
                    else if (e.StartsWith("x500:", StringComparison.CurrentCultureIgnoreCase))
                        emailAddresses.Add(e);
                    else if (e.StartsWith("x400:", StringComparison.CurrentCultureIgnoreCase))
                        emailAddresses.Add(e);
                    else
                        emailAddresses.Add("smtp:" + e);
                }
                cmd.AddParameter("EmailAddresses", emailAddresses.ToArray());

                if (!string.IsNullOrEmpty(user.ForwardingTo))
                    cmd.AddParameter("ForwardingAddress", user.ForwardingTo);

                if (!string.IsNullOrEmpty(user.ThrottlingPolicy))
                    cmd.AddParameter("ThrottlingPolicy", user.ThrottlingPolicy);


                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;

                Collection<PSObject> obj = powershell.Invoke();
                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to update mailbox for " + user.UserPrincipalName, ex);
                throw;
            }
        }

        public void UpdateCASMailbox(UsersObject user, MailboxPlanObject mailboxPlan)
        {
            try
            {
                this.logger.Debug("Updating CAS mailbox for " + user.UserPrincipalName);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Set-CASMailbox");
                cmd.AddParameter("Identity", user.UserPrincipalName);
                cmd.AddParameter("ActiveSyncEnabled", mailboxPlan.EnableAS);
                cmd.AddParameter("ECPEnabled", mailboxPlan.EnableECP);
                cmd.AddParameter("ImapEnabled", mailboxPlan.EnableIMAP);
                cmd.AddParameter("MAPIEnabled", mailboxPlan.EnableMAPI);
                cmd.AddParameter("OWAEnabled", mailboxPlan.EnableOWA);
                cmd.AddParameter("PopEnabled", mailboxPlan.EnablePOP3);

                // Email Addresses

                if (!string.IsNullOrEmpty(user.ActiveSyncName))
                    cmd.AddParameter("ActiveSyncMailboxPolicy", user.ActiveSyncName);
                else
                    cmd.AddParameter("ActiveSyncMailboxPolicy", null);


                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;

                Collection<PSObject> obj = powershell.Invoke();
                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to set CAS mailbox for " + user.UserPrincipalName, ex);
                throw;
            }
        }

        public void UpdateLitigationHold(string userPrincipalName, string comment, string url, int? days)
        {
            try
            {
                this.logger.Debug("Updating litigation hold for " + userPrincipalName);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Set-Mailbox");
                cmd.AddParameter("Identity", userPrincipalName);

                if (!string.IsNullOrEmpty(comment))
                    cmd.AddParameter("RetentionComment", comment);
                else
                    cmd.AddParameter("RetentionComment", null);

                if (!string.IsNullOrEmpty(url))
                    cmd.AddParameter("RetentionUrl", url);
                else
                    cmd.AddParameter("RetentionUrl", null);

                if (days != null)
                    cmd.AddParameter("LitigationHoldDuration", days);
                else
                    cmd.AddParameter("LitigationHoldDuration", null);

                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;

                Collection<PSObject> obj = powershell.Invoke();
                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to set litigation hold settings for " + userPrincipalName, ex);
                throw;
            }
        }

        public void UpdateArchiveMailbox(string userPrincipalName, string archiveName, ArchivePlanObject archivePlan)
        {
            try
            {
                this.logger.Debug("Updating archive mailbox for " + userPrincipalName);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Set-Mailbox");
                cmd.AddParameter("Identity", userPrincipalName);
                cmd.AddParameter("ArchiveQuota", archivePlan.ArchiveQuotaInMB + "MB");
                cmd.AddParameter("ArchiveWarningQuota", archivePlan.ArchiveWarningQuotaInMB + "MB");

                if (!string.IsNullOrEmpty(archiveName))
                    cmd.AddParameter("ArchiveName", archiveName);

                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;

                Collection<PSObject> obj = powershell.Invoke();
                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to update archive mailbox for " + userPrincipalName, ex);
                throw;
            }
        }

        public void UpdateDomain(string domainName, DomainType domainType)
        {
            try
            {
                this.logger.Debug("Updating accepted domain " + domainName + " to " + domainType.ToString());

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Set-AcceptedDomain");
                cmd.AddParameter("Identity", domainName);
                cmd.AddParameter("DomainController", domainController);

                switch (domainType)
                {
                    case DomainType.InternalRelayDomain:
                        cmd.AddParameter("DomainType", "InternalRelay");
                        break;
                    case DomainType.ExternalRelayDomain:
                        cmd.AddParameter("DomainType", "ExternalRelay");
                        break;
                    default:
                        cmd.AddParameter("DomainType", "Authoritative");
                        break;
                }

                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to update accepted domain " + domainName, ex);
                throw;
            }
        }

        public string UpdateContact(string distinguishedName, string newDisplayName, bool isHidden)
        {
            try
            {
                this.logger.Debug("Updating contact " + distinguishedName);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Set-MailContact");
                cmd.AddParameter("Identity", distinguishedName);
                cmd.AddParameter("DisplayName", newDisplayName);
                cmd.AddParameter("HiddenFromAddressListsEnabled", isHidden);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;

                Collection<PSObject> obj = powershell.Invoke();
                if (!powershell.HadErrors)
                {
                    // Get the distinguished name
                    string newDistinguishedName = distinguishedName;
                    foreach (PSObject o in obj)
                    {
                        if (o.Properties["DistinguishedName"] != null)
                            newDistinguishedName = o.Properties["DistinguishedName"].Value.ToString();
                    }

                    return distinguishedName;
                }
                else
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to update Exchange contact " + distinguishedName, ex);
                throw;
            }
        }

        #endregion

        #region Add Actions

        public void AddGroupMember(string groupEmailAddress, string identity)
        {
            try
            {
                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Add-DistributionGroupMember");
                cmd.AddParameter("Identity", groupEmailAddress);
                cmd.AddParameter("Member", identity);
                cmd.AddParameter("BypassSecurityGroupManagerCheck");
                cmd.AddParameter("DomainController", this.domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                {
                    ErrorCategory errCategory = powershell.Streams.Error[0].CategoryInfo.Category;
                    string reason = powershell.Streams.Error[0].CategoryInfo.Reason;

                    if (errCategory != ErrorCategory.NotSpecified && !reason.Equals("MemberAlreadyExistsException"))
                        throw powershell.Streams.Error[0].Exception;
                    else
                        this.logger.Info("Failed to add "+ identity + " to group " + groupEmailAddress + " because they it was already member.");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to add " + identity + " to group " + groupEmailAddress, ex);
                throw;
            }
        }

        #endregion

        #region Get Actions

        public UsersObject GetUser(string identity, int exchangeVersion)
        {
            try
            {
                UsersObject user = new UsersObject();

                //                         //
                // Get mailbox information //
                //                         //
                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Get-Mailbox");
                cmd.AddParameter("Identity", identity);
                cmd.AddParameter("DomainController", this.domainController);
                powershell.Commands = cmd;

                Collection<PSObject> obj = powershell.Invoke();
                if (obj != null && obj.Count > 0)
                {
                    foreach (PSObject ps in obj)
                    {
                        this.logger.Debug("Pulling comment information for mailbox " + identity);

                        if (ps.Members["Database"].Value != null)
                            user.CurrentMailboxDatabase = ps.Members["Database"].Value.ToString();

                        if (ps.Members["DeliverToMailboxAndForward"].Value != null)
                            user.DeliverToMailboxAndForward = bool.Parse(ps.Members["DeliverToMailboxAndForward"].Value.ToString());

                        if (ps.Members["LitigationHoldEnabled"].Value != null)
                            user.LitigationHoldEnabled = bool.Parse(ps.Members["LitigationHoldEnabled"].Value.ToString());

                        if (ps.Members["LitigationHoldDuration"].Value != null)
                        {
                            string value = ps.Members["LitigationHoldDuration"].Value.ToString();
                            if (!value.Equals("Unlimited", StringComparison.CurrentCultureIgnoreCase))
                            {
                                TimeSpan span = TimeSpan.Parse(value);
                                user.LitigationHoldDuration = span.Days;
                            }
                        }

                        if (ps.Members["HiddenFromAddressListsEnabled"].Value != null)
                            user.MailboxHiddenFromGAL = bool.Parse(ps.Members["HiddenFromAddressListsEnabled"].Value.ToString());

                        if (ps.Members["RetentionComment"].Value != null)
                            user.LitigationHoldComment = ps.Members["RetentionComment"].Value.ToString();

                        if (ps.Members["RetentionUrl"].Value != null)
                            user.LitigationHoldUrl = ps.Members["RetentionUrl"].Value.ToString();

                        if (ps.Members["ForwardingAddress"].Value != null)
                            user.ForwardingTo = ps.Members["ForwardingAddress"].Value.ToString();

                        if (ps.Members["SamAccountName"].Value != null)
                            user.sAMAccountName = ps.Members["SamAccountName"].Value.ToString();

                        if (ps.Members["UserPrincipalName"].Value != null)
                            user.UserPrincipalName = ps.Members["UserPrincipalName"].Value.ToString();

                        if (ps.Members["ThrottlingPolicy"].Value != null)
                            user.ThrottlingPolicy = ps.Members["ThrottlingPolicy"].Value.ToString();

                        if (ps.Members["Alias"].Value != null)
                            user.ExchangeAlias = ps.Members["Alias"].Value.ToString();

                        if (ps.Members["PrimarySmtpAddress"].Value != null)
                        user.PrimarySmtpAddress = ps.Members["PrimarySmtpAddress"].Value.ToString();

                        if (ps.Members["DistinguishedName"].Value != null)
                            user.DistinguishedName = ps.Members["DistinguishedName"].Value.ToString();

                        if (ps.Members["ArchiveDatabase"].Value != null)
                            user.ArchiveDatabase = ps.Members["ArchiveDatabase"].Value.ToString();

                        if (ps.Members["ArchiveName"].Value != null)
                            user.ArchiveName = ps.Members["ArchiveName"].Value.ToString();

                        if (exchangeVersion == 2013)
                            user.HasExchangePicture = bool.Parse(ps.Members["HasPicture"].Value.ToString());

                        // Get email aliases
                        this.logger.Debug("Pulling email aliases for " + identity);

                        user.EmailAliases = new List<string>();
                        if (ps.Members["EmailAddresses"].Value != null)
                        {
                            PSObject multiValue = (PSObject)ps.Members["EmailAddresses"].Value;
                            ArrayList addresses = (ArrayList)multiValue.BaseObject;

                            // Convert to string array
                            string[] stringEmailAddresses = (string[])addresses.ToArray(typeof(string));

                            foreach (string s in stringEmailAddresses)
                            {
                                if (!s.StartsWith("SMTP:"))
                                {
                                    user.EmailAliases.Add(s.Replace("smtp:", string.Empty));
                                }
                            }
                        }
                    }
                }

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;

                //                              //
                // Get full access permissions  //
                //                              //
                this.logger.Debug("Getting full access permissions for " + identity);
                user.FullAccessUsers = new List<string>();

                cmd = new PSCommand();
                cmd.AddCommand("Get-MailboxPermission");
                cmd.AddParameter("Identity", identity);
                cmd.AddParameter("DomainController", this.domainController);
                powershell.Commands = cmd;

                obj = powershell.Invoke();
                if (obj != null && obj.Count > 0)
                {
                    foreach (PSObject ps in obj)
                    {
                        // Only get users with a \ in them. Otherwise they are not users
                        if (ps.Members["User"].Value.ToString().Contains("\\"))
                        {
                            this.logger.Debug("Found a user that has a permission to mailbox " + identity + "... now checking if it is deny permission or allow permission");

                            // We don't care about the deny permissions
                            if (!bool.Parse(ps.Members["Deny"].Value.ToString()))
                            {
                                this.logger.Debug("Found a user that has a permission to mailbox " + identity + ": " + ps.Members["User"].Value.ToString());

                                // Get the permissions that this user has
                                PSObject multiValue = (PSObject)ps.Members["AccessRights"].Value;
                                ArrayList accessRights = (ArrayList)multiValue.BaseObject;

                                // Convert to string array
                                string[] stringAccessRights = (string[])accessRights.ToArray(typeof(string));

                                // Fix the sAMAccountName by removing the domain name
                                string sAMAccountName = ps.Members["User"].Value.ToString().Split('\\')[1];

                                // Check if it is full access or send as
                                if (stringAccessRights.Contains("FullAccess"))
                                    user.FullAccessUsers.Add(sAMAccountName);
                            }
                        }
                    }
                }

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;

                //                         //
                // Get send-as permissions //
                //                         //
                this.logger.Debug("Getting send-as permissions for " + identity);
                user.SendAsUsers = new List<string>();

                cmd = new PSCommand();
                cmd.AddCommand("Get-ADPermission");
                cmd.AddParameter("Identity", identity);
                cmd.AddParameter("DomainController", this.domainController);
                powershell.Commands = cmd;

                obj = powershell.Invoke();
                if (obj != null && obj.Count > 0)
                {
                    foreach (PSObject ps in obj)
                    {
                        if (ps.Members["ExtendedRights"].Value != null)
                        {
                            // Only get users with a \ in them. Otherwise they are not users
                            if (ps.Members["User"].Value.ToString().Contains("\\"))
                            {
                                this.logger.Debug("Found a user that has an extended rights permission to mailbox " + identity + "... now checking if it is deny permission or allow permission");

                                // We don't care about the deny permissions
                                if (!bool.Parse(ps.Members["Deny"].Value.ToString()))
                                {
                                    this.logger.Debug("Found a user that has an extended rights permission to mailbox " + identity + ": " + ps.Members["User"].Value.ToString());

                                    // Get the permissions that this user has
                                    PSObject multiValue = (PSObject)ps.Members["ExtendedRights"].Value;
                                    ArrayList accessRights = (ArrayList)multiValue.BaseObject;

                                    // Convert to string array
                                    string[] stringAccessRights = (string[])accessRights.ToArray(typeof(string));

                                    // Fix the sAMAccountName by removing the domain name
                                    string sAMAccountName = ps.Members["User"].Value.ToString().Split('\\')[1];

                                    // Check if it is full access or send as
                                    if (stringAccessRights.Contains("Send-As"))
                                        user.SendAsUsers.Add(sAMAccountName);
                                }
                            }
                        }
                    }
                }

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;

                //                         //
                // Get send-as permissions //
                //                         //
                this.logger.Debug("Getting send on behalf permissions for " + identity);
                user.SendOnBehalf = new List<string>();

                return user;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to get mailbox information for " + identity, ex);
                throw;
            }
        }

        private string GetCalendarName(string userPrincipalName)
        {
            try
            {
                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Get-MailboxFolderStatistics");
                cmd.AddParameter("Identity", userPrincipalName);
                cmd.AddParameter("FolderScope", "Calendar");
                cmd.AddParameter("DomainController", this.domainController);
                powershell.Commands = cmd;

                // Default calendar name in English
                string calendarName = "Calendar";

                // Find the folder type "Calendar" so we can get the name of the calender (because of different languages)
                Collection<PSObject> obj = powershell.Invoke();
                if (obj != null && obj.Count > 0)
                {
                    foreach (PSObject ps in obj)
                    {
                        if (ps.Members["FolderType"] != null)
                        {
                            string folderType = ps.Members["FolderType"].Value.ToString();

                            if (folderType.Equals("Calendar"))
                            {
                                calendarName = ps.Members["Name"].Value.ToString();
                                break;
                            }
                        }
                    }
                }

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;

                return calendarName;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to get calendar name for " + userPrincipalName, ex);
                throw;
            }
        }

        #endregion

        #region Remove Actions

        public void RemoveGroupMember(string groupEmailAddress, string identity)
        {
            try
            {
                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Remove-DistributionGroupMember");
                cmd.AddParameter("Identity", groupEmailAddress);
                cmd.AddParameter("Member", identity);
                cmd.AddParameter("BypassSecurityGroupManagerCheck");
                cmd.AddParameter("DomainController", this.domainController);
                cmd.AddParameter("Confirm", false);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                {
                    ErrorCategory errCategory = powershell.Streams.Error[0].CategoryInfo.Category;
                    string reason = powershell.Streams.Error[0].CategoryInfo.Reason;

                    if (errCategory != ErrorCategory.NotSpecified && !reason.Equals("ManagementObjectNotFoundException"))
                        throw powershell.Streams.Error[0].Exception;
                    else
                        this.logger.Info("Failed to remove " + identity + " from group " + groupEmailAddress + " because they were not a member or the group could not be found");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to remove " + identity + " from group " + groupEmailAddress, ex);
                throw;
            }
        }

        #endregion

        #region Delete Actions

        public void DeleteMailbox(string userPrincipalName)
        {
            try
            {
                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Disable-Mailbox");
                cmd.AddParameter("Identity", userPrincipalName);
                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                {
                    ErrorCategory errCategory = powershell.Streams.Error[0].CategoryInfo.Category;
                    string reason = powershell.Streams.Error[0].CategoryInfo.Reason;

                    if (errCategory != ErrorCategory.NotSpecified && !reason.Equals("ManagementObjectNotFoundException"))
                        throw powershell.Streams.Error[0].Exception;
                    else
                        this.logger.Info("Attempted to disable mailbox " + userPrincipalName + " but it could not be found. Must have already been disabled.");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to disable mailbox " + userPrincipalName, ex);
                throw;
            }
        }

        public void DeleteArchiveMailbox(string userPrincipalName)
        {
            try
            {
                this.logger.Debug("Disabling archive mailbox for " + userPrincipalName);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Disable-Mailbox");
                cmd.AddParameter("Identity", userPrincipalName);
                cmd.AddParameter("Archive");
                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to disable archive mailbox " + userPrincipalName, ex);
                throw;
            }
        }

        public void DeleteLitigationHold(string userPrincipalName)
        {
            try
            {
                this.logger.Debug("Disabling litigation hold for " + userPrincipalName);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Set-Mailbox");
                cmd.AddParameter("Identity", userPrincipalName);
                cmd.AddParameter("LitigationHoldEnabled", false);
                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to disable litigation hold for" + userPrincipalName, ex);
                throw;
            }
        }

        public void DeleteAllMailboxes(string companyCode)
        {
            try
            {
                this.logger.Info("Disabling all mailboxes for " + companyCode);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Get-Mailbox");
                cmd.AddParameter("Filter", string.Format("CustomAttribute1 -eq '{0}'", companyCode));
                cmd.AddParameter("DomainController", domainController);
                cmd.AddCommand("Disable-Mailbox");
                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to disable all mailboxes for company " + companyCode, ex);
                throw;
            }
        }

        public void DeleteAllContacts(string companyCode)
        {
            try
            {
                this.logger.Info("Deleting all contacts for " + companyCode);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Get-MailContact");
                cmd.AddParameter("Filter", string.Format("CustomAttribute1 -eq '{0}'", companyCode));
                cmd.AddParameter("DomainController", domainController);
                cmd.AddCommand("Remove-MailContact");
                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to disable all contacts for company " + companyCode, ex);
                throw;
            }
        }

        public void DeleteAllGroups(string companyCode)
        {
            try
            {
                this.logger.Info("Deleting all groups for " + companyCode);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Get-DistributionGroup");
                cmd.AddParameter("Filter", string.Format("CustomAttribute1 -eq '{0}'", companyCode));
                cmd.AddParameter("DomainController", domainController);
                cmd.AddCommand("Remove-DistributionGroup");
                cmd.AddParameter("BypassSecurityGroupManagerCheck");
                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                    throw powershell.Streams.Error[0].Exception;
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to disable all groups for company " + companyCode, ex);
                throw;
            }
        }

        public void DeleteAddressBookPolicy(string name)
        {
            try
            {
                this.logger.Info("Deleting address book policy " + name);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Remove-AddressBookPolicy");
                cmd.AddParameter("Identity", name);
                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                {
                    ErrorCategory errCategory = powershell.Streams.Error[0].CategoryInfo.Category;
                    string reason = powershell.Streams.Error[0].CategoryInfo.Reason;

                    if (errCategory != ErrorCategory.NotSpecified && !reason.Equals("ManagementObjectNotFoundException"))
                        throw powershell.Streams.Error[0].Exception;
                    else
                        this.logger.Info("Failed to delete address book policy " + name + " but it could not be found. Must have already been deleted.");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to delete address book policy " + name, ex);
                throw;
            }
        }

        public void DeleteOfflineAddressBook(string name)
        {
            try
            {
                this.logger.Info("Deleting offline address book " + name);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Remove-OfflineAddressBook");
                cmd.AddParameter("Identity", name);
                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                {
                    ErrorCategory errCategory = powershell.Streams.Error[0].CategoryInfo.Category;
                    string reason = powershell.Streams.Error[0].CategoryInfo.Reason;

                    if (errCategory != ErrorCategory.NotSpecified && !reason.Equals("ManagementObjectNotFoundException"))
                        throw powershell.Streams.Error[0].Exception;
                    else
                        this.logger.Info("Failed to delete offline address book " + name + " but it could not be found. Must have already been deleted.");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to delete offline address book " + name, ex);
                throw;
            }
        }

        public void DeleteGlobalAddressList(string name)
        {
            try
            {
                this.logger.Info("Deleting global address list " + name);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Remove-GlobalAddressList");
                cmd.AddParameter("Identity", name);
                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                {
                    ErrorCategory errCategory = powershell.Streams.Error[0].CategoryInfo.Category;
                    string reason = powershell.Streams.Error[0].CategoryInfo.Reason;

                    if (errCategory != ErrorCategory.NotSpecified && !reason.Equals("ManagementObjectNotFoundException"))
                        throw powershell.Streams.Error[0].Exception;
                    else
                        this.logger.Info("Failed to delete global address list " + name + " but it could not be found. Must have already been deleted.");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to delete global address list " + name, ex);
                throw;
            }
        }

        public void DeleteAddressList(string name)
        {
            try
            {
                this.logger.Info("Deleting address list " + name);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Remove-AddressList");
                cmd.AddParameter("Identity", name);
                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                {
                    ErrorCategory errCategory = powershell.Streams.Error[0].CategoryInfo.Category;
                    string reason = powershell.Streams.Error[0].CategoryInfo.Reason;

                    if (errCategory != ErrorCategory.NotSpecified && !reason.Equals("ManagementObjectNotFoundException"))
                        throw powershell.Streams.Error[0].Exception;
                    else
                        this.logger.Info("Failed to delete address list " + name + " but it could not be found. Must have already been deleted.");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to delete address list " + name, ex);
                throw;
            }
        }

        public void DeleteDomain(string name)
        {
            try
            {
                this.logger.Info("Deleting accepted domain " + name);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Remove-AcceptedDomain");
                cmd.AddParameter("Identity", name);
                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                {
                    ErrorCategory errCategory = powershell.Streams.Error[0].CategoryInfo.Category;
                    string reason = powershell.Streams.Error[0].CategoryInfo.Reason;

                    if (errCategory != ErrorCategory.NotSpecified && !reason.Equals("ManagementObjectNotFoundException"))
                        throw powershell.Streams.Error[0].Exception;
                    else
                        this.logger.Info("Failed to delete accepted domain " + name + " but it could not be found. Must have already been deleted.");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to delete accepted domain " + name, ex);
                throw;
            }
        }

        public void DeleteDistributionGroup(string identity)
        {
            try
            {
                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Remove-DistributionGroup");
                cmd.AddParameter("Identity", identity);
                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                {
                    ErrorCategory errCategory = powershell.Streams.Error[0].CategoryInfo.Category;
                    string reason = powershell.Streams.Error[0].CategoryInfo.Reason;

                    if (errCategory != ErrorCategory.NotSpecified && !reason.Equals("ManagementObjectNotFoundException"))
                        throw powershell.Streams.Error[0].Exception;
                    else
                        this.logger.Info("Failed to remove distribution group " + identity + " because it didn't exist");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to delete distribution group " + identity, ex);
                throw;
            }
        }

        public void DeleteContact(string distinguishedName)
        {
            try
            {
                this.logger.Info("Deleting Exchange contact " + distinguishedName);

                PSCommand cmd = new PSCommand();
                cmd.AddCommand("Remove-MailContact");
                cmd.AddParameter("Identity", distinguishedName);
                cmd.AddParameter("Confirm", false);
                cmd.AddParameter("DomainController", domainController);
                powershell.Commands = cmd;
                powershell.Invoke();

                if (powershell.HadErrors)
                {
                    ErrorCategory errCategory = powershell.Streams.Error[0].CategoryInfo.Category;
                    string reason = powershell.Streams.Error[0].CategoryInfo.Reason;

                    if (errCategory != ErrorCategory.NotSpecified && !reason.Equals("ManagementObjectNotFoundException"))
                        throw powershell.Streams.Error[0].Exception;
                    else
                        this.logger.Info("Failed to delete Exchange contact " + distinguishedName + " but it could not be found. Must have already been deleted.");
                }
            }
            catch (Exception ex)
            {
                this.logger.Error("Failed to Exchange contact " + distinguishedName, ex);
                throw;
            }
        }

        #endregion

        #region Connection

        /// <summary>
        /// Create our connection information
        /// </summary>
        /// <param name="uri">Uri to Exchange server powershell directory</param>
        /// <param name="username">Username to connect</param>
        /// <param name="password">Password to connect</param>
        /// <param name="kerberos">True to use Kerberos authentication, false to use Basic authentication</param>
        /// <returns></returns>
        private WSManConnectionInfo GetConnection(string uri, string username, string password, bool kerberos)
        {
            SecureString pwd = new SecureString();
            foreach (char x in password)
                pwd.AppendChar(x);

            PSCredential ps = new PSCredential(username, pwd);

            WSManConnectionInfo wsinfo = new WSManConnectionInfo(new Uri(uri), "http://schemas.microsoft.com/powershell/Microsoft.Exchange", ps);
            wsinfo.SkipCACheck = true;
            wsinfo.SkipCNCheck = true;
            wsinfo.SkipRevocationCheck = true;
            wsinfo.OpenTimeout = 9000;
            
            if (kerberos)
                wsinfo.AuthenticationMechanism = AuthenticationMechanism.Kerberos;
            else
                wsinfo.AuthenticationMechanism = AuthenticationMechanism.Basic;

            return wsinfo;
        }

        #endregion

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
                    wsConn = null;

                    if (powershell != null)
                        powershell.Dispose();
                    
                    if (runspace != null)
                        runspace.Dispose();
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
