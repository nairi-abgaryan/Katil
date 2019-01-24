using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Katil.WebAPI.WebApiHelpers
{
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;

        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            httpContext.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            httpContext.Response.Headers.Add("Access-Control-Allow-Headers", "Token, Content-Type, Authorization");
            httpContext.Response.Headers.Add("Access-Control-Allow-Methods", "POST, GET, PUT, PATCH, DELETE, OPTIONS");
            httpContext.Response.Headers.Add("Allow", "OPTIONS, TRACE, GET, HEAD, POST");
            return _next(httpContext);
        }
    }
}
