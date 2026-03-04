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
    [Collection(nameof(ContactCollection))]
    public class ContactRepositoryTest
    {
        private readonly ContactFixture _fixture;
        private readonly MySqlContextTest _dbContext;
        private readonly DbContextOptions<DbContext> _options;
        private readonly UserAuthContext _userAuthContext;
        public ContactRepositoryTest(ContactFixture fixture)
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

            var mockRepository = new Mock<ContactRepository>(_dbContext, _userAuthContext) { CallBase = true };

            var result = mockRepository.Object.CountItems(personId);

            result.Should().Be(5);
        }

        [Fact]
        public void CountItems_WhenNotHaveData_ThenReturnsEqualsZero()
        {
            var personId = 1;

            var mockRepository = new Mock<ContactRepository>(_dbContext, _userAuthContext) { CallBase = true };

            var result = mockRepository.Object.CountItems(personId);

            result.Should().Be(0);
        }
        #endregion
    }

    [Collection(nameof(ContactCollection))]
    public class ContactReadRepositoryTest : ReadWithCompanyRepositoryTests<Contact, MySqlContextTest>
    {
        public ContactReadRepositoryTest(ContactFixture fixture) : base(fixture) { }
    }

    [Collection(nameof(ContactCollection))]
    public class ContactWriteRepositoryTest : WriteWithCompanyRepositoryTests<Contact, MySqlContextTest>
    {
        public ContactWriteRepositoryTest(ContactFixture fixture) : base(fixture) { }
    }

    [Collection(nameof(ContactCollection))]
    public class ContactDeleteRepositoryTest : DeleteWithCompanyRepositoryTests<Contact, MySqlContextTest>
    {
        public ContactDeleteRepositoryTest(ContactFixture fixture) : base(fixture) { }
    }
}
