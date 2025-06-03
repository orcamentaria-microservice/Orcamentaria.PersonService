using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Contact;
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

        [Authorize(Roles = "PERSON:READ")]
        [HttpGet("GetById/{id}", Name = "ContactGetByName")]
        public Response<ContactResponseDTO> GetById(int id)
            => _service.GetById(id);

        [Authorize(Roles = "PERSON:READ")]
        [HttpGet("GetByPersonId/{personId}", Name = "ContactGetByPersonId")]
        public Response<IEnumerable<ContactResponseDTO>> GetByPersonId(long personId)
            => _service.GetByPersonId(personId);

        [Authorize(Roles = "PERSON:CREATE")]
        [HttpPost(Name = "ContactInsert")]
        public async Task<Response<ContactResponseDTO>> Insert([FromBody] ContactInsertDTO dto)
            => await _service.Insert(dto);

        [Authorize(Roles = "PERSON:UPATE")]
        [HttpPut("{id}", Name = "ContactUpdate")]
        public async Task<Response<ContactResponseDTO>> Update(long id, [FromBody] ContactUpdateDTO dto)
            => await _service.Update(id, dto);

        [Authorize(Roles = "PERSON:DELETE")]
        [HttpDelete("{id}", Name = "ContactDelete")]
        public Response<ContactResponseDTO> Delete(long id)
            => _service.Delete(id);
    }
}
