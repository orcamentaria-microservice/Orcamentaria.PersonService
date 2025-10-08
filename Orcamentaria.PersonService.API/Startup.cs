using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Orcamentaria.Lib.Application.Services;
using Orcamentaria.Lib.Domain.Models.Configurations;
using Orcamentaria.Lib.Domain.Models.Logs;
using Orcamentaria.Lib.Domain.Services;
using Orcamentaria.Lib.Domain.Validators;
using Orcamentaria.Lib.Infrastructure;
using Orcamentaria.Lib.Infrastructure.Middlewares;
using Orcamentaria.PersonService.Application.Services;
using Orcamentaria.PersonService.Application.Validators;
using Orcamentaria.PersonService.Domain.Mappers;
using Orcamentaria.PersonService.Domain.Models;
using Orcamentaria.PersonService.Domain.Repositories;
using Orcamentaria.PersonService.Domain.Services;
using Orcamentaria.PersonService.Infrastructure.Contexts;
using Orcamentaria.PersonService.Infrastructure.Repositories;
using System.Text.Json.Serialization.Metadata;

namespace Orcamentaria.PersonService.API
{
    public class Startup
    {
        private readonly string _serviceName = "Orcamentaria.PersonService";
        private readonly string _apiVersion = "v1";
        readonly static string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            CommonDI.AddServiceRegistryHosted(services, Configuration);
            
            CommonDI.ResolveCommonServices(_serviceName, _apiVersion, services, Configuration, () =>
            {

                services.AddDbContext<MySqlContext>(options =>
                    options.UseMySQL(Configuration.GetConnectionString("DefaultConnection")));

                //services.AddSwaggerGen(c =>
                //{
                //    c.OperationFilter<AddRoleToSwaggerOperationFilter>();
                //});

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

                //services.AddSingleton<ILogService, LogServiceTeste>();
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.Use(async (context, next) =>
            //{
            //    var path = context.Request.Path.Value;
            //    var isSwaggerJson = path != null && path.Contains("/swagger/v1/swagger.json");

            //    if (isSwaggerJson)
            //    {
            //        var serviceConfiguration = context.RequestServices.GetRequiredService<IOptions<ServiceConfiguration>>().Value;
            //        var clientId = context.Request.Headers["ClientId"].ToString();
            //        var clientSecret = context.Request.Headers["ClientSecret"].ToString();

            //        if (!clientId.Equals(serviceConfiguration.ClientId, StringComparison.OrdinalIgnoreCase) ||
            //        !clientSecret.Equals(serviceConfiguration.ClientSecret, StringComparison.OrdinalIgnoreCase))
            //        {
            //            context.Response.StatusCode = 401;
            //            await context.Response.WriteAsync("Unauthorized to access Swagger JSON");
            //            return;
            //        }
            //    }

            //    await next();
            //});

            //app.UseSwagger();

            //if (env.IsDevelopment())
            //{
            //    app.UseSwaggerUI(c => c.SwaggerEndpoint($"/swagger/{_apiVersion}/swagger.json", $"{_serviceName} {_apiVersion}"));
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseRouting();

            //app.UseCors(MyAllowSpecificOrigins);


            //app.UseAuthentication();
            //app.UseMiddleware<ErrorHandlingMiddlewareTeste>();
            //app.UseMiddleware<UserAuthMiddleware>();
            //app.UseMiddleware<RequestMiddleware>();
            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
            CommonDI.ConfigureCommon(_serviceName, _apiVersion, app, env);
        }
    }
}
