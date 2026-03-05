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
    [Collection(nameof(PersonCollection))]
    public class PersonValidatorTest
    {
        private readonly PersonFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly PersonValidator _validator;

        public PersonValidatorTest(PersonFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _validator = _mocker.CreateInstance<PersonValidator>(true);
        }

        #region ValidateBeforeInsert

        [Fact]
        public void ValidateBeforeInsert_WhenEntityValid_ReturnsIsValid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(0);
            entity.Rg = "123456789";
            entity.Cpf = "12345678901";
            entity.Cnpj = "12345678901234";
            entity.Type = PersonTypeEnum.Client;

            // Mock para todas as verificações de duplicidade retornarem vazio
            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync((new List<Person>(), new ResponsePagination(1, 10, 0)));

            // Act
            var result = _validator.ValidateBeforeInsert(entity);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateBeforeInsert_WhenIdIsProvided_ReturnsInvalid()
        {
            var entity = _fixture.CreateEntity(1);
            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Id não deve ser informado.");
        }

        [Xunit.Theory]
        [InlineData("Rg", "12345")] // Curto demais
        [InlineData("Cpf", "123456789012")] // Longo demais
        [InlineData("Cnpj", "A2345678901234")] // Com letras
        public void ValidateBeforeInsert_WhenDocumentsAreInvalid_ReturnsInvalid(string field, string value)
        {
            // Arrange
            var entity = _fixture.CreateEntity(0);
            if (field == "Rg") entity.Rg = value;
            if (field == "Cpf") entity.Cpf = value;
            if (field == "Cnpj") entity.Cnpj = value;

            // Act
            var result = _validator.ValidateBeforeInsert(entity);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Any(x => x.PropertyName == field).Should().BeTrue();
        }

        [Fact]
        public void ValidateBeforeInsert_WhenCpfAlreadyExists_ReturnsInvalid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(0);
            entity.Cpf = "11122233344";
            var existingPerson = new Person { Id = 99, Cpf = entity.Cpf };

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetAsync(It.Is<GridParams>(g => g.Filters.Any(f => f.Value == entity.Cpf)), It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync((new List<Person> { existingPerson }, new ResponsePagination(1, 10, 1)));

            // Act
            var result = _validator.ValidateBeforeInsert(entity);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Já possui um registro cadastrado com o mesmo valor do Cpf informado.");
        }

        #endregion

        #region ValidateBeforeUpdate

        [Fact]
        public void ValidateBeforeUpdate_WhenEntityValid_ReturnsIsValid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(1);

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync(entity);

            // Simula que ao buscar o documento, encontra a própria entidade (válido no update)
            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetAsync(It.IsAny<GridParams>(), It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync((new List<Person> { entity }, new ResponsePagination(1, 10, 1)));

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
            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync((Person)null);

            // Act
            var result = _validator.ValidateBeforeUpdate(entity);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Id não encontrado.");
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenCnpjExistsInAnotherPerson_ReturnsInvalid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(1);
            entity.Cnpj = "11222333000199";
            var otherPerson = new Person { Id = 50, Cnpj = entity.Cnpj };

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync(entity);

            // Encontra outra pessoa com o mesmo CNPJ
            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetAsync(It.Is<GridParams>(g => g.Filters.Any(f => f.Field == "Cnpj")), It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync((new List<Person> { otherPerson }, new ResponsePagination(1, 10, 1)));

            // Act
            var result = _validator.ValidateBeforeUpdate(entity);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Já possui um registro cadastrado com o mesmo valor do Cnpj informado.");
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void CommonValidation_WhenDocumentsAreNull_ShouldBeValid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(1);
            entity.Rg = null;
            entity.Cpf = null;
            entity.Cnpj = null;

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync(entity);

            // Act
            var result = _validator.ValidateBeforeUpdate(entity);

            // Assert
            // Como os campos são opcionais (When !IsNullOrEmpty), deve passar
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void CommonValidation_WhenTypeIsInvalid_ReturnsInvalid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(0);
            entity.Type = (PersonTypeEnum)99; // Enum inexistente

            // Act
            var result = _validator.ValidateBeforeInsert(entity);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Type é inválido.");
        }

        #endregion
    }
}
