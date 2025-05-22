using PersonService.Domain.Contexts;

namespace PersonService.Infrastructure.Contexts
{
    public class CompanyContext : ICompanyContext
    {
        public long CompanyId { get; set; }
    }
}
