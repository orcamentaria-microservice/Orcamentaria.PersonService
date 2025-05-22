using PersonService.BuildingBlocks.Reponses;
using PersonService.Domain.DTOs.Employee;
using PersonService.Domain.Models;

namespace PersonService.Domain.Services
{
    public interface IEmployeeService
    {
        Response<Employee> GetById(long id);
        Response<IEnumerable<Employee>> GetByName(string name);
        Task<Response<Employee>> Insert(EmployeeInsertDTO dto);
        Task<Response<Employee>> Update(long id, EmployeeUpdateDTO dto);
    }
}
