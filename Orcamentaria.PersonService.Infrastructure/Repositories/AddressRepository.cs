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
            => _dbContext.Addresses.Where(x => x.PersonId == personId).Count();

        public void Delete(Address address) => _dbContext.Addresses.Remove(address);

        public Address GetById(long id) => _dbContext.Addresses.FirstOrDefault(x => x.Id == id);

        public IEnumerable<Address> GetByPersonId(long personId)
            => _dbContext.Addresses.Where(x => x.PersonId == personId);

        public async Task<Address> Insert(Address address)
        {
            var entity = _dbContext.Addresses.First(x => 
            x.PersonId == address.PersonId &&
            x.Default);

            if (entity is not null && address.Default)
            {
                entity.Default = false;
                await _dbContext.SaveChangesAsync();
            }

            _dbContext.Addresses.Add(address);
            await _dbContext.SaveChangesAsync();
            return address;
        }

        public async Task<Address> Update(long id, Address address)
        {
            var entity = _dbContext.Addresses.FirstOrDefault(p => p.Id == id);
            var exists = _dbContext.Addresses.First(x => 
            x.Id != address.Id && 
            x.PersonId == entity.PersonId &&
            x.Default);

            if (exists is not null && address.Default)
            {
                exists.Default = false;
                await _dbContext.SaveChangesAsync();
            }


            if(entity is not null)
            {
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
            }

            return entity;
        }
    }
}
