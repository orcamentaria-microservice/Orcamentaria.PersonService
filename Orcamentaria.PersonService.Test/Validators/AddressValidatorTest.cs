using FluentAssertions;
using Moq;
using Moq.AutoMock;
using Orcamentaria.PersonService.Application.Validators;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Test.Fixtures;
using System.Linq.Expressions;
using Xunit;

namespace Orcamentaria.PersonService.Test.Validators
{
    [Collection(nameof(AddressCollection))]
    public class AddressValidatorTest
    {
        private readonly AddressFixture _fixture;
        private readonly AutoMocker _mocker;
        private readonly AddressValidator _validator;

        public AddressValidatorTest(AddressFixture fixture)
        {
            _fixture = fixture;
            _mocker = new AutoMocker();
            _validator = _mocker.CreateInstance<AddressValidator>(true);
        }

        #region ValidateBeforeInsert

        [Fact]
        public void ValidateBeforeInsert_WhenEntityValid_ReturnsIsValid()
        {
            var entity = _fixture.CreateEntity(0);
            entity.ZipCode = "12345678";
            entity.Uf = "SP";

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetByIdAsync(entity.PersonId, It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync(new Person());

            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.CountItems(entity.PersonId))
                .Returns(1);

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeTrue();
            result.Errors.Should().BeEmpty();

            _mocker.GetMock<IPersonRepository<Person>>()
                .Verify(r => r.GetByIdAsync(entity.PersonId, It.IsAny<Expression<Func<Person, object>>[]>()), Times.Once());
        }

        [Fact]
        public void ValidateBeforeInsert_WhenIdIsProvided_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(1);

            _mocker.GetMock<IPersonRepository<Person>>()
                .Setup(r => r.GetByIdAsync(It.IsAny<long>(), It.IsAny<Expression<Func<Person, object>>[]>()))
                .ReturnsAsync(new Person());

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Id não deve ser informado.");
        }

        [Fact]
        public void ValidateBeforeInsert_WhenStreetIsEmpty_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.Street = string.Empty;

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Street é obrigatório.");
        }

        [Fact]
        public void ValidateBeforeInsert_WhenZipCodeIsInvalid_ReturnsInvalidAndErrorMessage()
        {
            var entity = _fixture.CreateEntity(0);
            entity.ZipCode = "abc";

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Zip Code deve ter 8 caracteres.");
            result.Errors.Should().Contain(x => x.ErrorMessage == "O Zip Code deve conter apenas números.");
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

            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.CountItems(entity.PersonId))
                .Returns(4);

            var result = _validator.ValidateBeforeInsert(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Limite de cadastrados atingido (4).");
        }

        #endregion

        #region ValidateBeforeUpdate

        [Fact]
        public void ValidateBeforeUpdate_WhenEntityValid_ReturnsIsValid()
        {
            var entity = _fixture.CreateEntity(1);
            entity.ZipCode = "12345678";
            entity.Uf = "RJ";

            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<Expression<Func<Address, object>>[]>()))
                .ReturnsAsync(new Address());

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

            _mocker.GetMock<IAddressRepository<Address>>()
                .Setup(r => r.GetByIdAsync(entity.Id, It.IsAny<Expression<Func<Address, object>>[]>()))
                .ReturnsAsync((Address)null);

            var result = _validator.ValidateBeforeUpdate(entity);

            result.IsValid.Should().BeFalse();
            result.Errors.Should().Contain(x => x.ErrorMessage == "Id não encontrado.");
        }

        #endregion
    }
}
