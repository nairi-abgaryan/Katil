using System.Threading.Tasks;
using Katil.Business.Entities.Models.User;
using Katil.Business.Services.UserServices;
using Katil.Common.Utilities;
using Katil.Data.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Katil.WebAPI.Controllers
{
    public class AuthController : BaseController
    {
        private readonly IAuthenticateService _authenticateService;

        public AuthController(IAuthenticateService authenticateService)
        {
            _authenticateService = authenticateService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateRequest authParams)
        {
            var user = await _authenticateService.Login(authParams.Email, authParams.Password);

            return Ok(user);
        }
    }
}