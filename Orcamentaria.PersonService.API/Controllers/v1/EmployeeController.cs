using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Employee;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [Authorize(Roles = "PERSON:READ")]
        [HttpGet("GetById/{id}", Name = "EmployeeGetById")]
        public Response<EmployeeResponseDTO> GetById(int id)
            => _service.GetById(id);

        [Authorize(Roles = "PERSON:READ")]
        [HttpGet("GetByCompanyId", Name = "EmployeeGetByCompanyId")]
        public Response<IEnumerable<EmployeeResponseDTO>> GetByCompanyId()
           => _service.GetByCompanyId();

        [Authorize(Roles = "PERSON:READ")]
        [HttpGet("GetByName/{name}", Name = "EmployeeGetByName")]
        public Response<IEnumerable<EmployeeResponseDTO>> GetByName(string name)
            => _service.GetByName(name);

        [Authorize(Roles = "PERSON:CREATE")]
        [HttpPost(Name = "EmployeeInsert")]
        public async Task<Response<EmployeeResponseDTO>> Insert([FromBody] EmployeeInsertDTO dto)
            => await _service.Insert(dto);

        [Authorize(Roles = "PERSON:UPDATE")]
        [HttpPut("{id}", Name = "EmployeeUpdate")]
        public async Task<Response<EmployeeResponseDTO>> Update(long id, [FromBody] EmployeeUpdateDTO dto)
            => await _service.Update(id, dto);
    }
}
