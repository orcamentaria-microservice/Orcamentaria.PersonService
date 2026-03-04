using AutoMapper;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Moq.AutoMock;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.PersonService.Domain.DTOs.Contact;
using Orcamentaria.PersonService.Domain.DTOs.Person;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Test.Fixtures;
using System.Linq.Expressions;
using Xunit;

namespace Orcamentaria.PersonService.Test.Services
{
    [Collection(nameof(PersonCollection))]
    public class PersonServiceTest
    {
        private readonly PersonFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly Application.Services.PersonService _service;

        public PersonServiceTest(PersonFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _service = _mocker.CreateInstance<Application.Services.PersonService>(true);
        }

        #region GetByIdAsync

        [Xunit.Theory]
        [InlineData(1)]
        public async Task GetByIdAsync_WhenHaveData_ReturnsData(long id)
        {
            var repositoryResponse = _fixture.CreateEntity(id);

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetByIdAsync(id, It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var response = await _service.GetByIdAsync(id);

            response.Should().NotBeNull();
            response.Should().BeSameAs(repositoryResponse);

            _mocker.GetMock<IPersonRepository<Person>>()
                .Verify(r => r.GetByIdAsync(id, It.IsAny<Expression<Func<Person, object>>[]>()), Times.Once());
        }

        [Fact]
        public async Task GetByIdAsync_WhenExceptionOccurs_ThrowsUnexpectedException()
        {
            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<Person, object>>[]>()))
                .ThrowsAsync(new Exception());

            Func<Task> act = async () => await _service.GetByIdAsync(1);

            await act.Should().ThrowAsync<UnexpectedException>();
        }

        #endregion

        #region GetAsync

        [Fact]
        public async Task GetAsync_WhenHaveData_ReturnsSuccessTrue()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (
                new List<Person>() { new Person { Id = 1 } },
                new ResponsePagination(1, 10, 1)
            );

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetAsync(gridParams, It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var response = await _service.GetAsync(gridParams);

            response.Success.Should().BeTrue();
            _mocker.GetMock<IPersonRepository<Person>>()
                .Verify(r => r.GetAsync(gridParams, It.IsAny<Expression<Func<Person, object>>[]>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_WhenNotHaveData_ThrowsInfoException()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (new List<Person>(), new ResponsePagination(1, 10, 0));

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetAsync(gridParams, It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            Func<Task> act = async () => await _service.GetAsync(gridParams);

            var exception = await act.Should().ThrowAsync<InfoException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.NotFound);
        }

        #endregion

        #region GetForServiceAsync

        [Fact]
        public async Task GetForServiceAsync_WhenHaveData_ReturnsSuccessTrue()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (
                new List<Person>() { new Person { Id = 1 } },
                new ResponsePagination(1, 10, 1)
            );

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetForServiceAsync(gridParams, It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var response = await _service.GetForServiceAsync(gridParams);

            response.Success.Should().BeTrue();
            _mocker.GetMock<IPersonRepository<Person>>()
                .Verify(r => r.GetForServiceAsync(gridParams, It.IsAny<Expression<Func<Person, object>>[]>()), Times.Once);
        }

        [Fact]
        public async Task GetForServiceAsync_WhenExceptionOccurs_ThrowsUnexpectedException()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetForServiceAsync(gridParams, It.IsAny<Expression<Func<Person, object>>[]>()))
                .ThrowsAsync(new Exception());

            Func<Task> act = async () => await _service.GetForServiceAsync(gridParams);

            await act.Should().ThrowAsync<UnexpectedException>();
        }

        #endregion

        #region InsertAsync

        [Fact]
        public async Task InsertAsync_WhenValidBody_ReturnsSuccessTrue()
        {
            var serviceRequest = new PersonInsertDTO();
            var mapperInsertToEntity = new Person();
            var validationResult = new ValidationResult();
            var repositoryResponse = new Person { Id = 1, Name = "test" };
            var mapperResponseDTO = new PersonResponseDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<PersonInsertDTO, Person>(serviceRequest))
                .Returns(mapperInsertToEntity);

            _mocker.GetMock<IValidatorEntity<Person>>()
                .Setup(v => v.ValidateBeforeInsert(mapperInsertToEntity))
                .Returns(validationResult);

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.InsertAsync(mapperInsertToEntity))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Person, PersonResponseDTO>(repositoryResponse))
                .Returns(mapperResponseDTO);

            var response = await _service.InsertAsync(serviceRequest);

            response.Success.Should().BeTrue();
            _mocker.GetMock<IPersonRepository<Person>>().Verify(r => r.InsertAsync(It.IsAny<Person>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_WhenInvalidBody_ThrowsValidationException()
        {
            var serviceRequest = new PersonInsertDTO();
            var validationResult = new ValidationResult { Errors = { new ValidationFailure("Name", "Error") } };

            _mocker.GetMock<IMapper>().Setup(m => m.Map<PersonInsertDTO, Person>(serviceRequest)).Returns(new Person());
            _mocker.GetMock<IValidatorEntity<Person>>().Setup(v => v.ValidateBeforeInsert(It.IsAny<Person>())).Returns(validationResult);

            Func<Task> act = async () => await _service.InsertAsync(serviceRequest);

            await act.Should().ThrowAsync<ValidationException>();
        }

        #endregion

        #region UpdateAsync

        [Xunit.Theory]
        [InlineData(1)]
        public async Task UpdateAsync_WhenValidParameterAndBody_ReturnsSuccessTrue(long id)
        {
            var serviceRequest = new PersonUpdateDTO();
            var mapperUpdateToEntity = new Person();
            var validationResult = new ValidationResult();
            var repositoryResponse = new Person { Id = id };
            var mapperResponseDTO = new PersonResponseDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<PersonUpdateDTO, Person>(serviceRequest))
                .Returns(mapperUpdateToEntity);

            _mocker.GetMock<IValidatorEntity<Person>>()
                .Setup(v => v.ValidateBeforeUpdate(It.IsAny<Person>()))
                .Returns(validationResult);

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.UpdateAsync(id, mapperUpdateToEntity))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Person, PersonResponseDTO>(repositoryResponse))
                .Returns(mapperResponseDTO);

            var response = await _service.UpdateAsync(id, serviceRequest);

            response.Success.Should().BeTrue();
            _mocker.GetMock<IPersonRepository<Person>>().Verify(r => r.UpdateAsync(id, It.IsAny<Person>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenExceptionOccurs_ThrowsUnexpectedException()
        {
            _mocker.GetMock<IMapper>().Setup(m => m.Map<PersonUpdateDTO, Person>(It.IsAny<PersonUpdateDTO>())).Throws(new Exception());

            Func<Task> act = async () => await _service.UpdateAsync(1, new PersonUpdateDTO());

            await act.Should().ThrowAsync<UnexpectedException>();
        }

        #endregion
    }
}