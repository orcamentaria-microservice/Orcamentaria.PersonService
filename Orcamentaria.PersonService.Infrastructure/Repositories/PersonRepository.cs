using Microsoft.EntityFrameworkCore;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Infrastructure.Contexts;

namespace Orcamentaria.PersonService.Infrastructure.Repositories
{
    public class PersonRepository : IPersonRepository
    {
        private readonly MySqlContext _dbContext;
        private readonly ICompanyContext _companyContext;

        public PersonRepository(MySqlContext dbContext, ICompanyContext companyContext) 
        {
            _dbContext = dbContext;
            _companyContext = companyContext;
        }

        public Person GetById(long id) 
            => _dbContext.Persons
            .Include(x => x.Addresses)
            .Include(x => x.Contacts)
            .FirstOrDefault(x => x.Id == id && x.CompanyId == _companyContext.CompanyId);

        public IEnumerable<Person> GetByName(string name) 
            => _dbContext.Persons
            .Include(x => x.Addresses)
            .Include(x => x.Contacts)
            .Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase)
            && x.CompanyId == _companyContext.CompanyId);

        public async Task<Person> Insert(Person person)
        {
            _dbContext.Persons.Add(person);
            await _dbContext.SaveChangesAsync();
            return person;
        }

        public async Task<Person> Update(long id, Person person)
        {
            var entity = _dbContext.Persons.FirstOrDefault(p => p.Id == id);

            if(entity is not null)
            {
                entity.Name = person.Name;
                entity.Rg = person.Rg;
                entity.Cpf = person.Cpf;
                entity.Cnpj = person.Cnpj;
                entity.Type = person.Type;
                entity.Active = person.Active;

                await _dbContext.SaveChangesAsync();
            }

            return entity;
        }
    }
}
