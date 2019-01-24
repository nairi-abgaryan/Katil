using Microsoft.AspNetCore.Mvc;

namespace Katil.Services.EmailNotification.EmailNotificationService
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Get() => Ok("ok");
    }
}
