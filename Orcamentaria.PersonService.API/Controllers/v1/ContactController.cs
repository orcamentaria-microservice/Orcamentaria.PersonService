using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Contact;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.API.Controllers.v1
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
        public Response<ContactResponseDTO> GetById(int id)
            => _service.GetById(id);

        [HttpGet("GetByPersonId/{personId}")]
        public Response<IEnumerable<ContactResponseDTO>> Get(long personId)
            => _service.GetByPersonId(personId);

        [HttpPost]
        public async Task<Response<ContactResponseDTO>> Insert([FromBody] ContactInsertDTO dto)
            => await _service.Insert(dto);

        [HttpPut("{id}")]
        public async Task<Response<ContactResponseDTO>> Update(long id, [FromBody] ContactUpdateDTO dto)
            => await _service.Update(id, dto);

        [HttpDelete("{id}")]
        public Response<ContactResponseDTO> Delete(long id)
            => _service.Delete(id);
    }
}
