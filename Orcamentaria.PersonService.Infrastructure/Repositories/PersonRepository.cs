using Microsoft.EntityFrameworkCore;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.Lib.Infrastructure.Repositories;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Infrastructure.Contexts;
using System.Linq.Expressions;

namespace Orcamentaria.PersonService.Infrastructure.Repositories
{
    public class PersonRepository : BaseRepository<Person>, IPersonRepository<Person>
    {
        private readonly MySqlContext _context;

        public PersonRepository(
            MySqlContext context, 
            IUserAuthContext userAuthContext) 
            : base(context, userAuthContext)
        {
            _context = context;
        }

        public async Task<(IEnumerable<Person>?, ResponsePagination)> GetForServiceAsync(GridParams gridParams, params Expression<Func<Person, object>>[] includes)
        {
            try
            {
                var query = _context.Persons.AsQueryable();

                foreach (var include in includes)
                    query = query.Include(include);

                query = ApplyFiltersWithoutCompanyIdWrapper(query, gridParams.Filters);

                query = ApplySortingWrapper(query, gridParams.SortField, gridParams.SortDesc ?? false);

                var (page, pageSize, skip) = NormalizePagingWrapper(gridParams.Page, gridParams.PageSize);

                try
                {
                    return (
                        await query.Skip(skip).Take(pageSize).ToListAsync(),
                        new ResponsePagination(page, pageSize, await query.CountAsync()));
                }
                catch (Exception ex)
                {
                    throw new DatabaseException($"Erro na busca de dados: {ex.Message}");
                }
            }
            catch (DefaultException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new UnexpectedException($"Erro inesperado na busca de dados: {ex.Message}");
            }
        }
    }
}
