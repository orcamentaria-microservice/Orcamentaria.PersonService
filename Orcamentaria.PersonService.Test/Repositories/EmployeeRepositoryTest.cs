using Microsoft.EntityFrameworkCore;
using Orcamentaria.Lib.Infrastructure.Contexts;
using Orcamentaria.Lib.Test.Repositories;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Test.Contexts;
using Orcamentaria.PersonService.Test.Fixtures;
using Xunit;

namespace Orcamentaria.PersonService.Test.Repositories
{
    [Collection(nameof(EmployeeCollection))]
    public class EmployeeRepositoryTest
    {
        private readonly PersonFixture _fixture;
        private readonly MySqlContextTest _dbContext;
        private readonly DbContextOptions<DbContext> _options;
        private readonly UserAuthContext _userAuthContext;
        public EmployeeRepositoryTest(PersonFixture fixture)
        {
            _fixture = fixture;
            _userAuthContext = _fixture.CreateUserAuthContext();

            _options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

            _dbContext = new MySqlContextTest(_options);
        }
    }

    [Collection(nameof(EmployeeCollection))]
    public class EmployeeReadRepositoryTest : ReadWithCompanyRepositoryTests<Employee, MySqlContextTest>
    {
        public EmployeeReadRepositoryTest(EmployeeFixture fixture) : base(fixture) { }
    }

    [Collection(nameof(EmployeeCollection))]
    public class EmployeeWriteRepositoryTest : WriteWithCompanyRepositoryTests<Employee, MySqlContextTest>
    {
        public EmployeeWriteRepositoryTest(EmployeeFixture fixture) : base(fixture) { }
    }
}
