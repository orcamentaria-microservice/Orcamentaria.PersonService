using FluentValidation;
using FluentValidation.Results;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.PersonService.Domain.Enums;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;

namespace Orcamentaria.PersonService.Application.Validators
{
    public class PersonValidator : AbstractValidator<Person>, IValidatorEntity<Person>
    {
        private readonly IPersonRepository _repository;

        public PersonValidator(IPersonRepository repository)
        {
            _repository = repository;
        }

        public void CommonValidation(Person entity)
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(100).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.Rg)
                .Length(9).WithMessage("O {PropertyName} deve ter {MaxLength} caracteres.")
                .Matches("^[0-9]+$")
                .When(x => !string.IsNullOrEmpty(x.Rg))
                .WithMessage("O {PropertyName} deve conter apenas números.");
            RuleFor(x => x.Cpf)
                .Length(11).WithMessage("O {PropertyName} deve ter {MaxLength} caracteres.")
                .Matches("^[0-9]+$")
                .When(x => !string.IsNullOrEmpty(x.Cpf))
                .WithMessage("O {PropertyName} deve conter apenas números.");
            RuleFor(x => x.Cnpj)
                .Length(9).WithMessage("O {PropertyName} deve ter {MaxLength} caracteres.")
                .Matches("^[0-9]+$")
                .When(x => !string.IsNullOrEmpty(x.Cnpj))
                .WithMessage("O {PropertyName} deve conter apenas números.");
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .Must(x => Enum.IsDefined(typeof(PersonTypeEnum), x)).WithMessage("O {PropertyName} é inválido.");
            RuleFor(x => x.CompanyId)
                .NotNull().WithMessage("O {PropertyName} é obrigatório.")
                .GreaterThan(0).WithMessage("O {PropertyName} é inválido.");
        }

        public ValidationResult ValidateBeforeInsert(Person entity)
        {
            CommonValidation(entity);

            RuleFor(x => x.Id)
                .Empty().WithMessage("O {PropertyName} não deve ser informado.");

            return Validate(entity);
        }

        public ValidationResult ValidateBeforeUpdate(Person entity)
        {
            CommonValidation(entity);

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O {PropertyName} deve ser informado.");

            RuleFor(x => x.Id)
               .Must((x, cancelation) =>
               {
                   var entity = _repository.GetByIdAsync(x.Id).GetAwaiter().GetResult();

                   return entity is not null;

               }).WithMessage("Id não encontrado.");

            return Validate(entity);
        }
    }
}
