using AutoMapper;
using FluentAssertions;
using FluentValidation.Results;
using Moq;
using Moq.AutoMock;
using Orcamentaria.Lib.Domain.Enums;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Exceptions;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.PersonService.Application.Services;
using Orcamentaria.PersonService.Domain.DTOs.Employee;
using Orcamentaria.PersonService.Domain.Enums;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Test.Fixtures;
using System.Linq.Expressions;
using Xunit;

namespace Orcamentaria.PersonService.Test.Services
{
    [Collection(nameof(EmployeeCollection))]
    public class EmployeeServiceTest
    {
        private readonly EmployeeFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly Application.Services.EmployeeService _service;

        public EmployeeServiceTest(EmployeeFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _service = _mocker.CreateInstance<EmployeeService>(true);
        }

        #region GetByIdAsync

        [Xunit.Theory]
        [InlineData(1)]
        [InlineData(10)]
        public async Task GetByIdAsync_WhenHaveData_ReturnsData(long id)
        {
            var repositoryResponse = _fixture.CreateEntity(id);

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.GetByIdAsync(id, It.IsAny<Expression<Func<Employee, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var response = await _service.GetByIdAsync(id);

            response.Should().NotBeNull();
            response.Should().BeSameAs(repositoryResponse);

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Verify(r => r.GetByIdAsync(id, It.IsAny<Expression<Func<Employee, object>>[]>()), Times.Once());
        }

        [Xunit.Theory]
        [InlineData(0)]
        public async Task GetByIdAsync_WhenNotHaveData_ReturnsNull(long id)
        {
            Employee repositoryResponse = null;

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.GetByIdAsync(id, It.IsAny<Expression<Func<Employee, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            var response = await _service.GetByIdAsync(id);

            response.Should().BeNull();

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Verify(r => r.GetByIdAsync(id, It.IsAny<Expression<Func<Employee, object>>[]>()), Times.Once());
        }

        [Xunit.Theory]
        [InlineData(5)]
        public async Task GetByIdAsync_WhenRepositoryThrowsException_ThrowsUnexpectedException(long id)
        {
            var repositoryException = new Exception("Generic error");

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.GetByIdAsync(id, It.IsAny<Expression<Func<Employee, object>>[]>()))
                .ThrowsAsync(repositoryException);

            Func<Task> act = async () => await _service.GetByIdAsync(id);

            await act.Should().ThrowAsync<UnexpectedException>();
        }

        #endregion

        #region GetAsync

        [Fact]
        public async Task GetAsync_WhenHaveData_ReturnsSuccessTrue()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (
                new List<Employee>() { new Employee { Id = 1 } },
                new ResponsePagination(1, 10, 1)
            );
            var mapperResponseDTO = new EmployeeResponseDTO();

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<Employee, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Employee, EmployeeResponseDTO>(It.IsAny<Employee>()))
                .Returns(mapperResponseDTO);

            var response = await _service.GetAsync(gridParams);

            response.Success.Should().BeTrue();
            response.Data.Should().HaveCount(repositoryResponse.Item1.Count());

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Verify(r => r.GetAsync(gridParams, It.IsAny<Expression<Func<Employee, object>>[]>()), Times.Once);
        }

