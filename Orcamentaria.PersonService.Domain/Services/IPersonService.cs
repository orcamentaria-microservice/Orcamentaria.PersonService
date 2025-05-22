using Orcamentaria.Lib.Domain.Models;
using Orcamentaria.PersonService.Domain.DTOs.Person;
using Orcamentaria.PersonService.Domain.Models;

namespace Orcamentaria.PersonService.Domain.Services
{
    public interface IPersonService
    {
        Response<Person> GetById(long id);
        Response<IEnumerable<Person>> GetByName(string name);
        Task<Response<Person>> Insert(PersonInsertDTO dto);
        Task<Response<Person>> Update(long id, PersonUpdateDTO dto);
    }
}
