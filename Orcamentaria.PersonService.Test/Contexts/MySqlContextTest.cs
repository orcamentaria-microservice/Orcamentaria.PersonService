using Microsoft.EntityFrameworkCore;
using Orcamentaria.PersonService.Infrastructure.Contexts;

namespace Orcamentaria.PersonService.Test.Contexts
{
    public class MySqlContextTest : MySqlContext
    {
        public MySqlContextTest(DbContextOptions<DbContext> options) : base(options) { }
    }
}
