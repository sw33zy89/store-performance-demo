using Microsoft.AspNetCore.Mvc;

namespace PerformanceTest.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Test");
        }
    }
}
