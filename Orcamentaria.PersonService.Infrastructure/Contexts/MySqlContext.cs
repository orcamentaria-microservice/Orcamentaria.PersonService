using Microsoft.EntityFrameworkCore;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Infrastructure.Configurations;

namespace Orcamentaria.PersonService.Infrastructure.Contexts
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options)
        : base(options)
        {

        }

        public DbSet<Person> Persons { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PersonConfiguration());
            modelBuilder.ApplyConfiguration(new EmployeeConfiguration());
            modelBuilder.ApplyConfiguration(new ContactConfiguration());
            modelBuilder.ApplyConfiguration(new AddressConfiguration());
        }
    }
}
