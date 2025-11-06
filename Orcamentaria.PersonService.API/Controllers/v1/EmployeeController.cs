using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.PersonService.Domain.DTOs.Employee;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [Authorize(Roles = "MASTER,COMPANY_MASTER,PERSON:READ")]
        [HttpPost("Get", Name = "EmployeeGet")]
        public async Task<Response<IEnumerable<EmployeeResponseDTO>>?> GetAsync([FromBody] GridParams gridParams)
        {
            try
            {
                return await _service.GetAsync(gridParams);
            }
            catch (Exception)
            {
                throw;
            }
        }


        [Authorize(Roles = "MASTER,COMPANY_MASTER,PERSON:CREATE")]
        [HttpPost("Insert", Name = "EmployeeInsert")]
        public async Task<Response<EmployeeResponseDTO>> InsertAsync([FromBody] EmployeeInsertDTO dto)
        {
            try
            {
                return await _service.InsertAsync(dto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "MASTER,COMPANY_MASTER,PERSON:UPDATE")]
        [HttpPut("Update/{id}", Name = "EmployeeUpdate")]
        public async Task<Response<EmployeeResponseDTO>> UpdateAsync(long id, [FromBody] EmployeeUpdateDTO dto)
        {
            try
            {
                return await _service.UpdateAsync(id, dto);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
