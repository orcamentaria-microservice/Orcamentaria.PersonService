using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Infrastructure.Repositories;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Infrastructure.Contexts;

namespace Orcamentaria.PersonService.Infrastructure.Repositories
{
    public class EmployeeRepository : BasicRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(
            MySqlContext dbContext, 
            IUserAuthContext userAuthContext)
            : base(dbContext, userAuthContext)
        {
        }
    }
}
