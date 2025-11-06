using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Infrastructure.Repositories;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Infrastructure.Contexts;

namespace Orcamentaria.PersonService.Infrastructure.Repositories
{
    public class AddressRepository : BasicRepository<Address>, IAddressRepository
    {
        private readonly MySqlContext _dbContext;
        private readonly IUserAuthContext _userAuthContext;

        public AddressRepository(
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
                return _dbContext.Addresses.Where(x => x.PersonId == personId).Count();
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        override
        public async Task<Address> InsertAsync(Address entity)
        {
            try
            {
                entity.CreatedBy = _userAuthContext.UserId;
                entity.UpdatedBy = _userAuthContext.UserId;
                entity.CreatedAt = DateTime.Now;
                entity.UpdatedAt = DateTime.Now;

                if (!entity.Default)
                {
                    _dbContext.Addresses.Add(entity);
                    await _dbContext.SaveChangesAsync();
                    return entity;
                }

                var conflictDefault = _dbContext.Addresses.FirstOrDefault(x =>
                    x.PersonId == entity.PersonId && x.Default);

                if (conflictDefault is null)
                {
                    _dbContext.Addresses.Add(entity);
                    await _dbContext.SaveChangesAsync();
                    return entity;
                }

                conflictDefault.Default = false;
                conflictDefault.UpdatedBy = _userAuthContext.UserId;
                conflictDefault.UpdatedAt = DateTime.Now;
                _dbContext.Addresses.Add(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        override
        public async Task<Address> UpdateAsync(long id, Address entity)
        {
            try
            {
                entity.UpdatedBy = _userAuthContext.UserId;
                entity.UpdatedAt = DateTime.Now;

                var existing = _dbContext.Addresses.First(p => p.Id == id);

                if (!entity.Default)
                {
                    existing.Street = entity.Street;
                    existing.ZipCode = entity.ZipCode;
                    existing.Number = entity.Number;
                    existing.Complement = entity.Complement;
                    existing.Neihborhood = entity.Neihborhood;
                    existing.City = entity.City;
                    existing.State = entity.State;
                    existing.Uf = entity.Uf;
                    existing.Default = entity.Default;
                    await _dbContext.SaveChangesAsync();
                    return entity;
                }

                var conflictDefault = _dbContext.Addresses.First(x =>
                x.Id != entity.Id &&
                x.PersonId == existing.PersonId &&
                x.Default);

                if (conflictDefault is null)
                {
                    existing.Street = entity.Street;
                    existing.ZipCode = entity.ZipCode;
                    existing.Number = entity.Number;
                    existing.Complement = entity.Complement;
                    existing.Neihborhood = entity.Neihborhood;
                    existing.City = entity.City;
                    existing.State = entity.State;
                    existing.Uf = entity.Uf;
                    existing.Default = entity.Default;
                    await _dbContext.SaveChangesAsync();
                    return entity;
                }

                conflictDefault.Default = false;
                conflictDefault.UpdatedBy = _userAuthContext.UserId;
                conflictDefault.UpdatedAt = DateTime.Now;
                existing.Street = entity.Street;
                existing.ZipCode = entity.ZipCode;
                existing.Number = entity.Number;
                existing.Complement = entity.Complement;
                existing.Neihborhood = entity.Neihborhood;
                existing.City = entity.City;
                existing.State = entity.State;
                existing.Uf = entity.Uf;
                existing.Default = entity.Default;

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
