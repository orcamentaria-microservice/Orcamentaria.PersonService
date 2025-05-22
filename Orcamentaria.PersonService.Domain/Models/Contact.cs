using Orcamentaria.PersonService.Domain.Enums;

namespace Orcamentaria.PersonService.Domain.Models
{
    public class Contact
    {
        public long Id { get; set; }
        public string ContactDescription { get; set; }
        public ContactTypeEnum Type { get; set; }
        public bool Default { get; set; }
        public long PersonId { get; set; }
    }
}
