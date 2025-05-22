namespace PersonService.Domain.Models
{
    public class Employee : Person
    {
        public string Post { get; set; }
        public DateTime AdmissionDate { get; set; }
        public decimal ValuePerDay { get; set; }
    }
}
