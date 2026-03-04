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
using Orcamentaria.PersonService.Domain.DTOs.Contact;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Test.Fixtures;
using Xunit;

namespace Orcamentaria.PersonService.Test.Services
{
    [Collection(nameof(ContactCollection))]
    public class ContactServiceTest
    {
        private readonly ContactFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly ContactService _service;

        public ContactServiceTest(ContactFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _service = _mocker.CreateInstance<ContactService>(true);
        }

        #region GetByIdAsync

        [Xunit.Theory]
        [InlineData(1)]
        [InlineData(10)]
        public async Task GetByIdAsync_WhenHaveData_ReturnsData(long id)
        {
            var repositoryResponse = _fixture.CreateEntity(id);

            _mocker.GetMock<IContactRepository<Contact>>()
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(repositoryResponse);

            var response = await _service.GetByIdAsync(id);

            response.Should().NotBeNull();
            response.Should().BeSameAs(repositoryResponse);

            _mocker.GetMock<IContactRepository<Contact>>()
                .Verify(r => r.GetByIdAsync(id), Times.Once());
        }

        [Xunit.Theory]
        [InlineData(0)]
        public async Task GetByIdAsync_WhenNotHaveData_ReturnsNull(long id)
        {
            Contact repositoryResponse = null;

            _mocker.GetMock<IContactRepository<Contact>>()
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(repositoryResponse);

            var response = await _service.GetByIdAsync(id);

            response.Should().BeNull();

            _mocker.GetMock<IContactRepository<Contact>>()
                .Verify(r => r.GetByIdAsync(id), Times.Once());
        }

        [Xunit.Theory]
        [InlineData(5)]
        public async Task GetByIdAsync_WhenRepositoryThrowsException_ThrowsUnexpectedException(long id)
        {
            var repositoryException = new Exception("Generic error");

            _mocker.GetMock<IContactRepository<Contact>>()
                .Setup(r => r.GetByIdAsync(id))
                .ThrowsAsync(repositoryException);

            Func<Task> act = async () => await _service.GetByIdAsync(id);

            await act.Should().ThrowAsync<UnexpectedException>();

            _mocker.GetMock<IContactRepository<Contact>>()
                .Verify(r => r.GetByIdAsync(id), Times.Once());
        }

        #endregion

        #region GetAsync

        [Fact]
        public async Task GetAsync_WhenHaveData_ReturnsSuccessTrue()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (
                new List<Contact>() { new Contact { Id = 1 } },
                new ResponsePagination(1, 10, 1)
            );
            var mapperResponseDTO = new ContactResponseDTO();

