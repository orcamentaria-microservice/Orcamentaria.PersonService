using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Infrastructure.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("T_PERSON");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id)
                .HasColumnName("ID")
                .HasColumnType("BIGINT")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(p => p.Name)
                .HasColumnName("NAME")
                .HasColumnType("VARCHAR(70)")
                .IsRequired();

            builder.Property(p => p.Rg)
                .HasColumnName("RG")
                .HasColumnType("VARCHAR(9)");

            builder.Property(p => p.Cpf)
                .HasColumnName("CPF")
                .HasColumnType("VARCHAR(11)");

            builder.Property(p => p.Cnpj)
                .HasColumnName("CNPJ")
                .HasColumnType("VARCHAR(14)");

            builder.Property(p => p.Type)
                .HasColumnName("TYPE")
                .HasColumnType("INT")
                .IsRequired();

            builder.Property(p => p.CompanyId)
                .HasColumnName("COMPANY_ID")
                .HasColumnType("BIGINT")
                .IsRequired();

            builder.Property(p => p.Active)
                .HasColumnName("ACTIVE")
                .HasColumnType("BIT")
                .HasDefaultValue(true)
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

            builder.Ignore(p => p.Addresses);
            builder.Ignore(p => p.Contacts);

            builder
                .HasMany(p => p.Addresses)
                .WithOne()
                .HasForeignKey(p => p.PersonId)
                .HasConstraintName("fk_T_ADDRESS_T_PERSON");

            builder
                .HasMany(p => p.Contacts)
                .WithOne()
                .HasForeignKey(p => p.PersonId)
                .HasConstraintName("fk_T_CONTACT_T_PERSON");
        }
    }
}
