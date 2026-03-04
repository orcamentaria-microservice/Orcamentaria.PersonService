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
using Orcamentaria.PersonService.Domain.DTOs.Address;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Test.Fixtures;
using Xunit;

namespace Orcamentaria.PersonService.Test.Services
{
    
    [Collection(nameof(AddressCollection))]
    public class AddressServiceTest
    {
        private readonly AddressFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly AddressService _service;

        public AddressServiceTest(AddressFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _service = _mocker.CreateInstance<AddressService>(true);
        }

        #region GetByIdAsync

        [Xunit.Theory]
        [InlineData(1)]
        [InlineData(10)]
        public async Task GetByIdAsync_WhenHaveData_ReturnsData(long id)
        {
            var repositoryResponse = _fixture.CreateEntity(id);

            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(repositoryResponse);

            var response = await _service.GetByIdAsync(id);

            response.Should().NotBeNull();
            response.Should().BeSameAs(repositoryResponse);

            _mocker.GetMock<IAddressRepository<Address>>()
                .Verify(r => r.GetByIdAsync(id), Times.Once());
        }

        [Xunit.Theory]
        [InlineData(0)]
        public async Task GetByIdAsync_WhenNotHaveData_ReturnsNull(long id)
        {
            Address repositoryResponse = null;

            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(repositoryResponse);

            var response = await _service.GetByIdAsync(id);

            response.Should().BeNull();

            _mocker.GetMock<IAddressRepository<Address>>()
                .Verify(r => r.GetByIdAsync(id), Times.Once());
        }

        [Xunit.Theory]
        [InlineData(5)]
        public async Task GetByIdAsync_WhenRepositoryThrowsUnexpectedException_PropagatesException(long id)
        {
            var repositoryException = new Exception("Generic error");

            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.GetByIdAsync(id))
                .ThrowsAsync(repositoryException);

            Func<Task> act = async () => await _service.GetByIdAsync(id);

            await act.Should().ThrowAsync<UnexpectedException>();

            _mocker.GetMock<IAddressRepository<Address>>()
                .Verify(r => r.GetByIdAsync(id), Times.Once());
        }

        #endregion

        #region GetAsync

