using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonService.Domain.Models;

namespace PersonService.Infrastructure.Configurations
{
    public class PersonConfiguration : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            builder.ToTable("T_PERSON");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.Id).HasColumnName("ID");
            builder.Property(p => p.Name).HasColumnName("NAME");
            builder.Property(p => p.Rg).HasColumnName("RG");
            builder.Property(p => p.Cpf).HasColumnName("CPF");
            builder.Property(p => p.Cnpj).HasColumnName("CNPJ");
            builder.Property(p => p.Type).HasColumnName("TYPE");
            builder.Property(p => p.CompanyId).HasColumnName("COMPANY_ID");
            builder.Property(p => p.Active).HasColumnName("ACTIVE");
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
