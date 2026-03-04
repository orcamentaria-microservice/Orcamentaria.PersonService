using FluentValidation;
using FluentValidation.Results;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;

namespace Orcamentaria.PersonService.Application.Validators
{
    public class EmployeeValidator : AbstractValidator<Employee>, IValidatorEntity<Employee>
    {
        private readonly IEmployeeRepository<Employee> _repository;

        public EmployeeValidator(IEmployeeRepository<Employee> repository)
        {
            _repository = repository;
        }

        public void CommonValidation(Employee entity)
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

            RuleFor(x => x.Rg)
                .Must((x, cancelation) =>
                {
                    var gridParams = new GridParams
                    {
                        Filters = new List<FilterParam>
                        {
                            new FilterParam
                            {
                                Field = "Rg",
                                Operator = "eq",
                                Value = x.Rg
                            }
                        }
                    };

                    var (data, _) = _repository.GetAsync(gridParams).GetAwaiter().GetResult();

                    // Se não encontrar nenhum RG igual cadastrado, retorna válido
                    if (!data.Any())
                        return true;

                    // Se encontrar pelo menos um RG igual cadastrado e for um insert, retorna inválido
                    if (x.Id == 0)
                        return false;

                    // Se encontrar pelo menos um RG igual cadastrado, for um update e estiver atualizando outra entidade, retorna inválido
                    if (data.FirstOrDefault(p => p.Id == x.Id) is null)
                        return false;

                    // retorno padrão
                    //Se encontrar pelo menos um RG igual cadastrado, for um update e estiver atualizando a mesma entidade encontrada, retorna válido
                    return true;
                }).WithMessage("Já possui um registro cadastrado com o mesmo valor do {PropertyName} informado.");

            RuleFor(x => x.Cpf)
                .Must((x, cancelation) =>
                {
                    var gridParams = new GridParams
                    {
                        Filters = new List<FilterParam>
                        {
                            new FilterParam
                            {
                                Field = "Cpf",
                                Operator = "eq",
                                Value = x.Cpf
                            }
                        }
                    };

                    var (data, _) = _repository.GetAsync(gridParams).GetAwaiter().GetResult();

                    // Se não encontrar nenhum CPF igual cadastrado, retorna válido
                    if (!data.Any())
                        return true;

                    // Se encontrar pelo menos um CPF igual cadastrado e for um insert, retorna inválido
                    if (x.Id == 0)
                        return false;

                    // Se encontrar pelo menos um CPF igual cadastrado, for um update e estiver atualizando outra entidade, retorna inválido
                    if (data.FirstOrDefault(p => p.Id == x.Id) is null)
                        return false;

                    // retorno padrão
                    //Se encontrar pelo menos um CPF igual cadastrado, for um update e estiver atualizando a mesma entidade encontrada, retorna válido
                    return true;
                }).WithMessage("Já possui um registro cadastrado com o mesmo valor do {PropertyName} informado.");
        }

        public ValidationResult ValidateBeforeInsert(Employee entity)
        {
            RuleFor(x => x.Id)
                .Empty().WithMessage("O {PropertyName} não deve ser informado.");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.");

            CommonValidation(entity);

            return Validate(entity);
        }

        public ValidationResult ValidateBeforeUpdate(Employee entity)
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O {PropertyName} deve ser informado.");

            RuleFor(x => x.Id)
               .Must((x, cancelation) =>
               {
                   var entity = _repository.GetByIdAsync(x.Id).GetAwaiter().GetResult();

                   return entity is not null;

               }).WithMessage("Id não encontrado.");

            CommonValidation(entity);

            return Validate(entity);
        }
    }
}
