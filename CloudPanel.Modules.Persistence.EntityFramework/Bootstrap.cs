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

        public DbSet<User> Users { get; set; }
    }
}
