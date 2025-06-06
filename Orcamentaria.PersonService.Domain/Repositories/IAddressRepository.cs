using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Repositories
{
    public interface IAddressRepository
    {
        int CountItems(long personId);
        IEnumerable<Address> GetByPersonId(long personId);
        Address? GetById(long id);
        Task<Address> Insert(Address address);
        Task<Address> Update(long id, Address address);
        void Delete(Address address);
    }
}
