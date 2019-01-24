using Microsoft.AspNetCore.Mvc;

namespace Katil.Services.EmailGenerator.EmailGeneratorService
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Get() => Ok("ok");
    }
}
