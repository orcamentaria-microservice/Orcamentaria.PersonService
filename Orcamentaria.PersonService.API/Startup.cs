using Microsoft.EntityFrameworkCore;
using Orcamentaria.PersonService.Application.Validators;
using Orcamentaria.PersonService.Domain.Mappers;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Services;
using Orcamentaria.Lib.Infrastructure.Contexts;
using Orcamentaria.PersonService.Infrastructure.Repositories;
using Orcamentaria.PersonService.Application.Services;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.PersonService.Infrastructure.Contexts;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.Lib.Infrastructure;

namespace Orcamentaria.PersonService.API
{
    public class Startup
    {
        private readonly string _serviceName = "Orcamentaria.PersonService";
        private readonly string _apiVersion = "v1";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            CommonDI.ResolveCommonServices(_serviceName, _apiVersion, services, Configuration);

            services.AddDbContext<MySqlContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));

            services.AddAutoMapper(
                typeof(PersonMapper), 
                typeof(ContactMapper), 
                typeof(AddressMapper), 
                typeof(EmployeeMapper));

            services.AddScoped<IUserAuthContext, UserAuthContext>();

            services.AddScoped<IValidatorEntity<Person>, PersonValidator>();
            services.AddScoped<IValidatorEntity<Contact>, ContactValidator>();
            services.AddScoped<IValidatorEntity<Address>, AddressValidator>();
            services.AddScoped<IValidatorEntity<Employee>, EmployeeValidator>();

            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<IContactRepository, ContactRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            services.AddScoped<IPersonService, PersonService.Application.Services.PersonService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => CommonDI.ConfigureCommon(_serviceName, _apiVersion, app, env);
    }
}
