using System.Threading.Tasks;
using AutoMapper;
using Katil.Business.Entities.Models.User;
using Katil.Business.Services.TokenServices;
using Katil.Business.Services.UserServices;
using Katil.Data.Model;
using Katil.WebAPI.Filters;
using Katil.WebAPI.WebApiHelpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Katil.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public UsersController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetCurrentUserInfo()
        {
            var userId = GetLoggedInUserId();
            var user = await _userService.GetUser(userId);
            return Ok(user);
        }

        [HttpPatch("internaluserstatus")]
        [AuthorizationRequired(roles: new[] { "Staff User" })]
        [ApplyConcurrencyCheck]
        public async Task<IActionResult> PatchInternalUser(int userId, [FromBody]JsonPatchDocument<PatchUserRequest> user)
        {
            if (CheckModified(_userService, userId))
            {
                return StatusConflicted();
            }

            var originalUser = await _userService.GetNoTrackingSystemUser(userId);
            if (originalUser != null)
            {
                var userToPatch = Mapper.Map<User, PatchUserRequest>(originalUser);
                user.ApplyTo(userToPatch, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Mapper.Map(userToPatch, originalUser);
                var result = await _userService.PatchAsync(originalUser);

                if (result != null)
                {
                    return Ok(Mapper.Map<User, UserResponse>(result));
                }
            }

            return NotFound();
        }

        [HttpPost("logout")]
        [AuthorizationRequired(roles: new[] { "Staff User", "External User", "Access Code User" })]
        public async Task<IActionResult> Logout()
        {
            var token = Request.GetToken();

            var result = await _tokenService.KillToken(token);
            if (result)
            {
                return NoContent();
            }

            return NotFound();
        }
    }
}