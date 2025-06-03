using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
            => _service.GetById(id);

        [Authorize(Roles = "PERSON:READ")]
        [HttpGet("GetByPersonId/{personId}", Name = "AddressGetByPersonId")]
        public Response<IEnumerable<AddressResponseDTO>> GetByPersonId(long personId)
            => _service.GetByPersonId(personId);

        [Authorize(Roles = "PERSON:INSERT")]
        [HttpPost(Name = "AddressInsert")]
        public async Task<Response<AddressResponseDTO>> Insert([FromBody] AddressInsertDTO dto)
            => await _service.Insert(dto);

        [Authorize(Roles = "PERSON:UPDATE")]
        [HttpPut("{id}", Name = "AddressUpdate")]
        public async Task<Response<AddressResponseDTO>> Update(long id, [FromBody] AddressUpdateDTO dto)
            => await _service.Update(id, dto);

        [Authorize(Roles = "PERSON:DELETE")]
        [HttpDelete("{id}", Name = "AddressDelete")]
        public Response<AddressResponseDTO> Delete(long id)
            => _service.Delete(id);
    }
}
