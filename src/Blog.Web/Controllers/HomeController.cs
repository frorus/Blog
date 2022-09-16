using Microsoft.AspNetCore.Mvc;

namespace Blog.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Terms()
        {
            return View();
        }
    }
}