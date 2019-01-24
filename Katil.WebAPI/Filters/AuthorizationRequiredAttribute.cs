using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Katil.Business.Services.TokenServices;
using Katil.Business.Services.UserServices;
using Katil.Data.Model;
using Katil.WebAPI.WebApiHelpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Katil.WebAPI.Filters
{
    public class AuthorizationRequiredAttribute : ActionFilterAttribute
    {
        private bool _extendSession;

        public AuthorizationRequiredAttribute(string[] roles = null, bool extendSession = true)
        {
            Roles = roles;
            _extendSession = extendSession;
        }

        private string[] Roles { get; set; }

        public override async System.Threading.Tasks.Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var userToken = await GetUserToken(context);
            if (userToken == null)
            {
                context.Result = new UnauthorizedResult();
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else
            {
                var user = userToken.Id.HasValue ? await GetUser(context, userToken.Id.Value) : null;
                if (user != null)
                {
                    SetClaims(context, user);
                }
            }

            await base.OnActionExecutionAsync(context, next);
        }

        private async Task<UserToken> GetUserToken(ActionExecutingContext context)
        {
            ////TODO: Examine this part later with team: Line 688 throws null reference exception
            var tokenService = context.GetService<ITokenService>();

            var tokenValue = context.HttpContext.Request.GetToken();
            if (tokenService != null && string.IsNullOrWhiteSpace(tokenValue) == false)
            {
                var userToken = await tokenService.GetUserToken(tokenValue);
                var isValid = await tokenService.ValidateToken(userToken.AuthToken, true);
                return isValid ? userToken : null;
            }

            return null;
        }

        private async Task<User> GetUser(ActionExecutingContext context, int userId)
        {
            var userService = context.GetService<IUserService>();
            var user = await userService.GetUserWithFullInfo(userId);

            return user;
        }

        private void SetClaims(ActionExecutingContext context, User user)
        {
            List<System.Security.Claims.Claim> claims = new List<System.Security.Claims.Claim>
                {
                    new System.Security.Claims.Claim(ClaimTypes.Name, user.Id.ToString()),
                    new System.Security.Claims.Claim(ClaimTypes.Role, user.SystemUserRole.SystemUserRoleId.ToString())
                };

            ClaimsIdentity identity = new ClaimsIdentity(claims);

            ClaimsPrincipal principal = new ClaimsPrincipal(identity);

            context.HttpContext.User = principal;
        }
    }
}
