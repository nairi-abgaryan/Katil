using Microsoft.AspNetCore.Mvc;

namespace Katil.Services.PdfConvertor.PdfService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class HealthController : Controller
    {
        [HttpGet]
        public IActionResult Get() => Ok("ok");
    }
}
