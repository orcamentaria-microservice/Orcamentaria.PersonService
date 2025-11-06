using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.PersonService.Domain.DTOs.Address;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Services
{
    public interface IAddressService
    {
        Task<Address?> GetByIdAsync(long id);
        Task<Response<IEnumerable<AddressResponseDTO>>?> GetAsync(GridParams gridParams);
        Task<Response<AddressResponseDTO>> InsertAsync(AddressInsertDTO dto);
        Task<Response<AddressResponseDTO>> UpdateAsync(long id, AddressUpdateDTO dto);
        Task<Response<AddressResponseDTO>> DeleteAsync(long id);
    }
}
