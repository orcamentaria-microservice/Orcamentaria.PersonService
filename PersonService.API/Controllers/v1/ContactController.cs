using Microsoft.AspNetCore.Mvc;
using PersonService.BuildingBlocks.Reponses;
using PersonService.Domain.DTOs.Contact;
using PersonService.Domain.DTOs.Person;
using PersonService.Domain.Models;
using PersonService.Domain.Services;

namespace PersonService.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly IContactService _service;

        public ContactController(IContactService service)
        {
            _service = service;
        }

        [HttpGet("GetById/{id}")]
        public Response<Contact> GetById(int id)
            => _service.GetById(id);

        [HttpGet("GetByPersonId/{personId}")]
        public Response<IEnumerable<Contact>> Get(long personId)
            => _service.GetByPersonId(personId);

        [HttpPost]
        public async Task<Response<Contact>> Insert([FromBody] ContactInsertDTO dto)
            => await _service.Insert(dto);

        [HttpPut("{id}")]
        public async Task<Response<Contact>> Update(long id, [FromBody] ContactUpdateDTO dto)
            => await _service.Update(id, dto);

        [HttpDelete("{id}")]
        public Response<Contact> Delete(long id)
            => _service.Delete(id);
    }
}
