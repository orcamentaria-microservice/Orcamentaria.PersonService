using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Employee;

namespace Orcamentaria.PersonService.Domain.Services
{
    public interface IEmployeeService
    {
        Response<EmployeeResponseDTO> GetById(long id);
        Response<IEnumerable<EmployeeResponseDTO>> GetByCompanyId();
        Response<IEnumerable<EmployeeResponseDTO>> GetByName(string name);
        Task<Response<EmployeeResponseDTO>> Insert(EmployeeInsertDTO dto);
        Task<Response<EmployeeResponseDTO>> Update(long id, EmployeeUpdateDTO dto);
    }
}
