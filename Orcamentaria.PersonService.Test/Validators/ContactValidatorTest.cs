using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Orcamentaria.PersonService.Application.Validators;
using Orcamentaria.PersonService.Domain.Enums;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Test.Fixtures;
using System.Linq.Expressions;
using Xunit;

namespace Orcamentaria.PersonService.Test.Validators
{
    [Collection(nameof(ContactCollection))]
    public class ContactValidatorTest
    {
        private readonly ContactFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly ContactValidator _validator;

        public ContactValidatorTest(ContactFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _validator = _mocker.CreateInstance<ContactValidator>(true);
        }

        #region ValidateBeforeInsert

        [Fact]
        public void ValidateBeforeInsert_WhenEntityValid_ReturnsIsValid()
        {
            // Arrange
            var entity = _fixture.CreateEntity(0);
            entity.Type = ContactTypeEnum.Email; // Assume que existe este enum

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetByIdAsync(entity.PersonId, It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync(new Person());

            _mocker.GetMock<IContactRepository<Contact>>()
                .Setup(r => r.CountItems(entity.PersonId))
                .Returns(1);

            // Act
            var result = _validator.ValidateBeforeInsert(entity);

            // Assert
            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();

            _mocker.GetMock<IPersonRepository<Person>>()
                .Verify(r => r.GetByIdAsync(entity.PersonId, It.IsAny<Expression<Func<Person, object>>[]>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenIdIsProvided_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Id não deve ser informado.");
        }

        [Fact]
        public void ValidateBeforeInsert_WhenContactDescriptionIsEmpty_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.ContactDescription = string.Empty;

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Contact Description é obrigatório.");
        }

        [Fact]
        public void ValidateBeforeInsert_WhenTypeIsInvalid_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.Type = (ContactTypeEnum)999; // Valor fora do Enum

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Type é inválido.");
        }

        [Fact]
        public void ValidateBeforeInsert_WhenPersonNotFound_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetByIdAsync(entity.PersonId, It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync((Person)null);

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "PersonId não encontrado.");
        }

        [Fact]
        public void ValidateBeforeInsert_WhenLimitReached_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetByIdAsync(entity.PersonId, It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync(new Person());

            _mocker.GetMock<IContactRepository<Contact>>()
                .Setup(r => r.CountItems(entity.PersonId))
                .Returns(6); // Limite é < 6

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Limite de cadastrados atingido (6).");
        }

        #endregion

        #region ValidateBeforeUpdate

        [Fact]
        public void ValidateBeforeUpdate_WhenEntityValid_ReturnsIsValid()
        {
            var entity = _fixture.CreateEntity(1);

            _mocker.GetMock<IContactRepository<Contact>>()
                .Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<Expression<Func<Contact, object>>[]>()))
                .ReturnsAsync(new Contact());

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenIdIsEmpty_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Id deve ser informado.");
        }

        [Fact]
        public void ValidateBeforeUpdate_WhenIdNotFound_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);

            _mocker.GetMock<IContactRepository<Contact>>()
                .Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<Expression<Func<Contact, object>>[]>()))
                .ReturnsAsync((Contact)null);

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Id não encontrado.");
        }

        #endregion
    }
}
