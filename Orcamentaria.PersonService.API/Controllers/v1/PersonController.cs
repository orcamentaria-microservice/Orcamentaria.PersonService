using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.PersonService.Domain.DTOs.Person;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PersonController : Controller
    {
        private readonly IPersonService _service;

        public PersonController(IPersonService service)
        {
            _service = service;
        }

        [Authorize(Roles = "MASTER,COMPANY_MASTER,PERSON:READ")]
        [HttpPost("Get", Name = "PersonGet")]
        public async Task<Response<IEnumerable<PersonResponseDTO>>?> GetAsync([FromBody] GridParams gridParams)
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
        [HttpPost("Insert", Name = "PersonInsert")]
        public async Task<Response<PersonResponseDTO>> InsertAsync([FromBody] PersonInsertDTO dto)
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
        [HttpPut("Update/{id}", Name = "PersonUpdate")]
        public async Task<Response<PersonResponseDTO>> UpdateAsync(long id, [FromBody] PersonUpdateDTO dto)
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
