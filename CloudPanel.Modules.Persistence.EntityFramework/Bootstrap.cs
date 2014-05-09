using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudPanel.Modules.Persistence.EntityFramework.Models;
using CloudPanel.Modules.Persistence.EntityFramework.Migrations;
using CloudPanel.Modules.Persistence.EntityFramework.Models.Mapping;

// NB: http://stackoverflow.com/questions/16210771/entity-framework-code-first-without-app-config
namespace CloudPanel.Modules.Persistence.EntityFramework
{
    internal sealed class CloudPanelDbConfiguration : DbConfiguration
    {
    }

    internal sealed class CloudPanelContextInitializer : IDatabaseInitializer<CloudPanelContext>
    {
        #region IDatabaseInitializer<CloudPanelContext> Members

        public void InitializeDatabase(CloudPanelContext context)
        {
            var configuration = new Configuration
            {
                TargetDatabase = new DbConnectionInfo(context.Database.Connection.ConnectionString, @"System.Data.SqlClient"),
            };

            var migrator = new DbMigrator(configuration);
            migrator.Update();
        }

        #endregion
    }

    public sealed class CloudPanelContext : DbContext
    {
        public CloudPanelContext()
        {
        }

        public CloudPanelContext(string connectionString)
            : base(connectionString)
        {
            Database.SetInitializer(new CloudPanelContextInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new ApiAccessMap());
            modelBuilder.Configurations.Add(new AuditMap());
            modelBuilder.Configurations.Add(new AuditLoginMap());
            modelBuilder.Configurations.Add(new CompanyMap());
            modelBuilder.Configurations.Add(new CompanyStatMap());
            modelBuilder.Configurations.Add(new ContactMap());
            modelBuilder.Configurations.Add(new DistributionGroupMap());
            modelBuilder.Configurations.Add(new DomainMap());
            modelBuilder.Configurations.Add(new LogTableMap());
            modelBuilder.Configurations.Add(new Plans_CitrixMap());
            modelBuilder.Configurations.Add(new Plans_ExchangeActiveSyncMap());
            modelBuilder.Configurations.Add(new Plans_ExchangeMailboxMap());
            modelBuilder.Configurations.Add(new Plans_OrganizationMap());
            modelBuilder.Configurations.Add(new PriceOverrideMap());
            modelBuilder.Configurations.Add(new PriceMap());
            modelBuilder.Configurations.Add(new ResourceMailboxMap());
            modelBuilder.Configurations.Add(new SettingMap());
            modelBuilder.Configurations.Add(new Stats_CitrixCountMap());
            modelBuilder.Configurations.Add(new Stats_ExchCountMap());
            modelBuilder.Configurations.Add(new Stats_UserCountMap());
            modelBuilder.Configurations.Add(new SvcMailboxDatabaseSizeMap());
            modelBuilder.Configurations.Add(new SvcMailboxSizeMap());
            modelBuilder.Configurations.Add(new SvcQueueMap());
            modelBuilder.Configurations.Add(new SvcTaskMap());
            modelBuilder.Configurations.Add(new UserPermissionMap());
            modelBuilder.Configurations.Add(new UserPlansCitrixMap());
            modelBuilder.Configurations.Add(new UserMap());
        }

        public DbSet<ApiAccess> ApiAccesses { get; set; }
        public DbSet<Audit> Audits { get; set; }
        public DbSet<AuditLogin> AuditLogins { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<CompanyStat> CompanyStats { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<DistributionGroup> DistributionGroups { get; set; }
        public DbSet<Domain> Domains { get; set; }
        public DbSet<LogTable> LogTables { get; set; }
        public DbSet<Plans_Citrix> Plans_Citrix { get; set; }
        public DbSet<Plans_ExchangeActiveSync> Plans_ExchangeActiveSync { get; set; }
        public DbSet<Plans_ExchangeMailbox> Plans_ExchangeMailbox { get; set; }
        public DbSet<Plans_Organization> Plans_Organization { get; set; }
        public DbSet<PriceOverride> PriceOverrides { get; set; }
        public DbSet<Price> Prices { get; set; }
        public DbSet<ResourceMailbox> ResourceMailboxes { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Stats_CitrixCount> Stats_CitrixCount { get; set; }
        public DbSet<Stats_ExchCount> Stats_ExchCount { get; set; }
        public DbSet<Stats_UserCount> Stats_UserCount { get; set; }
        public DbSet<SvcMailboxDatabaseSize> SvcMailboxDatabaseSizes { get; set; }
        public DbSet<SvcMailboxSize> SvcMailboxSizes { get; set; }
        public DbSet<SvcQueue> SvcQueues { get; set; }
        public DbSet<SvcTask> SvcTasks { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<UserPlansCitrix> UserPlansCitrices { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
