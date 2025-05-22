using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Address;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Services
{
    public interface IAddressService
    {
        Response<IEnumerable<Address>> GetByPersonId(long personId);
        Response<Address> GetById(long id);
        Task<Response<Address>> Insert(AddressInsertDTO dto);
        Task<Response<Address>> Update(long id, AddressUpdateDTO dto);
        Response<Address> Delete(long id);
    }
}
