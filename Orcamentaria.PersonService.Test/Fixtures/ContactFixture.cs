using Orcamentaria.Lib.Test.Fixtures;
using Orcamentaria.PersonService.Domain.Enums;
using Orcamentaria.PersonService.Domain.Models;
using Xunit;

namespace Orcamentaria.PersonService.Test.Fixtures
{
    [CollectionDefinition(nameof(ContactCollection))]
    public class ContactCollection : ICollectionFixture<ContactFixture> { }

    public class ContactFixture : BaseFixture<Contact>
    {
        override
        public Contact CreateEntity(long id)
        {
            return new Contact
            {
                Id = id,
                ContactDescription = Faker.Internet.Email(),
                Type = ContactTypeEnum.Email,
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
