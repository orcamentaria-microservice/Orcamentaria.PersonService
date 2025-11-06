using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Infrastructure.Repositories;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Infrastructure.Contexts;

namespace Orcamentaria.PersonService.Infrastructure.Repositories
{
    public class ContactRepository : BasicRepository<Contact>, IContactRepository
    {
        private readonly MySqlContext _dbContext;
        private readonly IUserAuthContext _userAuthContext;

        public ContactRepository(
            MySqlContext dbContext,
            IUserAuthContext userAuthContext)
            : base(dbContext, userAuthContext)
        {
            _dbContext = dbContext;
            _userAuthContext = userAuthContext;
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

        override
        public async Task<Contact> InsertAsync(Contact entity)
        {
            try
            {
                entity.CreatedBy = _userAuthContext.UserId;
                entity.UpdatedBy = _userAuthContext.UserId;
                entity.CreatedAt = DateTime.Now;
                entity.UpdatedAt = DateTime.Now;

                if (!entity.Default)
                {
                    _dbContext.Contacts.Add(entity);
                    await _dbContext.SaveChangesAsync();
                    return entity;
                }

                var conflictDefault = _dbContext.Contacts.FirstOrDefault(x =>
                    x.PersonId == entity.PersonId && x.Type == entity.Type && x.Default);

                if (conflictDefault is null)
                {
                    _dbContext.Contacts.Add(entity);
                    await _dbContext.SaveChangesAsync();
                    return entity;
                }

                conflictDefault.Default = false;
                conflictDefault.UpdatedBy = _userAuthContext.UserId;
                conflictDefault.UpdatedAt = DateTime.Now;
                _dbContext.Contacts.Add(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }


        override
        public async Task<Contact> UpdateAsync(long id, Contact entity)
        {
            try
            {
                entity.UpdatedBy = _userAuthContext.UserId;
                entity.UpdatedAt = DateTime.Now;

                var existing = _dbContext.Contacts.First(p => p.Id == id);

                if(!entity.Default)
                {
                    existing.ContactDescription = entity.ContactDescription;
                    existing.Default = entity.Default;

                    await _dbContext.SaveChangesAsync();
                    return existing;
                }

                var conflictDefault = _dbContext.Contacts.First(x =>
                x.Id != entity.Id && x.PersonId == existing.PersonId && x.Type == existing.Type && x.Default);

                if (conflictDefault is null)
                {
                    existing.ContactDescription = entity.ContactDescription;
                    existing.Default = entity.Default;
                    await _dbContext.SaveChangesAsync();
                    return existing;
                }
                
                conflictDefault.Default = false;
                conflictDefault.UpdatedBy = _userAuthContext.UserId;
                conflictDefault.UpdatedAt = DateTime.Now;
                existing.ContactDescription = entity.ContactDescription;
                existing.Default = entity.Default;

                await _dbContext.SaveChangesAsync();

                return existing;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }
    }
}
