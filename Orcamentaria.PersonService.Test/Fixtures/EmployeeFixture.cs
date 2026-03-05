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
                Rg = Faker.Random.ReplaceNumbers("#########"),
                Cpf = Faker.Random.ReplaceNumbers("###########"),
                Cnpj = Faker.Random.ReplaceNumbers("##############"),
                Type = PersonTypeEnum.Employee,
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
