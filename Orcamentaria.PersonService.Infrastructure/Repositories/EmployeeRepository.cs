using Microsoft.EntityFrameworkCore;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Infrastructure.Contexts;

namespace Orcamentaria.PersonService.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MySqlContext _dbContext;
        private readonly ICompanyContext _companyContext;

        public EmployeeRepository(MySqlContext dbContext, ICompanyContext companyContext) 
        {
            _dbContext = dbContext;
            _companyContext = companyContext;
        }

        public Employee GetByRg(string rg)
            => _dbContext.Employees.FirstOrDefault(x => x.Rg == rg && x.CompanyId == _companyContext.CompanyId);

        public Employee GetByCpf(string cpf) 
            => _dbContext.Employees.FirstOrDefault(x => x.Cpf == cpf && x.CompanyId == _companyContext.CompanyId);

        public Employee GetById(long id) 
            => _dbContext.Employees
            .Include(x => x.Addresses)
            .Include(x => x.Contacts)
            .FirstOrDefault(x => x.Id == id && x.CompanyId == _companyContext.CompanyId);

        public IEnumerable<Employee> GetByName(string name) 
            => _dbContext.Employees
            .Include(x => x.Addresses)
            .Include(x => x.Contacts)
            .Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase) && x.CompanyId == _companyContext.CompanyId);


        public async Task<Employee> Insert(Employee employee)
        {
            _dbContext.Employees.Add(employee);
            await _dbContext.SaveChangesAsync();
            return employee;
        }

        public async Task<Employee> Update(long id, Employee employee)
        {
            var entity = _dbContext.Employees.FirstOrDefault(p => p.Id == id && p.CompanyId == _companyContext.CompanyId);

            if(entity is not null)
            {
                entity.Name = employee.Name;
                entity.Rg = employee.Rg;
                entity.Cpf = employee.Cpf;
                entity.Active = employee.Active;
                entity.Post = employee.Post;
                entity.AdmissionDate = employee.AdmissionDate;
                entity.ValuePerDay = employee.ValuePerDay;

                await _dbContext.SaveChangesAsync();
            }

            return entity;
        }
    }
}
