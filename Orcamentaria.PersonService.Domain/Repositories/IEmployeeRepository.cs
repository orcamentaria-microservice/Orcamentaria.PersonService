using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Repositories
{
    public interface IEmployeeRepository
    {
        Employee? GetById(long id);
        IEnumerable<Employee> GetByCompanyId();
        IEnumerable<Employee> GetByName(string name);
        Employee? GetByRg(string rg);
        Employee? GetByCpf(string cpf);
        Task<Employee> Insert(Employee employeee);
        Task<Employee> Update(long id, Employee employeee);
    }
}
