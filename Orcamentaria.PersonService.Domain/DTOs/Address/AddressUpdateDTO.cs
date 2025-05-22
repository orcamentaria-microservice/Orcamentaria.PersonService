namespace Orcamentaria.PersonService.Domain.DTOs.Address
{
    public class AddressUpdateDTO
    {
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string Number { get; set; }
        public string Complement { get; set; }
        public string Neihborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Uf { get; set; }
        public bool Default { get; set; }
    }
}
