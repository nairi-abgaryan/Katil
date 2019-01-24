using Microsoft.AspNetCore.Builder;

namespace Katil.WebAPI.WebApiHelpers
{
    public static class ErrorWrappingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorWrappingMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ErrorWrappingMiddleware>();
        }
    }
}
