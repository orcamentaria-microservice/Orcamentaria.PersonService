using FluentValidation.Results;
using PersonService.Domain.Models;

namespace PersonService.Domain.Validators
{
    public interface IValidatorEntity<T> where T : class
    {
        ValidationResult ValidateBeforeInsert(T entity);
        ValidationResult ValidateBeforeUpdate(T entity);
    }
}