        [Fact]
        public async Task GetAsync_WhenNotHaveData_ThrowsInfoException()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (new List<Employee>(), new ResponsePagination(1, 10, 0));

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<Employee, object>>[]>()))
                .ReturnsAsync(repositoryResponse);

            Func<Task> act = async () => await _service.GetAsync(gridParams);

            var exception = await act.Should().ThrowAsync<InfoException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.NotFound);
        }

        [Fact]
        public async Task GetAsync_WhenRepositoryThrowsException_ThrowsUnexpectedException()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<Employee, object>>[]>()))
                .ThrowsAsync(new Exception());

            Func<Task> act = async () => await _service.GetAsync(gridParams);

            await act.Should().ThrowAsync<UnexpectedException>();
        }

        #endregion

        #region InsertAsync

        [Fact]
        public async Task InsertAsync_WhenValidBody_ReturnsSuccessTrue_And_SetsEmployeeType()
        {
            var serviceRequest = new EmployeeInsertDTO();
            var mapperInsertToEntity = new Employee();
            var validationResult = new ValidationResult();
            var repositoryResponse = new Employee { Id = 1 };
            var mapperResponseDTO = new EmployeeResponseDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EmployeeInsertDTO, Employee>(serviceRequest))
                .Returns(mapperInsertToEntity);

            _mocker.GetMock<IValidatorEntity<Employee>>()
                .Setup(v => v.ValidateBeforeInsert(It.IsAny<Employee>()))
                .Returns(validationResult);

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.InsertAsync(It.IsAny<Employee>()))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Employee, EmployeeResponseDTO>(repositoryResponse))
                .Returns(mapperResponseDTO);

            var response = await _service.InsertAsync(serviceRequest);

            response.Success.Should().BeTrue();

            _mocker.GetMock<IValidatorEntity<Employee>>()
                .Verify(v => v.ValidateBeforeInsert(It.Is<Employee>(e => e.Type == PersonTypeEnum.Employee)), Times.Once);

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Verify(r => r.InsertAsync(It.Is<Employee>(e => e.Type == PersonTypeEnum.Employee)), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_WhenInvalidBody_ThrowsValidationException()
        {
            var serviceRequest = new EmployeeInsertDTO();
            var mapperInsertToEntity = new Employee();
            var validationResult = new ValidationResult { Errors = { new ValidationFailure("Prop", "Error") } };

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EmployeeInsertDTO, Employee>(serviceRequest))
                .Returns(mapperInsertToEntity);

            _mocker.GetMock<IValidatorEntity<Employee>>()
                .Setup(v => v.ValidateBeforeInsert(It.IsAny<Employee>()))
                .Returns(validationResult);

            Func<Task> act = async () => await _service.InsertAsync(serviceRequest);

            await act.Should().ThrowAsync<ValidationException>();
            _mocker.GetMock<IEmployeeRepository<Employee>>().Verify(r => r.InsertAsync(It.IsAny<Employee>()), Times.Never);
        }

        [Fact]
        public async Task InsertAsync_WhenGenericExceptionOccurs_ThrowsUnexpectedException()
        {
            var serviceRequest = new EmployeeInsertDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EmployeeInsertDTO, Employee>(serviceRequest))
                .Throws(new Exception());

            Func<Task> act = async () => await _service.InsertAsync(serviceRequest);

            await act.Should().ThrowAsync<UnexpectedException>();
        }

        #endregion

        #region UpdateAsync

        [Xunit.Theory]
        [InlineData(1)]
        public async Task UpdateAsync_WhenValidParameterAndBody_ReturnsSuccessTrue(long id)
        {
            var serviceRequest = new EmployeeUpdateDTO();
            var mapperUpdateToEntity = new Employee();
            var validationResult = new ValidationResult();
            var repositoryResponse = new Employee { Id = id };
            var mapperResponseDTO = new EmployeeResponseDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EmployeeUpdateDTO, Employee>(serviceRequest))
                .Returns(mapperUpdateToEntity);

            _mocker.GetMock<IValidatorEntity<Employee>>()
                .Setup(v => v.ValidateBeforeUpdate(It.Is<Employee>(e => e.Id == id)))
                .Returns(validationResult);

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.UpdateAsync(id, mapperUpdateToEntity))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Employee, EmployeeResponseDTO>(repositoryResponse))
                .Returns(mapperResponseDTO);

            var response = await _service.UpdateAsync(id, serviceRequest);

            response.Success.Should().BeTrue();
            _mocker.GetMock<IEmployeeRepository<Employee>>().Verify(r => r.UpdateAsync(id, It.IsAny<Employee>()), Times.Once);
        }

        [Xunit.Theory]
        [InlineData(1)]
        public async Task UpdateAsync_WhenGenericExceptionOccurs_ThrowsUnexpectedException(long id)
        {
            var serviceRequest = new EmployeeUpdateDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<EmployeeUpdateDTO, Employee>(serviceRequest))
                .Throws(new Exception());

            Func<Task> act = async () => await _service.UpdateAsync(id, serviceRequest);

            await act.Should().ThrowAsync<UnexpectedException>();
        }

        #endregion
    }
}
