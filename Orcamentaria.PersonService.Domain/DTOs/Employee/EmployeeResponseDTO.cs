using Orcamentaria.PersonService.Domain.DTOs.Address;
using Orcamentaria.PersonService.Domain.DTOs.Contact;
using Orcamentaria.PersonService.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orcamentaria.PersonService.Domain.DTOs.Employee
{
    public class EmployeeResponseDTO
    {
        public long Id { get; set; }
        public long CompanyId { get; set; }
        public string Name { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public bool Active { get; set; }
        public string Post{ get; set; }
        public DateTime AdmissionDate { get; set; }
        public decimal ValuePerDay { get; set; }
        public IEnumerable<AddressResponseDTO> Addresses { get; set; }
        public IEnumerable<ContactResponseDTO> Contacts { get; set; }
    }
}
