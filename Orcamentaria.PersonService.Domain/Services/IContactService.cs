using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Contact;

namespace Orcamentaria.PersonService.Domain.Services
{
    public interface IContactService
    {
        Response<IEnumerable<ContactResponseDTO>> GetByPersonId(long personId);
        Response<ContactResponseDTO> GetById(long id);
        Task<Response<ContactResponseDTO>> Insert(ContactInsertDTO dto);
        Task<Response<ContactResponseDTO>> Update(long id, ContactUpdateDTO dto);
        Response<ContactResponseDTO> Delete(long id);

    }
}
