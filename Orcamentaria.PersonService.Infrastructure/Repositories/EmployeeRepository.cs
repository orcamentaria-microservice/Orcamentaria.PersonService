using Microsoft.EntityFrameworkCore;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Infrastructure.Contexts;

namespace Orcamentaria.PersonService.Infrastructure.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly MySqlContext _dbContext;
        private readonly IUserAuthContext _userAuthContext;

        public EmployeeRepository(MySqlContext dbContext, IUserAuthContext userAuthContext) 
        {
            _dbContext = dbContext;
            _userAuthContext = userAuthContext;
        }

        public Employee? GetByRg(string rg)
        {
            try
            {
                return _dbContext.Employees
                    .Include(x => x.Addresses)
                    .Include(x => x.Contacts)
                    .FirstOrDefault(x => x.Rg == rg && x.CompanyId == _userAuthContext.UserCompanyId);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public Employee? GetByCpf(string cpf)
        {
            try
            {
                return _dbContext.Employees
                    .Include(x => x.Addresses)
                    .Include(x => x.Contacts)
                    .FirstOrDefault(x => x.Cpf == cpf && x.CompanyId == _userAuthContext.UserCompanyId);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public Employee? GetById(long id)
        {
            try
            {
                return _dbContext.Employees
                    .Include(x => x.Addresses)
                    .Include(x => x.Contacts)
                    .FirstOrDefault(x => x.Id == id && x.CompanyId == _userAuthContext.UserCompanyId);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public IEnumerable<Employee> GetByCompanyId()
        {
            try
            {
                return _dbContext.Employees
                   .Include(x => x.Addresses)
                   .Include(x => x.Contacts)
                   .Where(x => x.CompanyId == _userAuthContext.UserCompanyId);
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public IEnumerable<Employee> GetByName(string name)
        {
            try
            {
                return _dbContext.Employees
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

        public async Task<Employee> Insert(Employee employee)
        {
            try
            {
                _dbContext.Employees.Add(employee);
                await _dbContext.SaveChangesAsync();
                return employee;
            }
            catch (Exception ex)
            {
                throw new DatabaseException(ex.Message, ex);
            }
        }

        public async Task<Employee> Update(long id, Employee employee)
        {
            try
            {
                var entity = _dbContext.Employees.First(
                    x => x.Id == id && x.CompanyId == _userAuthContext.UserCompanyId);
                
                entity.Name = employee.Name;
                entity.Rg = employee.Rg;
                entity.Cpf = employee.Cpf;
                entity.Active = employee.Active;
                entity.Post = employee.Post;
                entity.AdmissionDate = employee.AdmissionDate;
                entity.ValuePerDay = employee.ValuePerDay;

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
