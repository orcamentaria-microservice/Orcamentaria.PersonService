using Orcamentaria.Lib.Domain.Entities;

namespace Orcamentaria.PersonService.Domain.Models
{
    public class Address : TenantEntity
    {
        public long Id { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Neihborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Uf { get; set; }
        public bool Default { get; set; }
        public long PersonId { get; set; }
    }
}
