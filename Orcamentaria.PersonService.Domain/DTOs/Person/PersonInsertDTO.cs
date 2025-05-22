using Orcamentaria.PersonService.Domain.Enums;
using Orcamentaria.PersonService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Orcamentaria.PersonService.Domain.DTOs.Person
{
    public class PersonInsertDTO
    {
        public string Name { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public PersonTypeEnum Type { get; set; }
        public bool Active { get; set; }
    }
}
