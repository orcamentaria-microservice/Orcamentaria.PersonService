
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PersonService.Application.Validators;
using PersonService.Domain.Mappers;
using PersonService.Domain.Models;
using PersonService.Domain.Repositories;
using PersonService.Domain.Services;
using PersonService.Domain.Validators;
using PersonService.Infrastructure.Contexts;
using PersonService.Infrastructure.Repositories;
using PersonService.Application.Services;
using PersonService.Domain.Contexts;
using PersonService.API.Middlewares;

namespace PersonService.API
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

            services.AddScoped<IPersonService, Application.Services.PersonService>();
            services.AddScoped<IContactService, ContactService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IEmployeeService, EmployeeService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Person MS v1"));
                app.UseDeveloperExceptionPage();
            }

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
