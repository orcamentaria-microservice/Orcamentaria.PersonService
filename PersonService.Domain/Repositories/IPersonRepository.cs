using PersonService.Domain.Models;

namespace PersonService.Domain.Repositories
{
    public interface IPersonRepository
    {
        Person GetById(long id);
        IEnumerable<Person> GetByName(string name);
        Task<Person> Insert(Person person);
        Task<Person> Update(long id, Person person);
    }
}
