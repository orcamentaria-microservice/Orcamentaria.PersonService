using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.Lib.Domain.Models.Responses;
using Orcamentaria.PersonService.Domain.DTOs.Person;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Services
{
    public interface IPersonService
    {
        Task<Person?> GetByIdAsync(long id);
        Task<Response<IEnumerable<PersonResponseDTO>>?> GetAsync(GridParams gridParams);
        Task<Response<IEnumerable<PersonResponseDTO>>?> GetForServiceAsync(GridParams gridParams);
        Task<Response<PersonResponseDTO>> InsertAsync(PersonInsertDTO dto);
        Task<Response<PersonResponseDTO>> UpdateAsync(long id, PersonUpdateDTO dto);
    }
}
