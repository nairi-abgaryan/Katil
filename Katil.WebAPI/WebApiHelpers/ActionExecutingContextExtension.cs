using System.Linq;
using Katil.Common.Utilities;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace Katil.WebAPI.WebApiHelpers
{
    public static class ActionExecutingContextExtension
    {
        public static T GetService<T>(this ActionExecutingContext context)
            where T : class
        {
            return context.HttpContext.RequestServices.GetService<T>() as T;
        }

        public static T GetContextId<T>(this ActionExecutingContext context, string name)
        {
            var descriptor = context.ActionDescriptor as ControllerActionDescriptor;
            var parameters = descriptor.MethodInfo.GetParameters();

            var value = context.ActionArguments[parameters.FirstOrDefault(p => p.Name == name).Name];
            return value.ChangeType<T>();
        }
    }
}
