using Microsoft.Extensions.DependencyInjection.Extensions;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.Lib.Infrastructure;
using Orcamentaria.PersonService.Application.Services;
using Orcamentaria.PersonService.Application.Validators;
using Orcamentaria.PersonService.Domain.Mappers;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Services;
using Orcamentaria.PersonService.Infrastructure.Contexts;
using Orcamentaria.PersonService.Infrastructure.Repositories;

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

        public IConfiguration Configuration { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            Configuration = CommonDI.ResolveConfigs(_serviceName, services, Configuration);

            services.Replace(ServiceDescriptor.Singleton(Configuration));

            CommonDI.AddServiceRegistryHosted(services, Configuration);

            CommonDI.ResolveCommonServicesWithMySql<MySqlContext>(_serviceName, _apiVersion, services, Configuration, () =>
            {
                services.AddAutoMapper(
                    typeof(PersonMapper), 
                    typeof(ContactMapper), 
                    typeof(AddressMapper), 
                    typeof(EmployeeMapper));

                services.AddScoped<IValidatorEntity<Person>, PersonValidator>();
                services.AddScoped<IValidatorEntity<Contact>, ContactValidator>();
                services.AddScoped<IValidatorEntity<Address>, AddressValidator>();
                services.AddScoped<IValidatorEntity<Employee>, EmployeeValidator>();

                services.AddScoped<IPersonRepository, PersonRepository>();
                services.AddScoped<IContactRepository, ContactRepository>();
                services.AddScoped<IAddressRepository, AddressRepository>();
                services.AddScoped<IEmployeeRepository, EmployeeRepository>();

                services.AddScoped<IPersonService, Application.Services.PersonService>();
                services.AddScoped<IContactService, ContactService>();
                services.AddScoped<IAddressService, AddressService>();
                services.AddScoped<IEmployeeService, EmployeeService>();
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            => CommonDI.ConfigureCommon(_serviceName, _apiVersion, app, env);
    }
}
