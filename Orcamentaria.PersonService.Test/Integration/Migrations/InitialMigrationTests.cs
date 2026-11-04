using Microsoft.EntityFrameworkCore;
using Orcamentaria.PersonService.Domain.Enums;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Infrastructure.Contexts;
using Testcontainers.MySql;
using Xunit;

namespace Orcamentaria.PersonService.Test.Integration.Migrations
{
    public class InitialMigrationTests : IAsyncLifetime
    {
        private readonly MySqlContainer _container = new MySqlBuilder()
            .WithDatabase("person-db")
            .WithUsername("test")
            .WithPassword("test")
            .Build();

        public Task InitializeAsync() => _container.StartAsync();

        public Task DisposeAsync() => _container.DisposeAsync().AsTask();

        [Fact]
        public async Task ApplyMigrations_CreatesSchemaAndIsIdempotent()
        {
            var cs = _container.GetConnectionString();
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseMySQL(cs)
                .Options;

            // Aplica migrations pela primeira vez
            using (var ctx = new MySqlContext(options))
                await ctx.Database.MigrateAsync();

            // Reaplicar é idempotente
            using (var ctx = new MySqlContext(options))
                await ctx.Database.MigrateAsync();

            // Insert e leitura básicos — Person + Address com Neighborhood
            using (var ctx = new MySqlContext(options))
            {
                var person = new Person
                {
                    Name = "Test",
                    Type = PersonTypeEnum.Client,
                    CompanyId = 1,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = 1
                };
                ctx.Persons.Add(person);
                await ctx.SaveChangesAsync();
                Assert.True(person.Id > 0);

                var address = new Address
                {
                    Street = "Rua A",
                    ZipCode = "12345678",
                    City = "São Paulo",
                    State = "São Paulo",
                    Uf = "SP",
                    Neighborhood = "Centro",
                    PersonId = person.Id,
                    CompanyId = 1,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = 1
                };
                ctx.Addresses.Add(address);
                await ctx.SaveChangesAsync();
                Assert.True(address.Id > 0);

                // Verifica que a coluna NEIGHBORHOOD persiste corretamente
                var saved = await ctx.Addresses.FindAsync(address.Id);
                Assert.Equal("Centro", saved!.Neighborhood);
            }

            // Insert de Employee com ValuePerDay decimal
            using (var ctx = new MySqlContext(options))
            {
                var person = new Person
                {
                    Name = "Employee Test",
                    Type = PersonTypeEnum.Employee,
                    CompanyId = 1,
                    Active = true,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = 1,
                    UpdatedAt = DateTime.UtcNow,
                    UpdatedBy = 1
                };
                ctx.Persons.Add(person);
                await ctx.SaveChangesAsync();

                var employee = new Employee
                {
                    Id = person.Id,
                    Post = "Developer",
                    AdmissionDate = DateTime.UtcNow.AddDays(-30),
                    ValuePerDay = 350.75m
                };
                ctx.Employees.Add(employee);
                await ctx.SaveChangesAsync();

                var saved = await ctx.Employees.FindAsync(employee.Id);
                Assert.Equal(350.75m, saved!.ValuePerDay);
            }
        }
    }
}
