using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Infrastructure.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("T_CONTACT");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id)
                .HasColumnName("ID")
                .HasColumnType("BIGINT")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(p => p.ContactDescription)
                .HasColumnName("CONTACT")
                .HasColumnType("VARCHAR(150)")
                .IsRequired();

            builder.Property(p => p.Type)
                .HasColumnName("TYPE")
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.Default)
                .HasColumnName("DEFAULT")
                .HasColumnType("BIT");

            builder.Property(p => p.PersonId)
                .HasColumnName("PERSON_ID")
                .HasColumnType("BIGINT")
                .IsRequired();

            builder.Property(p => p.CompanyId)
                .HasColumnName("COMPANY_ID")
                .HasColumnType("BIGINT")
                .IsRequired();

            builder.Property(p => p.CreatedAt)
                .HasColumnName("CREATED_AT")
                .HasColumnType("DATETIME")
                .IsRequired();

            builder.Property(p => p.CreatedBy)
                .HasColumnName("CREATED_BY")
                .HasColumnType("BIGINT")
                .IsRequired();

            builder.Property(p => p.UpdatedAt)
                .HasColumnName("UPDATED_AT")
                .HasColumnType("DATETIME")
                .IsRequired();

            builder.Property(p => p.UpdatedBy)
                .HasColumnName("UPDATED_BY")
                .HasColumnType("BIGINT")
                .IsRequired();
        }
    }
}
