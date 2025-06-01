
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Orcamentaria.PersonService.Application.Validators;
using Orcamentaria.PersonService.Domain.Mappers;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Services;
using Orcamentaria.Lib.Infrastructure.Contexts;
using Orcamentaria.PersonService.Infrastructure.Repositories;
using Orcamentaria.PersonService.Application.Services;
using Orcamentaria.Lib.Infrastructure.Middlewares;
using Orcamentaria.Lib.Domain.Contexts;
using Orcamentaria.PersonService.Infrastructure.Contexts;
using Orcamentaria.Lib.Domain.Validators;

namespace Orcamentaria.PersonService.API
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ReportApiVersions = true;
                opt.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            services.AddControllers()
                .AddJsonOptions(opts => opts.JsonSerializerOptions.PropertyNamingPolicy = null);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Person MS", Version = "v1" });
            });

            services.AddControllers()
                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressConsumesConstraintForFormFileParameters = false;
                    options.SuppressInferBindingSourcesForParameters = true;
                    options.SuppressModelStateInvalidFilter = false;
                    options.SuppressMapClientErrors = false;
                    options.ClientErrorMapping[StatusCodes.Status404NotFound].Link =
                        "https://httpstatuses.com/404";
                });

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });

            services.AddDbContext<MySqlContext>(options =>
                options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<ICompanyContext, CompanyContext>();

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

            services.AddScoped<IPersonService, PersonService.Application.Services.PersonService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Person MS v1"));
                app.UseDeveloperExceptionPage();
            //}

            app.UseMiddleware<CompanyMiddleware>();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
