using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.Title = "Contact us";
            return View();
        }

        public IActionResult About()
        {
            ViewBag.Title = "About us";
            return View();
        }
    }
}
