using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.PersonService.Domain.DTOs.Address;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AddressController : Controller
    {
        private readonly IAddressService _service;

        public AddressController(IAddressService service)
        {
            _service = service;
        }

        [Authorize(Roles = "MASTER,COMPANY_MASTER,PERSON:READ")]
        [HttpPost("Get", Name = "AddressGet")]
        public async Task<Response<IEnumerable<AddressResponseDTO>>?> GetAsync([FromBody] GridParams gridParams)
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
        [HttpPost("Insert", Name = "AddressInsert")]
        public async Task<Response<AddressResponseDTO>> InsertAsync([FromBody] AddressInsertDTO dto)
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
        [HttpPut("Update/{id}", Name = "AddressUpdate")]
        public async Task<Response<AddressResponseDTO>> UpdateAsync(long id, [FromBody] AddressUpdateDTO dto)
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
        [HttpDelete("Delete/{id}", Name = "AddressDelete")]
        public async Task<Response<AddressResponseDTO>> DeleteAsync(long id)
        {
            try
            {
                return await _service.DeleteAsync(id);
            }
            catch (Exception )
            {
                throw;
            }
        }
    }
}
