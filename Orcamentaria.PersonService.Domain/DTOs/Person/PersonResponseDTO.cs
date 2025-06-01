using Orcamentaria.PersonService.Domain.DTOs.Address;
using Orcamentaria.PersonService.Domain.DTOs.Contact;
using Orcamentaria.PersonService.Domain.Enums;

namespace Orcamentaria.PersonService.Domain.DTOs.Person
{
    public class PersonResponseDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public long CompanyId { get; set; }
        public PersonTypeEnum Type { get; set; }
        public bool Active { get; set; }
        public IEnumerable<AddressResponseDTO> Addresses { get; set; }
        public IEnumerable<ContactResponseDTO> Contacts { get; set; }
    }
}
