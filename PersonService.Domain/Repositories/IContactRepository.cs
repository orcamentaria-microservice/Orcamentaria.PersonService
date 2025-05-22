using PersonService.Domain.Models;

namespace PersonService.Domain.Repositories
{
    public interface IContactRepository
    {
        int CountItems(long personId);
        IEnumerable<Contact> GetByPersonId(long personId);
        Contact GetById(long id);
        Task<Contact> Insert(Contact contact);
        Task<Contact> Update(long id, Contact contact);
        void Delete(Contact contact);

    }
}
