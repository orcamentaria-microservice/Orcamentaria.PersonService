using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Infrastructure.Contexts;

namespace Orcamentaria.PersonService.Infrastructure.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly MySqlContext _dbContext;

        public AddressRepository(MySqlContext dbContext)
        {
            _dbContext = dbContext;
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

        public void Delete(Address address) 
        {
            try
            {
                _dbContext.Addresses.Remove(address);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        } 

        public Address? GetById(long id)
        {
            try
            {
                return _dbContext.Addresses.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public IEnumerable<Address> GetByPersonId(long personId)
        {
            try
            {
                return _dbContext.Addresses.Where(x => x.PersonId == personId);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public async Task<Address> Insert(Address address)
        {
            try
            {
                if (!address.Default)
                {
                    _dbContext.Addresses.Add(address);
                    await _dbContext.SaveChangesAsync();
                    return address;
                }
                
                var entity = _dbContext.Addresses.First(x => x.PersonId == address.PersonId && x.Default);

                if (entity is null)
                    return address;

                entity.Default = false;
                await _dbContext.SaveChangesAsync();
                return address;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public async Task<Address> Update(long id, Address address)
        {
            try
            {
                var entity = _dbContext.Addresses.First(p => p.Id == id);
                var exists = _dbContext.Addresses.First(x =>
                x.Id != address.Id &&
                x.PersonId == entity.PersonId &&
                x.Default);

                if (exists is not null && address.Default)
                {
                    exists.Default = false;
                    await _dbContext.SaveChangesAsync();
                }

                entity.Street = address.Street;
                entity.ZipCode = address.ZipCode;
                entity.Number = address.Number;
                entity.Complement = address.Complement;
                entity.Neihborhood = address.Neihborhood;
                entity.City = address.City;
                entity.State = address.State;
                entity.Uf = address.Uf;
                entity.Default = address.Default;

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
