using System;
using System.Security.Principal;
using System.Threading.Tasks;
using Katil.Business.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Katil.WebAPI.Filters
{
    public class ApiAuthenticationFilter : Attribute, IAsyncAuthorizationFilter
    {
        public string BasicRealm { get; set; }

        protected string Username { get; set; }

        protected string Password { get; set; }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var identity = FetchAuthHeader(context);

            if (identity == null)
            {
                ChellengeAuthRequest(context);
            }

            var service = context.HttpContext.RequestServices.GetService(typeof(IAuthenticateService)) as IAuthenticateService;

            if (service != null)
            {
                var token = await service.Login(Username, Password);
                context.HttpContext.Response.Headers.Add("Token", "sas");
            }
        }

        public void ChellengeAuthRequest(AuthorizationFilterContext context)
        {
            var dnsHost = context.HttpContext.Request.Host;
            context.HttpContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", dnsHost));
            context.Result = new UnauthorizedResult();
        }

        private GenericIdentity FetchAuthHeader(AuthorizationFilterContext context)
        {
            var req = context.HttpContext.Request;
            var auth = req.Headers["Authorization"];
            if (!string.IsNullOrEmpty(auth))
            {
                var cred = System.Text.ASCIIEncoding.ASCII.GetString(Convert.FromBase64String(auth.ToString().Substring(6))).Split(':');
                var user = new { Name = cred[0], Pass = cred[1] };
                if (user.Name == Username && user.Pass == Password)
                {
                    return new GenericIdentity(Username, Password);
                }
                else
                {
                    Username = user.Name;
                    Password = user.Pass;
                }
            }

            var dnsHost = context.HttpContext.Request.Host;
            context.HttpContext.Response.Headers.Add("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", dnsHost));

            if (string.IsNullOrEmpty(auth))
            {
                context.Result = new UnauthorizedResult();
            }

            if (string.IsNullOrWhiteSpace(Username) && string.IsNullOrWhiteSpace(Password))
            {
                Username = "guest";
                Password = "guest";
            }

            return new GenericIdentity(Username, Password);
        }
    }
}