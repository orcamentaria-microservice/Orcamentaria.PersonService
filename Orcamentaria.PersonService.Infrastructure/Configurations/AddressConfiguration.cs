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
            builder.Property(p => p.Id).HasColumnName("ID");
            builder.Property(p => p.Street).HasColumnName("STREET");
            builder.Property(p => p.ZipCode).HasColumnName("ZIPCODE");
            builder.Property(p => p.Number).HasColumnName("NUMBER");
            builder.Property(p => p.Complement).HasColumnName("COMPLEMENT");
            builder.Property(p => p.Neihborhood).HasColumnName("NEIHBORHOOD");
            builder.Property(p => p.City).HasColumnName("CITY");
            builder.Property(p => p.State).HasColumnName("STATE");
            builder.Property(p => p.Uf).HasColumnName("UF");
            builder.Property(p => p.Default).HasColumnName("DEFAULT");
            builder.Property(p => p.PersonId).HasColumnName("PERSON_ID");
        }
    }
}
