using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Infrastructure.Contexts;
using Orcamentaria.Lib.Infrastructure.Repositories;
using Orcamentaria.Lib.Test.Repositories;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Infrastructure.Repositories;
using Orcamentaria.PersonService.Test.Contexts;
using Orcamentaria.PersonService.Test.Fixtures;
using Xunit;

namespace Orcamentaria.PersonService.Test.Repositories
{
    [Collection(nameof(PersonCollection))]
    public class PersonRepositoryTest
    {
        private readonly PersonFixture _fixture;
        private readonly MySqlContextTest _dbContext;
        private readonly DbContextOptions<DbContext> _options;
        private readonly UserAuthContext _userAuthContext;
        public PersonRepositoryTest(PersonFixture fixture)
        {
            _fixture = fixture;
            _userAuthContext = _fixture.CreateUserAuthContext();

            _options = new DbContextOptionsBuilder<DbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .EnableSensitiveDataLogging()
            .Options;

            _dbContext = new MySqlContextTest(_options);
        }

        #region GetForServiceAsync
        [Xunit.Theory]
        [InlineData("id", "gt", 1)]
        [InlineData("id", "ne", 3)]
        [InlineData("id", "in", "1, 3")]
        [InlineData("id", "in", "1, 3, 9")]
        public async Task GetForServiceAsync_WhenHaveData_ReturnsData(string field, string op, object value)
        {
            var mockRepository = new Mock<PersonRepository>(_dbContext, _userAuthContext) { CallBase = true };

            await _fixture.SeedInMemoryDatabase(_dbContext, 1, 3, 6);

            var gridParams = _fixture.CreateGridParamsWithOneFilter(new FilterParam { Field = field, Operator = op, Value = value });

            var result = await mockRepository.Object.GetForServiceAsync(gridParams);

            result.Item1.Should().HaveCount(2);
            result.Item2.TotalItems.Should().Be(2);

            mockRepository.Protected()
                .Verify(
                "ApplyFiltersWithoutCompanyIdWrapper",
                Times.Once(),
                ItExpr.IsAny<IQueryable<Person>>(),
                ItExpr.IsAny<IEnumerable<FilterParam>>());

            mockRepository.Protected()
                .Verify(
                "ApplySortingWrapper",
                Times.Once(),
                ItExpr.IsAny<IQueryable<Person>>(),
                ItExpr.IsAny<string?>(),
                ItExpr.IsAny<bool>());

            mockRepository.Protected()
                .Verify(
                "NormalizePagingWrapper",
                Times.Once(),
                ItExpr.IsAny<int>(),
                ItExpr.IsAny<int>(),
                ItExpr.IsAny<int>());
        }

        [Xunit.Theory]
        [InlineData("id", "gt", 6)]
        [InlineData("companyId", "eq", 21)]
        [InlineData("id", "in", "44")]
        public async Task GetForServiceAsync_WhenNotHaveData_ReturnsEmpty(string field, string op, object value)
        {
            var mockRepository = new Mock<PersonRepository>(_dbContext, _userAuthContext) { CallBase = true };

            await _fixture.SeedInMemoryDatabase(_dbContext, 1, 3, 6);

            var gridParams = _fixture.CreateGridParamsWithOneFilter(new FilterParam { Field = field, Operator = op, Value = value });

            var result = await mockRepository.Object.GetForServiceAsync(gridParams);

            result.Item1.Should().HaveCount(0);
            result.Item2.TotalItems.Should().Be(0);

            mockRepository.Protected()
                .Verify(
                "ApplyFiltersWithoutCompanyIdWrapper",
                Times.Once(),
                ItExpr.IsAny<IQueryable<Person>>(),
                ItExpr.IsAny<IEnumerable<FilterParam>>());

            mockRepository.Protected()
                .Verify(
                "ApplySortingWrapper",
                Times.Once(),
                ItExpr.IsAny<IQueryable<Person>>(),
                ItExpr.IsAny<string?>(),
                ItExpr.IsAny<bool>());

            mockRepository.Protected()
                .Verify(
                "NormalizePagingWrapper",
                Times.Once(),
                ItExpr.IsAny<int>(),
                ItExpr.IsAny<int>(),
                ItExpr.IsAny<int>());
        }

        [Xunit.Theory]
        [InlineData("id", "eq", 1)]
        [InlineData("id", "eq", 3)]
        [InlineData("id", "in", "6")]
        public async Task GetForServiceAsync_WhenAnyExceptionIsThrownInApplyFiltersWithoutCompanyIdWrapper(string field, string op, object value)
        {
            var mockRepository = new Mock<PersonRepository>(_dbContext, _userAuthContext) { CallBase = true };

            await _fixture.SeedInMemoryDatabase(_dbContext, 1, 3, 6);

            var gridParams = _fixture.CreateGridParamsWithOneFilter(new FilterParam { Field = field, Operator = op, Value = value });

            mockRepository.Protected()
                .Setup<IQueryable<Person>>(
                    "ApplyFiltersWithoutCompanyIdWrapper",
                    ItExpr.IsAny<IQueryable<Person>>(),
                    ItExpr.IsAny<IEnumerable<FilterParam>>())
                .Throws(new Exception());

            Func<Task> act = async () => await mockRepository.Object.GetForServiceAsync(gridParams);

            var exception = await act.Should().ThrowAsync<UnexpectedException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.InternalError);

