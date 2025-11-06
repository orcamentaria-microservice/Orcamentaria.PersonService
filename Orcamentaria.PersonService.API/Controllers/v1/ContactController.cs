using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
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

        [Authorize(Roles = "MASTER,COMPANY_MASTER,PERSON:READ")]
        [HttpPost("Get", Name = "ContactGet")]
        public async Task<Response<IEnumerable<ContactResponseDTO>>?> GetAsync([FromBody] GridParams gridParams)
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

        [Authorize(Roles = "MASTER,COMPANY_MASTER,PERSON:INSERT")]
        [HttpPost("Insert", Name = "ContactInsert")]
        public async Task<Response<ContactResponseDTO>> InsertAsync([FromBody] ContactInsertDTO dto)
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
        [HttpPut("Update/{id}", Name = "ContactUpdate")]
        public async Task<Response<ContactResponseDTO>> UpdateAsync(long id, [FromBody] ContactUpdateDTO dto)
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

        [Authorize(Roles = "MASTER,COMPANY_MASTER,PERSON:DELETE")]
        [HttpDelete("Delete/{id}", Name = "ContactDelete")]
        public async Task<Response<ContactResponseDTO>> DeleteAsync(long id)
        {
            try
            {
                return await _service.DeleteAsync(id);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
