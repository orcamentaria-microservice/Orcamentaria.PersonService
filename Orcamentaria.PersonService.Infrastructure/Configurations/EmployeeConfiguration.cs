using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Infrastructure.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("T_EMPLOYEE");
            builder.Property(p => p.Post)
                .HasColumnName("POST")
                .HasColumnType("VARCHAR(60)")
                .IsRequired();

            builder.Property(p => p.AdmissionDate)
                .HasColumnName("ADMISSION_DATE")
                .HasColumnType("DATETIME")
                .IsRequired();

            builder.Property(p => p.ValuePerDay)
                .HasColumnName("VALUE_PER_DAY")
                .HasColumnType("DOUBLE")
                .IsRequired();
        }
    }
}
