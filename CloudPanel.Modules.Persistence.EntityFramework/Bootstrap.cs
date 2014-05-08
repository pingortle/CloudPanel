using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CloudPanel.Modules.Persistence.Domain;
using CloudPanel.Modules.Persistence.EntityFramework.Migrations;

// NB: http://stackoverflow.com/questions/16210771/entity-framework-code-first-without-app-config
namespace CloudPanel.Modules.Persistence.EntityFramework
{
    internal sealed class CloudPanelDbConfiguration : DbConfiguration
    {
    }

    // Need to support schema migrations here.
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
        public DbSet<Plans_TerminalServices> Plans_TerminalServices { get; set; }
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
        public DbSet<UserPlan> UserPlans { get; set; }
        public DbSet<UserPlansCitrix> UserPlansCitrices { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
