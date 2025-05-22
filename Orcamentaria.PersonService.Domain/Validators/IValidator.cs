using FluentValidation.Results;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Validators
{
    public interface IValidatorEntity<T> where T : class
    {
        ValidationResult ValidateBeforeInsert(T entity);
        ValidationResult ValidateBeforeUpdate(T entity);
    }
}
