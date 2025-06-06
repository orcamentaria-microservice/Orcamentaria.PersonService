using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Exceptions;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Address;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.API.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AddressController : Controller
    {
        private readonly IAddressService _service;

        public AddressController(IAddressService service)
        {
            _service = service;
        }

        [Authorize(Roles = "PERSON:READ")]
        [HttpGet("GetById/{id}", Name = "AddressGetByName")]
        public Response<AddressResponseDTO> GetById(int id)
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
        [HttpGet("GetByPersonId/{personId}", Name = "AddressGetByPersonId")]
        public Response<IEnumerable<AddressResponseDTO>> GetByPersonId(long personId)
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

        [Authorize(Roles = "PERSON:INSERT")]
        [HttpPost(Name = "AddressInsert")]
        public async Task<Response<AddressResponseDTO>> Insert([FromBody] AddressInsertDTO dto)
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

        [Authorize(Roles = "PERSON:UPDATE")]
        [HttpPut("{id}", Name = "AddressUpdate")]
        public async Task<Response<AddressResponseDTO>> Update(long id, [FromBody] AddressUpdateDTO dto)
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

        [Authorize(Roles = "PERSON:DELETE")]
        [HttpDelete("{id}", Name = "AddressDelete")]
        public Response<AddressResponseDTO> Delete(long id)
        {
            try
            {
                return _service.Delete(id);
            }
            catch (Exception )
            {
                throw;
            }
        }
    }
}
