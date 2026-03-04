using FluentValidation;
using FluentValidation.Results;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.PersonService.Domain.Enums;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;

namespace Orcamentaria.PersonService.Application.Validators
{
    public class ContactValidator : AbstractValidator<Contact>, IValidatorEntity<Contact>
    {
        private IContactRepository<Contact> _repository;
        private IPersonRepository<Person> _personRepository;

        public ContactValidator(
            IContactRepository<Contact> repository, 
            IPersonRepository<Person> personRepository)
        {
            _repository = repository;
            _personRepository = personRepository;
        }

        public void CommonValidation(Contact entity)
        {
            RuleFor(x => x.ContactDescription)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(150).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.");
        }

        public ValidationResult ValidateBeforeInsert(Contact entity)
        {
            CommonValidation(entity);

            RuleFor(x => x.Id)
                .Empty().WithMessage("O {PropertyName} não deve ser informado.");

            RuleFor(x => x.PersonId)
                .NotNull().WithMessage("O {PropertyName} é obrigatório.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .Must(x => Enum.IsDefined(typeof(ContactTypeEnum), x)).WithMessage("O {PropertyName} é inválido.");

            RuleFor(x => x.PersonId)
                .Must((x, cancelation) =>
                {
                    var person = _personRepository.GetByIdAsync(x.PersonId).GetAwaiter().GetResult();

                    return person is not null;

                }).WithMessage("PersonId não encontrado.");

            RuleFor(x => x)
                .Must((x, cancelation) =>
                {
                    var count = _repository.CountItems(x.PersonId);

                    return count < 6;

                }).WithMessage("Limite de cadastrados atingido (6).");

            return Validate(entity);
        }

        public ValidationResult ValidateBeforeUpdate(Contact entity)
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

            return this.Validate(entity);
        }
    }
}
