using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Infrastructure.Contexts;
using Orcamentaria.Lib.Test.Repositories;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Infrastructure.Repositories;
using Orcamentaria.PersonService.Test.Contexts;
using Orcamentaria.PersonService.Test.Fixtures;
using Xunit;

namespace Orcamentaria.PersonService.Test.Repositories
{
    [Collection(nameof(AddressCollection))]
    public class AddressRepositoryTest
    {
        private readonly AddressFixture _fixture;
        private readonly MySqlContextTest _dbContext;
        private readonly DbContextOptions<DbContext> _options;
        private readonly UserAuthContext _userAuthContext;
        public AddressRepositoryTest(AddressFixture fixture)
        {
            _fixture = fixture;
            _userAuthContext = _fixture.CreateUserAuthContext();

            _options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

            _dbContext = new MySqlContextTest(_options);
        }

        #region CountItems
        [Fact]
        public void CountItems_WhenHaveData_ThenReturnsGreaterZero()
        {
            var personId = 1;

            _fixture.SeedInMemoryDatabase(_dbContext, 1, 2, 3, 4, 5);

            var mockRepository = new Mock<AddressRepository>(_dbContext, _userAuthContext) { CallBase = true };

            var result = mockRepository.Object.CountItems(personId);

            result.Should().Be(5);
        }

        [Fact]
        public void CountItems_WhenNotHaveData_ThenReturnsEqualsZero()
        {
            var personId = 1;

            var mockRepository = new Mock<AddressRepository>(_dbContext, _userAuthContext) { CallBase = true };

            var result = mockRepository.Object.CountItems(personId);

            result.Should().Be(0);
        }
        #endregion
    }

    [Collection(nameof(AddressCollection))]
    public class AddressReadRepositoryTest : ReadWithCompanyRepositoryTests<Address, MySqlContextTest>
    {
        public AddressReadRepositoryTest(AddressFixture fixture) : base(fixture) { }
    }

    [Collection(nameof(AddressCollection))]
    public class AddressWriteRepositoryTest : WriteWithCompanyRepositoryTests<Address, MySqlContextTest>
    {
        public AddressWriteRepositoryTest(AddressFixture fixture) : base(fixture) { }
    }

    [Collection(nameof(AddressCollection))]
    public class AddressDeleteRepositoryTest : DeleteWithCompanyRepositoryTests<Address, MySqlContextTest>
    {
        public AddressDeleteRepositoryTest(AddressFixture fixture) : base(fixture) { }
    }
}
