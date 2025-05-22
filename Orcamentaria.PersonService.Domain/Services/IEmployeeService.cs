using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Employee;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Services
{
    public interface IEmployeeService
    {
        Response<Employee> GetById(long id);
        Response<IEnumerable<Employee>> GetByName(string name);
        Task<Response<Employee>> Insert(EmployeeInsertDTO dto);
        Task<Response<Employee>> Update(long id, EmployeeUpdateDTO dto);
    }
}
