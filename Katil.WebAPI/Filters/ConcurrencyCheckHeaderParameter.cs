using System.Linq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Katil.WebAPI.Filters
{
    public class ConcurrencyCheckHeaderParameter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isApplied = filterPipeline.Select(f => f.Filter).Any(f => f is ApplyConcurrencyCheckAttribute);

            if (isApplied)
            {
                operation.Parameters.Add(new NonBodyParameter()
                {
                    Name = "If-Unmodified-Since",
                    In = "header",
                    Description = "Concurrency",
                    Required = false,
                    Type = "DateTime"
                });
            }
        }
    }
}
