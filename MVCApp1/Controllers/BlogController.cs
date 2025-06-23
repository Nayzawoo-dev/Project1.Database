using Microsoft.AspNetCore.Mvc;

namespace MVCApp1.Controllers
{
    public class BlogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
