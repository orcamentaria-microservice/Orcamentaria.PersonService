using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.Lib.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Models;
using System.Linq.Expressions;

namespace Orcamentaria.PersonService.Domain.Repositories
{
    public interface IPersonRepository : IBasicRepository<Person>
    {
        Task<(IEnumerable<Person>?, ResponsePagination)> GetForServiceAsync(GridParams gridParams, params Expression<Func<Person, object>>[] includes);
    }
}
