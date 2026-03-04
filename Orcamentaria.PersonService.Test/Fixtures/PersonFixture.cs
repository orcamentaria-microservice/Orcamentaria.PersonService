using Orcamentaria.Lib.Test.Fixtures;
using Orcamentaria.PersonService.Domain.Enums;
using Orcamentaria.PersonService.Domain.Models;
using Xunit;

namespace Orcamentaria.PersonService.Test.Fixtures
{
    [CollectionDefinition(nameof(PersonCollection))]
    public class PersonCollection : ICollectionFixture<PersonFixture> { }

    public class PersonFixture : BaseFixture<Person>
    {
        override
        public Person CreateEntity(long id)
        {
            return new Person
            {
                Id = id,
                Name = Faker.Name.FirstName(),
                Active = true,
                Rg = Faker.Random.Number(9, 9).ToString(),
                Cpf = Faker.Random.Number(11, 11).ToString(),
                Cnpj = Faker.Random.Number(14, 14).ToString(),
                Type = PersonTypeEnum.Client,
                CompanyId = 1,
                CreatedAt = Faker.Date.Past(),
                CreatedBy = 1,
                UpdatedAt = Faker.Date.Future(),
                UpdatedBy = 1
            };
        }
    }
}
