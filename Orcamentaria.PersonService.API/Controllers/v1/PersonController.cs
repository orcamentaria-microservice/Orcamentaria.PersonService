using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Person;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        private readonly IPersonService _service;

        public PersonController(IPersonService service)
        {
            _service = service;
        }

        [HttpGet("GetById/{id}")]
        public Response<Person> GetById(int id)
            => _service.GetById(id);

        [HttpGet("GetByName/{name}")]
        public Response<IEnumerable<Person>> Get(string name)
            => _service.GetByName(name);

        [HttpPost]
        public async Task<Response<Person>> Insert([FromBody] PersonInsertDTO dto)
            => await _service.Insert(dto);

        [HttpPut("{id}")]
        public async Task<Response<Person>> Update(long id, [FromBody] PersonUpdateDTO dto)
            => await _service.Update(id, dto);
    }
}
