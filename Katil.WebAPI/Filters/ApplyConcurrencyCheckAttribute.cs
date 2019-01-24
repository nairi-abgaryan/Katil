using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Katil.WebAPI.Filters
{
    public class ApplyConcurrencyCheckAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await base.OnActionExecutionAsync(context, next);
        }
    }
}
