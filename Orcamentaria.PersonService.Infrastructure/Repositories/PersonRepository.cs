using Microsoft.EntityFrameworkCore;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Infrastructure.Contexts;

namespace Orcamentaria.PersonService.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly MySqlContext _dbContext;
        private readonly IUserAuthContext _userAuthContext;

        public PersonRepository(MySqlContext dbContext, IUserAuthContext userAuthContext) 
        {
            _dbContext = dbContext;
            _userAuthContext = userAuthContext;
        }

        public Person? GetById(long id)
        {
            try
            {
                return _dbContext.Persons
                    .Include(x => x.Addresses)
                    .Include(x => x.Contacts)
                    .FirstOrDefault(x => x.Id == id && x.CompanyId == _userAuthContext.UserCompanyId);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public IEnumerable<Person> GetByCompanyId()
        {
            try
            {
                return _dbContext.Persons
                    .Include(x => x.Addresses)
                    .Include(x => x.Contacts)
                    .Where(x => x.CompanyId == _userAuthContext.UserCompanyId);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public IEnumerable<Person> GetByName(string name)
        {
            try
            {
                return _dbContext.Persons
                    .Include(x => x.Addresses)
                    .Include(x => x.Contacts)
                    .Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)
                    && x.CompanyId == _userAuthContext.UserCompanyId);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public async Task<Person> Insert(Person person)
        {
            try
            {
                _dbContext.Persons.Add(person);
                await _dbContext.SaveChangesAsync();
                return person;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public async Task<Person> Update(long id, Person person)
        {
            try
            {
                var entity = _dbContext.Persons.First(p => p.Id == id);

                entity.Name = person.Name;
                entity.Rg = person.Rg;
                entity.Cpf = person.Cpf;
                entity.Cnpj = person.Cnpj;
                entity.Type = person.Type;
                entity.Active = person.Active;

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
