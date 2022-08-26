using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DutchTreat.Controllers
{
    public class AppController : Controller
    {
        private readonly IMailService _mailService;
        private readonly IDutchRepository _repository;
        public AppController(IMailService mailService, IDutchRepository repository)
        {
            _mailService = mailService;
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("about")]
        public IActionResult About()
        {
            ViewBag.Title = "About us";
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact us";
            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            if (ModelState.IsValid)
            {
                _mailService.SendMessage("email", model.Subject, model.Message);
                ViewBag.UserMessage = "Mail sent";
                ModelState.Clear();
            }
            else
                throw new System.Exception("Soucis de validation");
            return View();
        }

        public IActionResult Shop()
        {
            var results = _repository.GetAllProducts();
            return View(results);
        }
    }
}
