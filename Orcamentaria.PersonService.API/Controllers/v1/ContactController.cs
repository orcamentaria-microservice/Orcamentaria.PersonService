using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Contact;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ContactController : Controller
    {
        private readonly IContactService _service;

        public ContactController(IContactService service)
        {
            _service = service;
        }

        [Authorize(Roles = "MASTER,PERSON:READ")]
        [HttpGet("GetById/{id}", Name = "ContactGetByName")]
        public Response<ContactResponseDTO> GetById(int id)
        {
            try
            {
                return _service.GetById(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "MASTER,PERSON:READ")]
        [HttpGet("GetByPersonId/{personId}", Name = "ContactGetByPersonId")]
        public Response<IEnumerable<ContactResponseDTO>> GetByPersonId(long personId)
        {
            try
            {
                return _service.GetByPersonId(personId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "MASTER,PERSON:CREATE")]
        [HttpPost(Name = "ContactInsert")]
        public async Task<Response<ContactResponseDTO>> Insert([FromBody] ContactInsertDTO dto)
        {
            try
            {
                return await _service.Insert(dto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "MASTER,PERSON:UPATE")]
        [HttpPut("{id}", Name = "ContactUpdate")]
        public async Task<Response<ContactResponseDTO>> Update(long id, [FromBody] ContactUpdateDTO dto)
        {
            try
            {
                return await _service.Update(id, dto);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "MASTER,PERSON:DELETE")]
        [HttpDelete("{id}", Name = "ContactDelete")]
        public Response<ContactResponseDTO> Delete(long id)
        {
            try
            {
                return _service.Delete(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
