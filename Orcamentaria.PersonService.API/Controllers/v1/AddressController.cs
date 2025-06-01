using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Address;
using Orcamentaria.PersonService.Domain.Models;
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

        [HttpGet("GetById/{id}")]
        public Response<AddressResponseDTO> GetById(int id)
            => _service.GetById(id);

        [HttpGet("GetByPersonId/{personId}")]
        public Response<IEnumerable<AddressResponseDTO>> Get(long personId)
            => _service.GetByPersonId(personId);

        [HttpPost]
        public async Task<Response<AddressResponseDTO>> Insert([FromBody] AddressInsertDTO dto)
            => await _service.Insert(dto);

        [HttpPut("{id}")]
        public async Task<Response<AddressResponseDTO>> Update(long id, [FromBody] AddressUpdateDTO dto)
            => await _service.Update(id, dto);

        [HttpDelete("{id}")]
        public Response<AddressResponseDTO> Delete(long id)
            => _service.Delete(id);
    }
}
