using Microsoft.AspNetCore.Mvc;

namespace thomasgregcorewebapi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<string>> Hello()
        {
            return Ok("Hello! I'm alive!!!");
        }
    }
}