        [Fact]
        public async Task GetAsync_WhenHaveData_ReturnsSuccessTrue()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (
                new List<Address>() { new Address { Id = 1 } },
                new ResponsePagination(1, 10, 1)
            );
            var mapperResponseDTO = new AddressResponseDTO();

            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>()))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Address, AddressResponseDTO>(It.IsAny<Address>()))
                .Returns(mapperResponseDTO);

            var response = await _service.GetAsync(gridParams);

            response.Success.Should().BeTrue();
            response.Data.Should().HaveCount(repositoryResponse.Item1.Count());

            _mocker.GetMock<IAddressRepository<Address>>()
                .Verify(r => r.GetAsync(gridParams), Times.Once);
        }

        [Fact]
        public async Task GetAsync_WhenNotHaveData_ThrowsInfoException()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (new List<Address>(), new ResponsePagination(1, 10, 0));

            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>()))
                .ReturnsAsync(repositoryResponse);

            Func<Task> act = async () => await _service.GetAsync(gridParams);

            var exception = await act.Should().ThrowAsync<InfoException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.NotFound);
        }

        [Fact]
        public async Task GetAsync_WhenRepositoryThrowsException_ThrowsUnexpectedException()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();

            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>()))
                .ThrowsAsync(new Exception());

            Func<Task> act = async () => await _service.GetAsync(gridParams);

            await act.Should().ThrowAsync<UnexpectedException>();
        }

        #endregion

        #region InsertAsync

        [Fact]
        public async Task InsertAsync_WhenValidBody_ReturnsSuccessTrue()
        {
            var serviceRequest = new AddressInsertDTO();
            var mapperInsertToEntity = new Address();
            var validationResult = new ValidationResult();
            var repositoryResponse = new Address { Id = 1 };
            var mapperResponseDTO = new AddressResponseDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<AddressInsertDTO, Address>(serviceRequest))
                .Returns(mapperInsertToEntity);

            _mocker.GetMock<IValidatorEntity<Address>>()
                .Setup(v => v.ValidateBeforeInsert(mapperInsertToEntity))
                .Returns(validationResult);

            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.InsertAsync(mapperInsertToEntity))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Address, AddressResponseDTO>(repositoryResponse))
                .Returns(mapperResponseDTO);

            var response = await _service.InsertAsync(serviceRequest);

            response.Success.Should().BeTrue();
            response.Data.Should().BeSameAs(mapperResponseDTO);
        }

        [Fact]
        public async Task InsertAsync_WhenInvalidBody_ThrowsValidationException()
        {
            var serviceRequest = new AddressInsertDTO();
            var mapperInsertToEntity = new Address();
            var validationResult = new ValidationResult { Errors = { new ValidationFailure("Prop", "Error") } };

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<AddressInsertDTO, Address>(serviceRequest))
                .Returns(mapperInsertToEntity);

            _mocker.GetMock<IValidatorEntity<Address>>()
                .Setup(v => v.ValidateBeforeInsert(mapperInsertToEntity))
                .Returns(validationResult);

            Func<Task> act = async () => await _service.InsertAsync(serviceRequest);

            await act.Should().ThrowAsync<ValidationException>();
            _mocker.GetMock<IAddressRepository<Address>>().Verify(r => r.InsertAsync(It.IsAny<Address>()), Times.Never);
        }

        [Fact]
        public async Task InsertAsync_WhenGenericExceptionOccurs_ThrowsUnexpectedException()
        {
            var serviceRequest = new AddressInsertDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<AddressInsertDTO, Address>(serviceRequest))
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
            var serviceRequest = new AddressUpdateDTO();
            var mapperUpdateToEntity = new Address();
            var validationResult = new ValidationResult();
            var repositoryResponse = new Address { Id = id };
            var mapperResponseDTO = new AddressResponseDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<AddressUpdateDTO, Address>(serviceRequest))
                .Returns(mapperUpdateToEntity);

            _mocker.GetMock<IValidatorEntity<Address>>()
                .Setup(v => v.ValidateBeforeUpdate(It.Is<Address>(a => a.Id == id)))
                .Returns(validationResult);

            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.UpdateAsync(id, mapperUpdateToEntity))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Address, AddressResponseDTO>(repositoryResponse))
                .Returns(mapperResponseDTO);

            var response = await _service.UpdateAsync(id, serviceRequest);

            response.Success.Should().BeTrue();
            _mocker.GetMock<IAddressRepository<Address>>().Verify(r => r.UpdateAsync(id, It.IsAny<Address>()), Times.Once);
        }

        [Xunit.Theory]
        [InlineData(1)]
        public async Task UpdateAsync_WhenGenericExceptionOccurs_ThrowsUnexpectedException(long id)
        {
            var serviceRequest = new AddressUpdateDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<AddressUpdateDTO, Address>(serviceRequest))
                .Throws(new Exception());

            Func<Task> act = async () => await _service.UpdateAsync(id, serviceRequest);

            await act.Should().ThrowAsync<UnexpectedException>();
        }

        #endregion

        #region DeleteAsync

        [Xunit.Theory]
        [InlineData(1)]
        public async Task DeleteAsync_WhenIdExists_ReturnsSuccessTrue(long id)
        {
            var repositoryResponse = new Address { Id = id };

            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(repositoryResponse);

            var response = await _service.DeleteAsync(id);

            response.Success.Should().BeTrue();
            _mocker.GetMock<IAddressRepository<Address>>().Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Xunit.Theory]
        [InlineData(1)]
        public async Task DeleteAsync_WhenIdNotExists_ThrowsInfoException(long id)
        {
            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Address)null);

            Func<Task> act = async () => await _service.DeleteAsync(id);

            var exception = await act.Should().ThrowAsync<InfoException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.NotFound);
        }

        [Xunit.Theory]
        [InlineData(1)]
        public async Task DeleteAsync_WhenGenericExceptionOccurs_ThrowsUnexpectedException(long id)
        {
            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.GetByIdAsync(id))
                .ThrowsAsync(new Exception());

            Func<Task> act = async () => await _service.DeleteAsync(id);

            await act.Should().ThrowAsync<UnexpectedException>();
        }

        #endregion
    }
}
