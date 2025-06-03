using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Person;

namespace Orcamentaria.PersonService.Domain.Services
{
    public interface IPersonService
    {
        Response<PersonResponseDTO> GetById(long id);
        Response<IEnumerable<PersonResponseDTO>> GetByCompanyId();
        Response<IEnumerable<PersonResponseDTO>> GetByName(string name);
        Task<Response<PersonResponseDTO>> Insert(PersonInsertDTO dto);
        Task<Response<PersonResponseDTO>> Update(long id, PersonUpdateDTO dto);
    }
}
