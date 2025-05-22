using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonService.Domain.Models;

namespace PersonService.Infrastructure.Configurations
{
    public class ContactConfiguration : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("T_CONTACT");
            builder.HasKey(x => x.Id);
            builder.Property(p => p.Id).HasColumnName("ID");
            builder.Property(p => p.ContactDescription).HasColumnName("CONTACT");
            builder.Property(p => p.Type).HasColumnName("TYPE");
            builder.Property(p => p.Default).HasColumnName("DEFAULT");
            builder.Property(p => p.PersonId).HasColumnName("PERSON_ID");
        }
    }
}
