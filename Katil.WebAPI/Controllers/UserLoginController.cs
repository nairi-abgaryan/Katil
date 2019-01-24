using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Katil.Business.Entities.Models.User;
using Katil.Business.Services.UserServices;
using Katil.Common.Utilities;
using Katil.Data.Model;
using Katil.WebAPI.Filters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Katil.WebAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/userlogin")]
    public class UserLoginController : BaseController
    {
        private readonly IUserService _userService;

        public UserLoginController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        [AuthorizationRequired(roles: new[] { "Staff User" })]
        public async Task<IActionResult> Create([FromBody]UserLoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (request.Scheduler && request.SystemUserRoleId != (byte)Roles.StaffUser)
            {
                return BadRequest(ApiReturnMessages.SchedulerOnlyForStaffUsers);
            }

            var newUser = await _userService.CreateUser(request);
            return Ok(newUser);
        }

        [HttpPatch("update/{userId:int}")]
        [AuthorizationRequired(roles: new[] { "Staff User", "External User" })]
        public async Task<IActionResult> Update(int userId, [FromBody]JsonPatchDocument<UserLoginPatchRequest> request)
        {
            var loggedInUser = await _userService.GetSystemUser(GetLoggedInUserId());

            if (userId != GetLoggedInUserId())
            {
                if (loggedInUser.SystemUserRole.RoleName != "ROLE_USER" ||
                    loggedInUser.SystemUserRole.SystemUserRoleId != (byte)Roles.StaffUser)
                {
                    return Unauthorized();
                }
            }

            var originalUser = await _userService.GetSystemUser(userId);
            if (originalUser != null)
            {
                var userToPatch = Mapper.Map<User, UserLoginPatchRequest>(originalUser);
                request.ApplyTo(userToPatch, ModelState);

                var scheduler = request.Operations.FirstOrDefault(o => o.path == "/scheduler");
                if (scheduler != null)
                {
                    if (originalUser.SystemUserRoleId != (byte)Roles.StaffUser)
                    {
                        return BadRequest(ApiReturnMessages.SchedulerOnlyForStaffUsers);
                    }
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                Mapper.Map(userToPatch, originalUser);
                var result = await _userService.PatchAsync(originalUser);

                if (result != null)
                {
                    return Ok(Mapper.Map<User, UserLoginResponse>(result));
                }
            }

            return NotFound();
        }

        [HttpPatch("reset")]
        [AuthorizationRequired(roles: new[] { "Staff User", "External User" })]
        public async Task<IActionResult> Reset(int userId, [FromBody]JsonPatchDocument<UserLoginResetRequest> request)
        {
            var originalUser = await _userService.GetSystemUser(userId);
            if (originalUser != null)
            {
                var newVal = request.Operations.FirstOrDefault(o => o.path == "/password")?.ToString();
                if (newVal != null)
                {
                    var result = await _userService.Reset(originalUser, newVal);

                    if (result)
                    {
                        return Ok();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }

            return NotFound();
        }
    }
}