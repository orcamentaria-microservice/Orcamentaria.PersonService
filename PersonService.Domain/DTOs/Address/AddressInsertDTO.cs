using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonService.Domain.DTOs.Address
{
    public class AddressInsertDTO
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
        public long PersonId { get; set; }
    }
}
