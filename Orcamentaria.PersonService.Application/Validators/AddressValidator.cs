using FluentValidation;
using FluentValidation.Results;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Validators;

namespace Orcamentaria.PersonService.Application.Validators
{
    public class AddressValidator : AbstractValidator<Address>, IValidatorEntity<Address>
    {
        private readonly IAddressRepository _repository;
        private IPersonRepository _personRepository;

        public AddressValidator(
            IAddressRepository repository, 
            IPersonRepository personRepository)
        {
            _repository = repository;
            _personRepository = personRepository;
        }

        public AddressValidator() 
        {
            RuleFor(x => x.Street)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(70).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.ZipCode)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .Length(8).WithMessage("O {PropertyName} deve ter {MaxLength} caracteres.")
                .Matches("^[0-9]+$").WithMessage("O {PropertyName} deve conter apenas números.");
            RuleFor(x => x.Number)
                .MaximumLength(6).WithMessage("O tamanho máximo de {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.Complement)
                .MaximumLength(45).WithMessage("O tamanho máximo de {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.Neihborhood)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(100).WithMessage("O tamanho máximo de {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.City)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(100).WithMessage("O tamanho máximo de {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.State)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(45).WithMessage("O tamanho máximo de {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.Uf)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(2).WithMessage("O tamanho máximo de {PropertyName} é de {MaxLength} caracteres.");
        }

        public ValidationResult ValidateBeforeInsert(Address entity)
        {
            RuleFor(x => x.Id)
            .Empty().WithMessage("O {PropertyName} não deve ser informado.");

            RuleFor(x => x.PersonId)
               .NotEmpty().WithMessage("O {PropertyName} é obrigatório.");

            RuleFor(x => x.PersonId)
                .Must((x, cancelation) =>
                {
                    var person = _personRepository.GetById(x.PersonId);

                    return person is not null;

                }).WithMessage("PersonId não encontrado.");

            RuleFor(x => x)
                .Must((x, cancelation) =>
                {
                    var count = _repository.CountItems(x.PersonId);

                    return count < 4;

                }).WithMessage("Limite de cadastrados atingido (4).");

            return this.Validate(entity);
        }

        public ValidationResult ValidateBeforeUpdate(Address entity)
        {
            RuleFor(x => x.Id)
            .NotEmpty().WithMessage("O {PropertyName} deve ser informado.");

            RuleFor(x => x.Id)
                .Must((x, cancelation) =>
                {
                    var entity = _repository.GetById(x.Id);

                    return entity is not null;

                }).WithMessage("Id não encontrado.");

            return this.Validate(entity);
        }
    }
}
