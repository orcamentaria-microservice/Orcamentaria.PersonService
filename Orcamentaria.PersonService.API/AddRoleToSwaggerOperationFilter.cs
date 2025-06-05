using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Orcamentaria.PersonService.API
{
    public class AddRoleToSwaggerOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var authorizeAttributes = context.MethodInfo
                .GetCustomAttributes(true)
                .OfType<AuthorizeAttribute>();

            var roles = authorizeAttributes
                .Where(attr => !string.IsNullOrEmpty(attr.Roles))
                .Select(attr => attr.Roles)
                .Distinct()
                .ToList();

            if (roles.Any())
            {
                operation.Extensions.Add("x-roles", new OpenApiArray
            {
                new OpenApiString(string.Join(", ", roles))
            });
            }
        }
    }
}
