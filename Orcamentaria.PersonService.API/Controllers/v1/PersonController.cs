using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Person;
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

        [Authorize(Roles = "PERSON:READ")]
        [HttpGet("GetById/{id}", Name = "PersonGetById")]
        public Response<PersonResponseDTO> GetById(int id)
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

        [Authorize(Roles = "PERSON:READ")]
        [HttpGet("GetByCompanyId", Name = "PersonGetByCompanyId")]
        public Response<IEnumerable<PersonResponseDTO>> GetByCompanyId()
        {
            try
            {
                return _service.GetByCompanyId();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "PERSON:READ")]
        [HttpGet("GetByName/{name}", Name = "PersonGetByName")]
        public Response<IEnumerable<PersonResponseDTO>> GetByName(string name)
        {
            try
            {
                return _service.GetByName(name);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Authorize(Roles = "PERSON:INSERT")]
        [HttpPost(Name = "PersonInsert")]
        public async Task<Response<PersonResponseDTO>> Insert([FromBody] PersonInsertDTO dto)
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

        [Authorize(Roles = "PERSON:CREATE")]
        [HttpPut("{id}", Name = "PersonUpdate")]
        public async Task<Response<PersonResponseDTO>> Update(long id, [FromBody] PersonUpdateDTO dto)
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
    }
}
