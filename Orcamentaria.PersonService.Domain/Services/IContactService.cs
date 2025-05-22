using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Contact;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Services
{
    public interface IContactService
    {
        Response<IEnumerable<Contact>> GetByPersonId(long personId);
        Response<Contact> GetById(long id);
        Task<Response<Contact>> Insert(ContactInsertDTO dto);
        Task<Response<Contact>> Update(long id, ContactUpdateDTO dto);
        Response<Contact> Delete(long id);

    }
}
