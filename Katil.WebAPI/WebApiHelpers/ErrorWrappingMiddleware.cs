using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace Katil.WebAPI.WebApiHelpers
{
    public class ErrorWrappingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorWrappingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                Log.Information(ex, ex.Message);

                #if !DEBUG
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                #endif
                await context.Response.WriteAsync(ex.Message);
            }

            if (!context.Response.HasStarted)
            {
                context.Response.ContentType = "application/json";
                var response = new ApiResponse(context.Response.StatusCode);
                var json = JsonConvert.SerializeObject(
                    response,
                    new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() });

                if (context.Response.StatusCode != StatusCodes.Status204NoContent)
                {
                    await context.Response.WriteAsync(json);
                }
            }
        }
    }
}
