using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.PersonService.Domain.DTOs.Employee;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Services
{
    public interface IEmployeeService
    {
        Task<Employee?> GetByIdAsync(long id);
        Task<Response<IEnumerable<EmployeeResponseDTO>>?> GetAsync(GridParams gridParams);
        Task<Response<EmployeeResponseDTO>> InsertAsync(EmployeeInsertDTO dto);
        Task<Response<EmployeeResponseDTO>> UpdateAsync(long id, EmployeeUpdateDTO dto);
    }
}