            _mocker.GetMock<IContactRepository<Contact>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>()))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Contact, ContactResponseDTO>(It.IsAny<Contact>()))
                .Returns(mapperResponseDTO);

            var response = await _service.GetAsync(gridParams);

            response.Success.Should().BeTrue();
            response.Data.Should().HaveCount(repositoryResponse.Item1.Count());

            _mocker.GetMock<IContactRepository<Contact>>()
                .Verify(r => r.GetAsync(gridParams), Times.Once);
        }

        [Fact]
        public async Task GetAsync_WhenNotHaveData_ThrowsInfoException()
        {
            var gridParams = _fixture.CreateGridParamsWithWithoutFilter();
            var repositoryResponse = (new List<Contact>(), new ResponsePagination(1, 10, 0));

            _mocker.GetMock<IContactRepository<Contact>>()
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

            _mocker.GetMock<IContactRepository<Contact>>()
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
            var serviceRequest = new ContactInsertDTO();
            var mapperInsertToEntity = new Contact();
            var validationResult = new ValidationResult();
            var repositoryResponse = new Contact { Id = 1 };
            var mapperResponseDTO = new ContactResponseDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<ContactInsertDTO, Contact>(serviceRequest))
                .Returns(mapperInsertToEntity);

            _mocker.GetMock<IValidatorEntity<Contact>>()
                .Setup(v => v.ValidateBeforeInsert(mapperInsertToEntity))
                .Returns(validationResult);

            _mocker.GetMock<IContactRepository<Contact>>()
                .Setup(r => r.InsertAsync(mapperInsertToEntity))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Contact, ContactResponseDTO>(repositoryResponse))
                .Returns(mapperResponseDTO);

            var response = await _service.InsertAsync(serviceRequest);

            response.Success.Should().BeTrue();
            response.Data.Should().BeSameAs(mapperResponseDTO);

            _mocker.GetMock<IContactRepository<Contact>>().Verify(r => r.InsertAsync(It.IsAny<Contact>()), Times.Once);
        }

        [Fact]
        public async Task InsertAsync_WhenInvalidBody_ThrowsValidationException()
        {
            var serviceRequest = new ContactInsertDTO();
            var mapperInsertToEntity = new Contact();
            var validationResult = new ValidationResult { Errors = { new ValidationFailure("Prop", "Error") } };

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<ContactInsertDTO, Contact>(serviceRequest))
                .Returns(mapperInsertToEntity);

            _mocker.GetMock<IValidatorEntity<Contact>>()
                .Setup(v => v.ValidateBeforeInsert(mapperInsertToEntity))
                .Returns(validationResult);

            Func<Task> act = async () => await _service.InsertAsync(serviceRequest);

            await act.Should().ThrowAsync<ValidationException>();
            _mocker.GetMock<IContactRepository<Contact>>().Verify(r => r.InsertAsync(It.IsAny<Contact>()), Times.Never);
        }

        [Fact]
        public async Task InsertAsync_WhenGenericExceptionOccurs_ThrowsUnexpectedException()
        {
            var serviceRequest = new ContactInsertDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<ContactInsertDTO, Contact>(serviceRequest))
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
            var serviceRequest = new ContactUpdateDTO();
            var mapperUpdateToEntity = new Contact();
            var validationResult = new ValidationResult();
            var repositoryResponse = new Contact { Id = id };
            var mapperResponseDTO = new ContactResponseDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<ContactUpdateDTO, Contact>(serviceRequest))
                .Returns(mapperUpdateToEntity);

            _mocker.GetMock<IValidatorEntity<Contact>>()
                .Setup(v => v.ValidateBeforeUpdate(It.Is<Contact>(c => c.Id == id)))
                .Returns(validationResult);

            _mocker.GetMock<IContactRepository<Contact>>()
                .Setup(r => r.UpdateAsync(id, mapperUpdateToEntity))
                .ReturnsAsync(repositoryResponse);

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<Contact, ContactResponseDTO>(repositoryResponse))
                .Returns(mapperResponseDTO);

            var response = await _service.UpdateAsync(id, serviceRequest);

            response.Success.Should().BeTrue();
            _mocker.GetMock<IContactRepository<Contact>>().Verify(r => r.UpdateAsync(id, It.IsAny<Contact>()), Times.Once);
        }

        [Xunit.Theory]
        [InlineData(1)]
        public async Task UpdateAsync_WhenGenericExceptionOccurs_ThrowsUnexpectedException(long id)
        {
            var serviceRequest = new ContactUpdateDTO();

            _mocker.GetMock<IMapper>()
                .Setup(m => m.Map<ContactUpdateDTO, Contact>(serviceRequest))
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
            var repositoryResponse = new Contact { Id = id };

            _mocker.GetMock<IContactRepository<Contact>>()
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync(repositoryResponse);

            var response = await _service.DeleteAsync(id);

            response.Success.Should().BeTrue();
            _mocker.GetMock<IContactRepository<Contact>>().Verify(r => r.DeleteAsync(id), Times.Once);
        }

        [Xunit.Theory]
        [InlineData(1)]
        public async Task DeleteAsync_WhenIdNotExists_ThrowsInfoException(long id)
        {
            _mocker.GetMock<IContactRepository<Contact>>()
                .Setup(r => r.GetByIdAsync(id))
                .ReturnsAsync((Contact)null);

            Func<Task> act = async () => await _service.DeleteAsync(id);

            var exception = await act.Should().ThrowAsync<InfoException>();
            exception.Which.ErrorCode.Should().Be((int)ErrorCodeEnum.NotFound);
        }

        [Xunit.Theory]
        [InlineData(1)]
        public async Task DeleteAsync_WhenGenericExceptionOccurs_ThrowsUnexpectedException(long id)
        {
            _mocker.GetMock<IContactRepository<Contact>>()
                .Setup(r => r.GetByIdAsync(id))
                .ThrowsAsync(new Exception());

            Func<Task> act = async () => await _service.DeleteAsync(id);

            await act.Should().ThrowAsync<UnexpectedException>();
        }

        #endregion
    }
}