            mockRepository.Protected()
                .Verify(
                "ApplyFiltersWithoutCompanyIdWrapper",
                Times.Once(),
                ItExpr.IsAny<IQueryable<Person>>(),
                ItExpr.IsAny<IEnumerable<FilterParam>>());

            mockRepository.Protected()
                .Verify(
                "ApplySortingWrapper",
                Times.Never(),
                ItExpr.IsAny<IQueryable<Person>>(),
                ItExpr.IsAny<string?>(),
                ItExpr.IsAny<bool>());

            mockRepository.Protected()
                .Verify(
                "NormalizePagingWrapper",
                Times.Never(),
                ItExpr.IsAny<int>(),
                ItExpr.IsAny<int>(),
                ItExpr.IsAny<int>());
        }

        [Xunit.Theory]
        [InlineData("id", "eq", 1)]
        [InlineData("id", "eq", 3)]
        [InlineData("id", "in", "6")]
        public async Task GetForServiceAsync_WhenAnyExceptionIsThrownInApplySortingWrapper(string field, string op, object value)
        {
            var mockRepository = new Mock<PersonRepository>(_dbContext, _userAuthContext) { CallBase = true };

            await _fixture.SeedInMemoryDatabase(_dbContext, 1, 3, 6);

            var gridParams = _fixture.CreateGridParamsWithOneFilter(new FilterParam { Field = field, Operator = op, Value = value });

            mockRepository.Protected()
                .Setup<IQueryable<Person>>(
                    "ApplySortingWrapper",
                    ItExpr.IsAny<IQueryable<Person>>(),
                    ItExpr.IsAny<string?>(),
                    ItExpr.IsAny<bool>())
                .Throws(new Exception());

            Func<Task> act = async () => await mockRepository.Object.GetForServiceAsync(gridParams);

            var exception = await act.Should().ThrowAsync<UnexpectedException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.InternalError);

            mockRepository.Protected()
                .Verify(
                "ApplyFiltersWithoutCompanyIdWrapper",
                Times.Once(),
                ItExpr.IsAny<IQueryable<Person>>(),
                ItExpr.IsAny<IEnumerable<FilterParam>>());

            mockRepository.Protected()
                .Verify(
                "ApplySortingWrapper",
                Times.Once(),
                ItExpr.IsAny<IQueryable<Person>>(),
                ItExpr.IsAny<string?>(),
                ItExpr.IsAny<bool>());

            mockRepository.Protected()
                .Verify(
                "NormalizePagingWrapper",
                Times.Never(),
                ItExpr.IsAny<int>(),
                ItExpr.IsAny<int>(),
                ItExpr.IsAny<int>());
        }

        [Xunit.Theory]
        [InlineData("id", "eq", 1)]
        [InlineData("id", "eq", 3)]
        [InlineData("id", "in", "6")]
        public async Task GetForServiceAsync_WhenAnyExceptionIsThrownInNormalizePagingWrapper(string field, string op, object value)
        {
            var mockRepository = new Mock<PersonRepository>(_dbContext, _userAuthContext) { CallBase = true };

            await _fixture.SeedInMemoryDatabase(_dbContext, 1, 3, 6);

            var gridParams = _fixture.CreateGridParamsWithOneFilter(new FilterParam { Field = field, Operator = op, Value = value });

            mockRepository.Protected()
                .Setup<(int, int, int)>(
                    "NormalizePagingWrapper",
                    ItExpr.IsAny<int>(),
                    ItExpr.IsAny<int>(),
                    ItExpr.IsAny<int>())
                .Throws(new Exception());

            Func<Task> act = async () => await mockRepository.Object.GetForServiceAsync(gridParams);

            var exception = await act.Should().ThrowAsync<UnexpectedException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.InternalError);

            mockRepository.Protected()
                .Verify(
                "ApplyFiltersWithoutCompanyIdWrapper",
                Times.Once(),
                ItExpr.IsAny<IQueryable<Person>>(),
                ItExpr.IsAny<IEnumerable<FilterParam>>());

            mockRepository.Protected()
                .Verify(
                "ApplySortingWrapper",
                Times.Once(),
                ItExpr.IsAny<IQueryable<Person>>(),
                ItExpr.IsAny<string?>(),
                ItExpr.IsAny<bool>());

            mockRepository.Protected()
                .Verify(
                "NormalizePagingWrapper",
                Times.Once(),
                ItExpr.IsAny<int>(),
                ItExpr.IsAny<int>(),
                ItExpr.IsAny<int>());
        }
        #endregion
    }

    [Collection(nameof(PersonCollection))]
    public class PersonReadRepositoryTest : ReadWithCompanyRepositoryTests<Person, MySqlContextTest>
    {
        public PersonReadRepositoryTest(PersonFixture fixture) : base(fixture) { }
    }

    [Collection(nameof(PersonCollection))]
    public class PersonWriteRepositoryTest : WriteWithCompanyRepositoryTests<Person, MySqlContextTest>
    {
        public PersonWriteRepositoryTest(PersonFixture fixture) : base(fixture) { }
    }
}
