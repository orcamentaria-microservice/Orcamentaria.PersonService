using PersonService.BuildingBlocks.Reponses;
using PersonService.Domain.DTOs.Contact;
using PersonService.Domain.Models;

namespace PersonService.Domain.Services
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
