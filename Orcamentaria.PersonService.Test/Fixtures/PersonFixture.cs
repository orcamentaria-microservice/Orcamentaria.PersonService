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
                Rg = Faker.Random.ReplaceNumbers("#########"),
                Cpf = Faker.Random.ReplaceNumbers("###########"),
                Cnpj = Faker.Random.ReplaceNumbers("##############"),
                Type = Faker.Random.Bool() ? PersonTypeEnum.Client : PersonTypeEnum.Supplier,
                CompanyId = 1,
                CreatedAt = Faker.Date.Past(),
                CreatedBy = 1,
                UpdatedAt = Faker.Date.Future(),
                UpdatedBy = 1
            };
        }
    }
}
