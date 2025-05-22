using FluentValidation;
using FluentValidation.Results;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Validators;

namespace Orcamentaria.PersonService.Application.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>, IValidatorEntity<Employee>
    {
        private readonly IEmployeeRepository _repository;

        public EmployeeValidator(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public EmployeeValidator() 
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(100).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.Rg)
                .MaximumLength(9).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.Cpf)
                .MaximumLength(11).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.Rg)
                .MaximumLength(14).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.CompanyId)
                .NotNull().WithMessage("O {PropertyName} é obrigatório.");
            RuleFor(x => x.Post)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .MaximumLength(60).WithMessage("O tamanho máximo do {PropertyName} é de {MaxLength} caracteres.");
            RuleFor(x => x.AdmissionDate.Date)
                .NotEmpty().WithMessage("A {PropertyName} é obrigatório.")
                .LessThanOrEqualTo(DateTime.Now.Date).WithMessage("A {PropertyName} não pode ser posterior a {ComparisonValue}.");
            RuleFor(x => x.ValuePerDay)
                .NotNull().WithMessage("O {PropertyName} é obrigatório.")
                .GreaterThan(0).WithMessage("O {PropertyName} não pode ser menor que {ComparisonValue}.");
        }

        private ValidationResult CommonValidation(Employee entity, bool newRegistry)
        {

            var validator = new EmployeeValidator();

            validator.RuleFor(x => x.Rg)
                .Must((x, cancelation) =>
                {
                    var exists = _repository.GetByRg(x.Rg);

                    if (exists is null)
                        return true;

                    if(newRegistry)
                        return false;

                    return true;
                }).WithMessage("Já possui um registro cadastrado com o mesmo valor do {PropertyName} informado.");

            validator.RuleFor(x => x.Cpf)
                .Must((x, cancelation) =>
                {
                    var exists = _repository.GetByCpf(x.Cpf);

                    if (exists is null)
                        return true;

                    if (newRegistry)
                        return false;

                    return true;
                }).WithMessage("Já possui um registro cadastrado com o mesmo valor do {PropertyName} informado.");

            return validator.Validate(entity);
        }

        public ValidationResult ValidateBeforeInsert(Employee entity)
        {
            RuleFor(x => x.Id)
                .Empty().WithMessage("O {PropertyName} não deve ser informado.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.");

            var resultCommon = CommonValidation(entity, newRegistry: true);

            if (!resultCommon.IsValid)
                return resultCommon;

            return this.Validate(entity);
        }

        public ValidationResult ValidateBeforeUpdate(Employee entity)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O {PropertyName} deve ser informado.");

            RuleFor(x => x.Id)
               .Must((x, cancelation) =>
               {
                   var entity = _repository.GetById(x.Id);

                   return entity is not null;

               }).WithMessage("Id não encontrado.");

            var resultCommon = CommonValidation(entity, newRegistry: false);

            if (!resultCommon.IsValid)
                return resultCommon;

            return this.Validate(entity);
        }
    }
}
