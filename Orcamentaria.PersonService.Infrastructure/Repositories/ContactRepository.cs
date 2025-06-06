using Orcamentaria.Lib.Domain.Exceptions;
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
        {
            try
            {
                return _dbContext.Contacts.Where(x => x.PersonId == personId).Count();
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public void Delete(Contact contact)
        {
            try
            {
                _dbContext.Contacts.Remove(contact);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public Contact? GetById(long id)
        {
            try
            {
                return _dbContext.Contacts.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public IEnumerable<Contact> GetByPersonId(long personId)
        {
            try
            {
                return _dbContext.Contacts.Where(x => x.PersonId == personId);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public async Task<Contact> Insert(Contact contact)
        {
            try
            {
                if (!contact.Default)
                {
                    _dbContext.Contacts.Add(contact);
                    await _dbContext.SaveChangesAsync();
                    return contact;
                }

                var entity = _dbContext.Contacts.FirstOrDefault(x =>  
                    x.PersonId == contact.PersonId && x.Type == contact.Type && x.Default);

                if(entity is null)
                    return contact;

                entity.Default = false;
                await _dbContext.SaveChangesAsync();
                return contact;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public async Task<Contact> Update(long id, Contact contact)
        {
            try
            {
                var entity = _dbContext.Contacts.First(p => p.Id == id);
                var exists = _dbContext.Contacts.First(x => 
                x.Id != contact.Id && x.PersonId == entity.PersonId && x.Type == entity.Type && x.Default);
            
                if(exists is not null && contact.Default)
                {
                    exists.Default = false;
                    await _dbContext.SaveChangesAsync();
                }
            
                entity.ContactDescription = contact.ContactDescription;
                entity.Default = contact.Default;

                await _dbContext.SaveChangesAsync();

                return entity;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }
    }
}
