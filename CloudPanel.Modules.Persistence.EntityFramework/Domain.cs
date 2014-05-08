using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudPanel.Modules.Persistence.Domain
{
    public class ApiAccess
    {
        public string CustomerKey { get; set; }
        public string CustomerSecret { get; set; }
        public string CompanyCode { get; set; }
    }

    public class Audit
    {
        public int AuditID { get; set; }
        public string Username { get; set; }
        public System.DateTime Date { get; set; }
        public string CompanyCode { get; set; }
        public int ActionID { get; set; }
        public string Variable1 { get; set; }
        public string Variable2 { get; set; }
    }

    public class AuditLogin
    {
        public int ID { get; set; }
        public string IPAddress { get; set; }
        public string Username { get; set; }
        public bool LoginStatus { get; set; }
        public System.DateTime AuditTimeStamp { get; set; }
    }

    public class Company
    {
        public int CompanyId { get; set; }
        public bool IsReseller { get; set; }
        public string ResellerCode { get; set; }
        public Nullable<int> OrgPlanID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyCode { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Website { get; set; }
        public string Description { get; set; }
        public string AdminName { get; set; }
        public string AdminEmail { get; set; }
        public string DistinguishedName { get; set; }
        public System.DateTime Created { get; set; }
        public bool ExchEnabled { get; set; }
        public Nullable<bool> LyncEnabled { get; set; }
        public Nullable<bool> CitrixEnabled { get; set; }
        public Nullable<int> ExchPFPlan { get; set; }
        public string Country { get; set; }
        public Nullable<bool> ExchPermFixed { get; set; }
    }

    public class CompanyStat
    {
        public int CompanyID { get; set; }
        public string CompanyCode { get; set; }
        public int UserCount { get; set; }
        public int DomainCount { get; set; }
        public int SubDomainCount { get; set; }
        public int ExchMailboxCount { get; set; }
        public int ExchContactsCount { get; set; }
        public int ExchDistListsCount { get; set; }
        public int ExchPublicFolderCount { get; set; }
        public int ExchMailPublicFolderCount { get; set; }
        public int ExchKeepDeletedItems { get; set; }
        public int RDPUserCount { get; set; }
    }

    public class Contact
    {
        public string DistinguishedName { get; set; }
        public string CompanyCode { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool Hidden { get; set; }
    }

    public class DistributionGroup
    {
        public int ID { get; set; }
        public string DistinguishedName { get; set; }
        public string CompanyCode { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public bool Hidden { get; set; }
    }

    public class Domain
    {
        public int DomainID { get; set; }
        public string CompanyCode { get; set; }
        public string Domain1 { get; set; }
        public Nullable<bool> IsSubDomain { get; set; }
        public bool IsDefault { get; set; }
        public bool IsAcceptedDomain { get; set; }
        public Nullable<bool> IsLyncDomain { get; set; }
        public Nullable<int> DomainType { get; set; }
    }

    public class LogTable
    {
        public int AuditID { get; set; }
        public System.DateTime LogTime { get; set; }
        public int LogType { get; set; }
        public string CodeClass { get; set; }
        public string UserPrincipalName { get; set; }
        public string CompanyCode { get; set; }
        public string ResellerCode { get; set; }
        public string Message { get; set; }
        public string RecommendedAction { get; set; }
        public string Exception { get; set; }
    }

    public class Plans_Citrix
    {
        public int CitrixPlanID { get; set; }
        public string Name { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public bool IsServer { get; set; }
        public string CompanyCode { get; set; }
        public string Price { get; set; }
        public string Cost { get; set; }
        public string PictureURL { get; set; }
    }

    public class Plans_ExchangeActiveSync
    {
        public int ASID { get; set; }
        public string CompanyCode { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string ExchangeName { get; set; }
        public Nullable<bool> AllowNonProvisionableDevices { get; set; }
        public Nullable<int> RefreshIntervalInHours { get; set; }
        public Nullable<bool> RequirePassword { get; set; }
        public Nullable<bool> RequireAlphanumericPassword { get; set; }
        public Nullable<bool> EnablePasswordRecovery { get; set; }
        public Nullable<bool> RequireEncryptionOnDevice { get; set; }
        public Nullable<bool> RequireEncryptionOnStorageCard { get; set; }
        public Nullable<bool> AllowSimplePassword { get; set; }
        public Nullable<int> NumberOfFailedAttempted { get; set; }
        public Nullable<int> MinimumPasswordLength { get; set; }
        public Nullable<int> InactivityTimeoutInMinutes { get; set; }
        public Nullable<int> PasswordExpirationInDays { get; set; }
        public Nullable<int> EnforcePasswordHistory { get; set; }
        public string IncludePastCalendarItems { get; set; }
        public string IncludePastEmailItems { get; set; }
        public Nullable<int> LimitEmailSizeInKB { get; set; }
        public Nullable<bool> AllowDirectPushWhenRoaming { get; set; }
        public Nullable<bool> AllowHTMLEmail { get; set; }
        public Nullable<bool> AllowAttachmentsDownload { get; set; }
        public Nullable<int> MaximumAttachmentSizeInKB { get; set; }
        public Nullable<bool> AllowRemovableStorage { get; set; }
        public Nullable<bool> AllowCamera { get; set; }
        public Nullable<bool> AllowWiFi { get; set; }
        public Nullable<bool> AllowInfrared { get; set; }
        public Nullable<bool> AllowInternetSharing { get; set; }
        public Nullable<bool> AllowRemoteDesktop { get; set; }
        public Nullable<bool> AllowDesktopSync { get; set; }
        public string AllowBluetooth { get; set; }
        public Nullable<bool> AllowBrowser { get; set; }
        public Nullable<bool> AllowConsumerMail { get; set; }
        public Nullable<bool> IsEnterpriseCAL { get; set; }
        public Nullable<bool> AllowTextMessaging { get; set; }
        public Nullable<bool> AllowUnsignedApplications { get; set; }
        public Nullable<bool> AllowUnsignedInstallationPackages { get; set; }
    }

    public class Plans_ExchangeMailbox
    {
        public int MailboxPlanID { get; set; }
        public string MailboxPlanName { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string ResellerCode { get; set; }
        public string CompanyCode { get; set; }
        public int MailboxSizeMB { get; set; }
        public Nullable<int> MaxMailboxSizeMB { get; set; }
        public int MaxSendKB { get; set; }
        public int MaxReceiveKB { get; set; }
        public int MaxRecipients { get; set; }
        public bool EnablePOP3 { get; set; }
        public bool EnableIMAP { get; set; }
        public bool EnableOWA { get; set; }
        public bool EnableMAPI { get; set; }
        public bool EnableAS { get; set; }
        public bool EnableECP { get; set; }
        public int MaxKeepDeletedItems { get; set; }
        public string MailboxPlanDesc { get; set; }
        public string Price { get; set; }
        public string Cost { get; set; }
        public string AdditionalGBPrice { get; set; }
    }

    public class Plans_Organization
    {
        public int OrgPlanID { get; set; }
        public string OrgPlanName { get; set; }
        public int ProductID { get; set; }
        public string ResellerCode { get; set; }
        public int MaxUsers { get; set; }
        public int MaxDomains { get; set; }
        public int MaxExchangeMailboxes { get; set; }
        public int MaxExchangeContacts { get; set; }
        public int MaxExchangeDistLists { get; set; }
        public int MaxExchangePublicFolders { get; set; }
        public int MaxExchangeMailPublicFolders { get; set; }
        public int MaxExchangeKeepDeletedItems { get; set; }
        public Nullable<int> MaxExchangeActivesyncPolicies { get; set; }
        public int MaxTerminalServerUsers { get; set; }
        public Nullable<int> MaxExchangeResourceMailboxes { get; set; }
    }

    public class Plans_TerminalServices
    {
        public int TSPlanID { get; set; }
        public string TSPlanName { get; set; }
        public string ResellerCode { get; set; }
        public int ProductID { get; set; }
        public Nullable<int> MaxUserSpaceMB { get; set; }
    }

    public class PriceOverride
    {
        public string CompanyCode { get; set; }
        public string Price { get; set; }
        public Nullable<int> PlanID { get; set; }
        public string Product { get; set; }
    }

    public class Price
    {
        public int PriceID { get; set; }
        public int ProductID { get; set; }
        public int PlanID { get; set; }
        public string CompanyCode { get; set; }
        public decimal Price1 { get; set; }
    }

    public class ResourceMailbox
    {
        public int ResourceID { get; set; }
        public string DisplayName { get; set; }
        public string CompanyCode { get; set; }
        public string UserPrincipalName { get; set; }
        public string PrimarySmtpAddress { get; set; }
        public string ResourceType { get; set; }
        public int MailboxPlan { get; set; }
        public int AdditionalMB { get; set; }
    }

    public class Setting
    {
        public string BaseOU { get; set; }
        public string PrimaryDC { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string SuperAdmins { get; set; }
        public string BillingAdmins { get; set; }
        public string ExchangeFqdn { get; set; }
        public string ExchangePFServer { get; set; }
        public int ExchangeVersion { get; set; }
        public bool ExchangeSSLEnabled { get; set; }
        public string ExchangeConnectionType { get; set; }
        public int PasswordMinLength { get; set; }
        public int PasswordComplexityType { get; set; }
        public bool CitrixEnabled { get; set; }
        public bool PublicFolderEnabled { get; set; }
        public bool LyncEnabled { get; set; }
        public bool WebsiteEnabled { get; set; }
        public bool SQLEnabled { get; set; }
        public string CurrencySymbol { get; set; }
        public string CurrencyEnglishName { get; set; }
        public Nullable<bool> ResellersEnabled { get; set; }
        public string CompanysName { get; set; }
        public Nullable<bool> AllowCustomNameAttrib { get; set; }
        public Nullable<bool> ExchStats { get; set; }
        public Nullable<bool> IPBlockingEnabled { get; set; }
        public Nullable<int> IPBlockingFailedCount { get; set; }
        public Nullable<int> IPBlockingLockedMinutes { get; set; }
        public string ExchDatabases { get; set; }
        public string UsersOU { get; set; }
        public string BrandingLoginLogo { get; set; }
        public string BrandingCornerLogo { get; set; }
        public Nullable<bool> LockdownEnabled { get; set; }
        public string LyncFrontEnd { get; set; }
        public string LyncUserPool { get; set; }
        public string LyncMeetingUrl { get; set; }
        public string LyncDialinUrl { get; set; }
        public string CompanysLogo { get; set; }
        public Nullable<bool> SupportMailEnabled { get; set; }
        public string SupportMailAddress { get; set; }
        public string SupportMailServer { get; set; }
        public Nullable<int> SupportMailPort { get; set; }
        public string SupportMailUsername { get; set; }
        public string SupportMailPassword { get; set; }
        public string SupportMailFrom { get; set; }
        public int ID { get; set; }
    }

    public class Stats_CitrixCount
    {
        public System.DateTime StatDate { get; set; }
        public int UserCount { get; set; }
    }

    public class Stats_ExchCount
    {
        public System.DateTime StatDate { get; set; }
        public int UserCount { get; set; }
    }

    public class Stats_UserCount
    {
        public System.DateTime StatDate { get; set; }
        public int UserCount { get; set; }
    }

    public class SvcMailboxDatabaseSize
    {
        public int ID { get; set; }
        public string DatabaseName { get; set; }
        public string Server { get; set; }
        public string DatabaseSize { get; set; }
        public System.DateTime Retrieved { get; set; }
    }

    public class SvcMailboxSize
    {
        public int ID { get; set; }
        public string UserPrincipalName { get; set; }
        public string MailboxDatabase { get; set; }
        public string TotalItemSizeInKB { get; set; }
        public string TotalDeletedItemSizeInKB { get; set; }
        public int ItemCount { get; set; }
        public int DeletedItemCount { get; set; }
        public System.DateTime Retrieved { get; set; }
    }

    public class SvcQueue
    {
        public int SvcQueueID { get; set; }
        public int TaskID { get; set; }
        public string UserPrincipalName { get; set; }
        public string CompanyCode { get; set; }
        public string TaskOutput { get; set; }
        public System.DateTime TaskCreated { get; set; }
        public Nullable<System.DateTime> TaskCompleted { get; set; }
        public int TaskDelayInMinutes { get; set; }
        public int TaskSuccess { get; set; }
    }

    public class SvcTask
    {
        public int SvcTaskID { get; set; }
        public int TaskType { get; set; }
        public System.DateTime LastRun { get; set; }
        public Nullable<System.DateTime> NextRun { get; set; }
        public string TaskOutput { get; set; }
        public int TaskDelayInMinutes { get; set; }
        public Nullable<System.DateTime> TaskCreated { get; set; }
        public bool Reoccurring { get; set; }
    }

    public class UserPermission
    {
        public int UserID { get; set; }
        public bool EnableExchange { get; set; }
        public bool DisableExchange { get; set; }
        public bool AddDomain { get; set; }
        public bool DeleteDomain { get; set; }
        public bool EnableAcceptedDomain { get; set; }
        public bool DisableAcceptedDomain { get; set; }
    }

    public class UserPlan
    {
        public System.Guid UserGuid { get; set; }
        public int ProductID { get; set; }
        public int PlanID { get; set; }
        public string CompanyCode { get; set; }
    }

    public class UserPlansCitrix
    {
        public int UPCID { get; set; }
        public int UserID { get; set; }
        public int CitrixPlanID { get; set; }
    }

    public class User
    {
        public int ID { get; set; }
        public System.Guid UserGuid { get; set; }
        public string CompanyCode { get; set; }
        public string sAMAccountName { get; set; }
        public string UserPrincipalName { get; set; }
        public string DistinguishedName { get; set; }
        public string DisplayName { get; set; }
        public string Firstname { get; set; }
        public string Middlename { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Department { get; set; }
        public Nullable<bool> IsResellerAdmin { get; set; }
        public Nullable<bool> IsCompanyAdmin { get; set; }
        public Nullable<int> MailboxPlan { get; set; }
        public Nullable<int> TSPlan { get; set; }
        public Nullable<int> LyncPlan { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
        public Nullable<int> AdditionalMB { get; set; }
        public Nullable<int> ActiveSyncPlan { get; set; }
        public Nullable<bool> IsEnabled { get; set; }
        public Nullable<int> ExchArchivePlan { get; set; }
    }
}
