using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.PersonService.Domain.Models;
using System.Linq.Expressions;

namespace Orcamentaria.PersonService.Domain.Repositories
{
    public interface IPersonRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByIdAsync(long id, params Expression<Func<TEntity, object>>[] includes);
        Task<(IEnumerable<TEntity?>, ResponsePagination pagination)> GetAsync(GridParams gridParams, params Expression<Func<TEntity, object>>[] includes);
        Task<TEntity> InsertAsync(TEntity entity);
        Task<TEntity> UpdateAsync(long id, TEntity entity);
        Task<(IEnumerable<Person>?, ResponsePagination)> GetForServiceAsync(GridParams gridParams, params Expression<Func<Person, object>>[] includes);
    }
}
