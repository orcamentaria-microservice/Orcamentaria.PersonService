using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.PersonService.Application.Validators;
using Orcamentaria.PersonService.Domain.Enums;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Test.Fixtures;
using System.Linq.Expressions;
using Xunit;

namespace Orcamentaria.PersonService.Test.Validators
{
    [Collection(nameof(EmployeeCollection))]
    public class EmployeeValidatorTest
    {
        private readonly EmployeeFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly EmployeeValidator _validator;

        public EmployeeValidatorTest(EmployeeFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _validator = _mocker.CreateInstance<EmployeeValidator>(true);
        }

        #region ValidateBeforeInsert

        [Fact]
        public void ValidateBeforeInsert_WhenEntityValid_ReturnsIsValid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(0);
            entity.Type = PersonTypeEnum.Employee;
            entity.AdmissionDate = DateTime.Now.AddDays(-1);
            entity.ValuePerDay = 100;

            // Mock para verificação de duplicidade de RG e CPF (retornando lista vazia)
            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<Employee, object>>[]>()))
                .ReturnsAsync((new List<Employee>(), new ResponsePagination(1, 10, 0)));

            // Act
            var result = _validator.ValidateBeforeInsert(entity);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateBeforeInsert_WhenRequiredFieldsAreEmpty_ReturnsInvalid()
        {
            // Arrange
            var entity = new Employee { Id = 0 }; // Tudo nulo/vazio

            // Act
            var result = _validator.ValidateBeforeInsert(entity);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Name é obrigatório.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Type é obrigatório.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "A Admission Date Date é obrigatório.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Value Per Day não pode ser menor que 0.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Post é obrigatório.");
        }

        [Fact]
        public void ValidateBeforeInsert_WhenAdmissionDateIsFuture_ReturnsInvalid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(0);
            entity.AdmissionDate = DateTime.Now.AddDays(1);

            // Act
            var result = _validator.ValidateBeforeInsert(entity);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage.Contains("não pode ser posterior a"));
        }

        [Fact]
        public void ValidateBeforeInsert_WhenValuePerDayIsZeroOrLess_ReturnsInvalid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(0);
            entity.ValuePerDay = 0;

            // Act
            var result = _validator.ValidateBeforeInsert(entity);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Value Per Day não pode ser menor que 0.");
        }

        [Fact]
        public void ValidateBeforeInsert_WhenRgAlreadyExists_ReturnsInvalid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(0);
            var existingEmployee = new Employee { Id = 99, Rg = entity.Rg };

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.GetAsync(It.Is<GridParams>(g => g.Filters.Any(f => f.Field == "Rg")), It.IsAny<Expression<Func<Employee, object>>[]>()))
                .ReturnsAsync((new List<Employee> { existingEmployee }, new ResponsePagination(1, 10, 1)));

            // Act
            var result = _validator.ValidateBeforeInsert(entity);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Já possui um registro cadastrado com o mesmo valor do Rg informado.");
        }

        #endregion

        #region ValidateBeforeUpdate

        [Fact]
        public void ValidateBeforeUpdate_WhenEntityValid_ReturnsIsValid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(1);
            entity.AdmissionDate = DateTime.Now.AddDays(-1);
            entity.ValuePerDay = 150;

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<Expression<Func<Employee, object>>[]>()))
                .ReturnsAsync(entity);

            // Mock duplicidade (Encontra ele mesmo, o que é válido no Update)
            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<Employee, object>>[]>()))
                .ReturnsAsync((new List<Employee> { entity }, new ResponsePagination(1, 10, 1)));

            // Act
            var result = _validator.ValidateBeforeUpdate(entity);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenIdNotFound_ReturnsInvalid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(1);

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<Expression<Func<Employee, object>>[]>()))
                .ReturnsAsync((Employee)null);

            // Act
            var result = _validator.ValidateBeforeUpdate(entity);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Id não encontrado.");
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenCpfExistsInAnotherEmployee_ReturnsInvalid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(1);
            var otherEmployee = new Employee { Id = 2, Cpf = entity.Cpf };

            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<Expression<Func<Employee, object>>[]>()))
                .ReturnsAsync(entity);

            // Simula que o CPF informado pertence a OUTRO funcionário (Id 2)
            _mocker.GetMock<IEmployeeRepository<Employee>>()
                .Setup(r => r.GetAsync(It.Is<GridParams>(g => g.Filters.Any(f => f.Field == "Cpf")), It.IsAny<Expression<Func<Employee, object>>[]>()))
                .ReturnsAsync((new List<Employee> { otherEmployee }, new ResponsePagination(1, 10, 1)));

            // Act
            var result = _validator.ValidateBeforeUpdate(entity);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Já possui um registro cadastrado com o mesmo valor do Cpf informado.");
        }

        #endregion
    }
}
