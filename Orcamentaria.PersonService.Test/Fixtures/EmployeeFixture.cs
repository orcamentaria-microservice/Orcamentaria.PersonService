using Orcamentaria.Lib.Test.Fixtures;
using Orcamentaria.PersonService.Domain.Enums;
using Orcamentaria.PersonService.Domain.Models;
using Xunit;

namespace Orcamentaria.PersonService.Test.Fixtures
{
    [CollectionDefinition(nameof(EmployeeCollection))]
    public class EmployeeCollection : ICollectionFixture<EmployeeFixture> { }

    public class EmployeeFixture : BaseFixture<Employee>
    {
        override
        public Employee CreateEntity(long id)
        {
            return new Employee
            {
                Id = id,
                Name = Faker.Name.FirstName(),
                Active = true,
                Rg = Faker.Random.Number(9, 9).ToString(),
                Cpf = Faker.Random.Number(11, 11).ToString(),
                Cnpj = Faker.Random.Number(14, 14).ToString(),
                Type = PersonTypeEnum.Client,
                Post = Faker.Name.JobTitle(),
                AdmissionDate = Faker.Date.Past(),
                ValuePerDay = Faker.Random.Decimal(100, 1000),
                CompanyId = 1,
                CreatedAt = Faker.Date.Past(),
                CreatedBy = 1,
                UpdatedAt = Faker.Date.Future(),
                UpdatedBy = 1
            };
        }
    }
}
