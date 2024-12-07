using Microsoft.AspNetCore.Mvc;

namespace PerformanceTest.API.Controllers
{
    [ApiController]
    [Route("api/test")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok("Test");
        }
    }
}
