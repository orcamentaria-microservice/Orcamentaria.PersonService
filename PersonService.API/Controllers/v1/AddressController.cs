using Microsoft.AspNetCore.Mvc;
using PersonService.BuildingBlocks.Reponses;
using PersonService.Domain.DTOs.Address;
using PersonService.Domain.DTOs.Contact;
using PersonService.Domain.DTOs.Person;
using PersonService.Domain.Models;
using PersonService.Domain.Services;

namespace PersonService.API.Controllers.v1
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
        public Response<Address> GetById(int id)
            => _service.GetById(id);

        [HttpGet("GetByPersonId/{personId}")]
        public Response<IEnumerable<Address>> Get(long personId)
            => _service.GetByPersonId(personId);

        [HttpPost]
        public async Task<Response<Address>> Insert([FromBody] AddressInsertDTO dto)
            => await _service.Insert(dto);

        [HttpPut("{id}")]
        public async Task<Response<Address>> Update(long id, [FromBody] AddressUpdateDTO dto)
            => await _service.Update(id, dto);

        [HttpDelete("{id}")]
        public Response<Address> Delete(long id)
            => _service.Delete(id);
    }
}
