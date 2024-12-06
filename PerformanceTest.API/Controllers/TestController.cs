using Microsoft.AspNetCore.Mvc;

namespace PerformanceTest.API.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
