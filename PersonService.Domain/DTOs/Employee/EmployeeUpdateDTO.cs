namespace PersonService.Domain.DTOs.Employee
{
    public class EmployeeUpdateDTO
    {
        public string Name { get; set; }
        public string Rg { get; set; }
        public string Cpf { get; set; }
        public bool Active { get; set; }
        public string Post{ get; set; }
        public DateTime AdmissionDate { get; set; }
        public decimal ValuePerDay { get; set; }
    }
}
