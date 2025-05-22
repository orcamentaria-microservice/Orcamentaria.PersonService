using Microsoft.AspNetCore.Mvc;
using PersonService.BuildingBlocks.Reponses;
using PersonService.Domain.DTOs.Employee;
using PersonService.Domain.DTOs.Person;
using PersonService.Domain.Models;
using PersonService.Domain.Services;

namespace PersonService.API.Controllers.v1
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
        public Response<Employee> GetById(int id)
            => _service.GetById(id);

        [HttpGet("GetByName/{name}")]
        public Response<IEnumerable<Employee>> Get(string name)
            => _service.GetByName(name);

        [HttpPost]
        public async Task<Response<Employee>> Insert([FromBody] EmployeeInsertDTO dto)
            => await _service.Insert(dto);

        [HttpPut("{id}")]
        public async Task<Response<Employee>> Update(long id, [FromBody] EmployeeUpdateDTO dto)
            => await _service.Update(id, dto);
    }
}
