using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace CloudPanel.Modules.Persistence.EntityFramework.Models.Mapping
{
    public class ContactMap : EntityTypeConfiguration<Contact>
    {
        public ContactMap()
        {
            // Primary Key
            this.HasKey(t => t.DistinguishedName);

            // Properties
            this.Property(t => t.DistinguishedName)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.CompanyCode)
                .HasMaxLength(255);

            this.Property(t => t.DisplayName)
                .IsRequired()
                .HasMaxLength(255);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(255);

            // Table & Column Mappings
            this.ToTable("Contacts");
            this.Property(t => t.DistinguishedName).HasColumnName("DistinguishedName");
            this.Property(t => t.CompanyCode).HasColumnName("CompanyCode");
            this.Property(t => t.DisplayName).HasColumnName("DisplayName");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Hidden).HasColumnName("Hidden");
        }
    }
}
