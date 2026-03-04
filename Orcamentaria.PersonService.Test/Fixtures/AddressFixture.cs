using Bogus;
using Orcamentaria.Lib.Test.Fixtures;
using Orcamentaria.PersonService.Domain.Models;
using Xunit;

namespace Orcamentaria.PersonService.Test.Fixtures
{
    [CollectionDefinition(nameof(AddressCollection))]
    public class AddressCollection : ICollectionFixture<AddressFixture> { }

    public class AddressFixture : BaseFixture<Address>
    {
        override
        public Address CreateEntity(long id)
        {
            return new Address
            {
                Id = id,
                Street = Faker.Address.StreetName(),
                ZipCode = Faker.Address.ZipCode(),
                Number = Faker.Random.Number(1, 1000).ToString(),
                City = Faker.Address.City(),
                State = Faker.Address.State(),
                Uf = Faker.Address.StateAbbr(),
                Complement = Faker.Address.SecondaryAddress(),
                Neihborhood = Faker.Address.County(),
                Default = Faker.Random.Bool(),
                PersonId = 1,
                CompanyId = 1,
                CreatedAt = Faker.Date.Past(),
                CreatedBy = 1,
                UpdatedAt = Faker.Date.Future(),
                UpdatedBy = 1
            };
        }
    }
}
