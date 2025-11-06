using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.PersonService.Domain.DTOs.Contact;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Services
{
    public interface IContactService
    {
        Task<Contact?> GetByIdAsync(long id);
        Task<Response<IEnumerable<ContactResponseDTO>>?> GetAsync(GridParams gridParams);
        Task<Response<ContactResponseDTO>> InsertAsync(ContactInsertDTO dto);
        Task<Response<ContactResponseDTO>> UpdateAsync(long id, ContactUpdateDTO dto);
        Task<Response<ContactResponseDTO>> DeleteAsync(long id);
    }
}
