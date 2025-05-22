using PersonService.Domain.Enums;

namespace PersonService.Domain.DTOs.Person
{
    public class PersonUpdateDTO
    {
        public string Name { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public string Cnpj { get; set; }
        public PersonTypeEnum Type { get; set; }
        public bool Active { get; set; }
    }
}
