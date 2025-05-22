using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonService.Domain.Models;

namespace PersonService.Infrastructure.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("T_EMPLOYEE");
            builder.Property(p => p.Post).HasColumnName("POST");
            builder.Property(p => p.AdmissionDate).HasColumnName("ADMISSION_DATE");
            builder.Property(p => p.ValuePerDay).HasColumnName("VALUE_PER_DAY");
        }
    }
}
