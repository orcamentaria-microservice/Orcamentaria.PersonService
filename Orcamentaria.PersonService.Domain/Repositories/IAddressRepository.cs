using Orcamentaria.Lib.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Repositories
{
    public interface IAddressRepository : IBasicRepository<Address>
    {
        int CountItems(long personId);
    }
}
