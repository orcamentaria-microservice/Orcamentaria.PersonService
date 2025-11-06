using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Infrastructure.Configurations
{
    public class AddressConfiguration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("T_ADDRESS");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id)
                .HasColumnName("ID")
                .HasColumnType("BIGINT")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(p => p.Street)
                .HasColumnName("STREET")
                .HasColumnType("VARCHAR(70)")
                .IsRequired();

            builder.Property(p => p.ZipCode)
                .HasColumnName("ZIPCODE")
                .HasColumnType("VARCHAR(8)")
                .IsRequired();

            builder.Property(p => p.Number)
                .HasColumnName("NUMBER")
                .HasColumnType("VARCHAR(6)");

            builder.Property(p => p.Complement)
                .HasColumnName("COMPLEMENT")
                .HasColumnType("VARCHAR(45)");

            builder.Property(p => p.Neihborhood)
                .HasColumnName("NEIHBORHOOD")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(p => p.City)
                .HasColumnName("CITY")
                .HasColumnType("VARCHAR(100)")
                .IsRequired();

            builder.Property(p => p.State)
                .HasColumnName("STATE")
                .HasColumnType("VARCHAR(45)")
                .IsRequired();

            builder.Property(p => p.Uf)
                .HasColumnName("UF")
                .HasColumnType("VARCHAR(2)")
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
