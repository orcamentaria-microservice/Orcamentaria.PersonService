using FluentValidation;
using FluentValidation.Results;
using Orcamentaria.Lib.Domain.Models;
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
                .WithMessage("O {PropertyName} deve conter apenas números.")
                .Must((x, cancelation) =>
                {
                    if (string.IsNullOrEmpty(x.Rg))
                        return true;

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
                .Length(11).WithMessage("O {PropertyName} deve ter {MaxLength} caracteres.")
                .Matches("^[0-9]+$")
                .When(x => !string.IsNullOrEmpty(x.Cpf))
                .WithMessage("O {PropertyName} deve conter apenas números.")
                .Must((x, cancelation) =>
                {
                    if (string.IsNullOrEmpty(x.Cpf))
                        return true;

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
            
            RuleFor(x => x.Cnpj)
                .Length(14).WithMessage("O {PropertyName} deve ter {MaxLength} caracteres.")
                .Matches("^[0-9]+$")
                .When(x => !string.IsNullOrEmpty(x.Cnpj))
                .WithMessage("O {PropertyName} deve conter apenas números.")
                .Must((x, cancelation) =>
                {
                    if (string.IsNullOrEmpty(x.Cnpj))
                        return true;

                    var gridParams = new GridParams
                    {
                        Filters = new List<FilterParam>
                        {
                            new FilterParam
                            {
                                Field = "Cnpj",
                                Operator = "eq",
                                Value = x.Cnpj
                            }
                        }
                    };

                    var (data, _) = _repository.GetAsync(gridParams).GetAwaiter().GetResult();

                    // Se não encontrar nenhum CNPJ igual cadastrado, retorna válido
                    if (!data.Any())
                        return true;

                    // Se encontrar pelo menos um CNPJ igual cadastrado e for um insert, retorna inválido
                    if (x.Id == 0)
                        return false;

                    // Se encontrar pelo menos um CNPJ igual cadastrado, for um update e estiver atualizando outra entidade, retorna inválido
                    if (data.FirstOrDefault(p => p.Id == x.Id) is null)
                        return false;

                    // retorno padrão
                    //Se encontrar pelo menos um CNPJ igual cadastrado, for um update e estiver atualizando a mesma entidade encontrada, retorna válido
                    return true;
                }).WithMessage("Já possui um registro cadastrado com o mesmo valor do {PropertyName} informado.");
            
            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("O {PropertyName} é obrigatório.")
                .Must(x => Enum.IsDefined(typeof(PersonTypeEnum), x)).WithMessage("O {PropertyName} é inválido.");
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
