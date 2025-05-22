using PersonService.BuildingBlocks.Reponses;
using PersonService.Domain.DTOs.Person;
using PersonService.Domain.Models;

namespace PersonService.Domain.Services
{
    public interface IPersonService
    {
        Response<Person> GetById(long id);
        Response<IEnumerable<Person>> GetByName(string name);
        Task<Response<Person>> Insert(PersonInsertDTO dto);
        Task<Response<Person>> Update(long id, PersonUpdateDTO dto);
    }
}
