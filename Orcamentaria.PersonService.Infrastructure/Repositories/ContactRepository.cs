using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Infrastructure.Contexts;

namespace Orcamentaria.PersonService.Infrastructure.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private readonly MySqlContext _dbContext;

        public ContactRepository(MySqlContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public int CountItems(long personId)
            => _dbContext.Contacts.Where(x => x.PersonId == personId).Count();
        public void Delete(Contact contact) => _dbContext.Contacts.Remove(contact);

        public Contact GetById(long id) => _dbContext.Contacts.FirstOrDefault(x => x.Id == id);

        public IEnumerable<Contact> GetByPersonId(long personId)
            => _dbContext.Contacts.Where(x => x.PersonId == personId);

        public async Task<Contact> Insert(Contact contact)
        {
            var entity = _dbContext.Contacts.FirstOrDefault(x => 
            x.PersonId == contact.PersonId && 
            x.Type == contact.Type &&
            x.Default);

            if(entity is not null && contact.Default) 
            {
                entity.Default = false;
                await _dbContext.SaveChangesAsync();
            }

            _dbContext.Contacts.Add(contact);
            await _dbContext.SaveChangesAsync();
            return contact;
        }

        public async Task<Contact> Update(long id, Contact contact)
        {
            var entity = _dbContext.Contacts.FirstOrDefault(p => p.Id == id);
            var exists = _dbContext.Contacts.First(x => 
            x.Id != contact.Id && 
            x.PersonId == entity.PersonId && 
            x.Type == entity.Type &&
            x.Default);
            
            if(exists is not null && contact.Default)
            {
                exists.Default = false;
                await _dbContext.SaveChangesAsync();
            }
            
            if (entity is not null)
            {
                entity.ContactDescription = contact.ContactDescription;
                entity.Default = contact.Default;

                await _dbContext.SaveChangesAsync();
            }

            return entity;
        }
    }
}
