using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Address;

namespace Orcamentaria.PersonService.Domain.Services
{
    public interface IAddressService
    {
        Response<IEnumerable<AddressResponseDTO>> GetByPersonId(long personId);
        Response<AddressResponseDTO> GetById(long id);
        Task<Response<AddressResponseDTO>> Insert(AddressInsertDTO dto);
        Task<Response<AddressResponseDTO>> Update(long id, AddressUpdateDTO dto);
        Response<AddressResponseDTO> Delete(long id);
    }
}
