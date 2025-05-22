using Orcamentaria.PersonService.Domain.Enums;

namespace Orcamentaria.PersonService.Domain.Models
{
    public class Person
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string? Rg { get; set; }
        public string? Cpf { get; set; }
        public string? Cnpj { get; set; }
        public PersonTypeEnum Type { get; set; }
        public long CompanyId { get; set; }
        public bool Active { get; set; }
        public IEnumerable<Address> Addresses { get; set; }
        public IEnumerable<Contact> Contacts { get; set; }
    }
}
