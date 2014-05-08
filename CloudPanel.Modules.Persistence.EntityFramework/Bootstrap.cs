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

// NB: http://stackoverflow.com/questions/16210771/entity-framework-code-first-without-app-config
namespace CloudPanel.Modules.Persistence.EntityFramework
{
    internal sealed class CloudPanelDbConfiguration : DbConfiguration
    {
    }

    // Need to support schema migrations here.
    internal sealed class CloudPanelContextInitializer : CreateDatabaseIfNotExists<CloudPanelContext>
    {
    }

    public sealed class CloudPanelContext : DbContext
    {
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
