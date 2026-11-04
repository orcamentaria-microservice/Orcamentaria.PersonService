using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using MySql.EntityFrameworkCore.Extensions;

namespace Orcamentaria.PersonService.Infrastructure.Contexts
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<MySqlContext>
    {
        public MySqlContext CreateDbContext(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection")
                ?? "Server=localhost;Port=3306;Database=person-db;SslMode=Disabled";
            var options = new DbContextOptionsBuilder<DbContext>()
                .UseMySQL(connectionString)
                .Options;
            return new MySqlContext(options);
        }
    }
}
