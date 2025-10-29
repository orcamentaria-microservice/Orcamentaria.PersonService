using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Employee;
using Orcamentaria.PersonService.Domain.Services;

namespace Orcamentaria.PersonService.API.Controllers.v1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _service;

        public EmployeeController(IEmployeeService service)
        {
            _service = service;
        }

        [Authorize(Roles = "MASTER,PERSON:READ")]
        [HttpGet("GetById/{id}", Name = "EmployeeGetById")]
        public Response<EmployeeResponseDTO> GetById(int id)
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
        [HttpGet("GetByCompanyId", Name = "EmployeeGetByCompanyId")]
        public Response<IEnumerable<EmployeeResponseDTO>> GetByCompanyId()
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

        [Authorize(Roles = "MASTER,PERSON:READ")]
        [HttpGet("GetByName/{name}", Name = "EmployeeGetByName")]
        public Response<IEnumerable<EmployeeResponseDTO>> GetByName(string name)
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

        [Authorize(Roles = "MASTER,PERSON:CREATE")]
        [HttpPost(Name = "EmployeeInsert")]
        public async Task<Response<EmployeeResponseDTO>> Insert([FromBody] EmployeeInsertDTO dto)
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

        [Authorize(Roles = "MASTER,PERSON:UPDATE")]
        [HttpPut("{id}", Name = "EmployeeUpdate")]
        public async Task<Response<EmployeeResponseDTO>> Update(long id, [FromBody] EmployeeUpdateDTO dto)
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
