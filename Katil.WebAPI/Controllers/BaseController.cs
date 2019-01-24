using System;
using Katil.Business.Services;
using Katil.Common.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Katil.WebAPI.Controllers
{
    public class BaseController : Controller
    {
        protected bool CheckModified(IServiceBase service, object id)
        {
            if (this.Request.Headers.ContainsKey("If-Unmodified-Since"))
            {
                var ifUnmodifiedSince = DateTime.Parse(this.Request.Headers["If-Unmodified-Since"]);
                var lastModified = service.GetLastModifiedDateAsync(id).Result;

                if (lastModified != null)
                {
                    if (lastModified > ifUnmodifiedSince)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        protected int GetLoggedInUserId()
        {
            var username = User?.Identity?.Name;
            return int.Parse(username ?? "0");
        }

        protected ObjectResult StatusConflicted(string apiReturnMessage = ApiReturnMessages.ConflictOccured)
        {
            return this.StatusCode(StatusCodes.Status409Conflict, apiReturnMessage);
        }
    }
}
