using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Employee;
using Orcamentaria.PersonService.Domain.Models;
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

        [HttpGet("GetById/{id}")]
        public Response<EmployeeResponseDTO> GetById(int id)
            => _service.GetById(id);

        [HttpGet("GetByName/{name}")]
        public Response<IEnumerable<EmployeeResponseDTO>> Get(string name)
            => _service.GetByName(name);

        [HttpPost]
        public async Task<Response<EmployeeResponseDTO>> Insert([FromBody] EmployeeInsertDTO dto)
            => await _service.Insert(dto);

        [HttpPut("{id}")]
        public async Task<Response<EmployeeResponseDTO>> Update(long id, [FromBody] EmployeeUpdateDTO dto)
            => await _service.Update(id, dto);
    }
}
