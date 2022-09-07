using Microsoft.AspNetCore.Mvc;
using MoanaSoftwareTrello.Models;
using MoanaSoftwareTrello.Services;

namespace MoanaSoftwareTrello.Controllers
{
    public class LoginController : Controller
    {
        private ApiService _apiSerice;
        public LoginController(ApiService apiService)
        {
            _apiSerice = apiService;
        }

        [HttpGet]
        public IActionResult Index(bool logged = false)
        {
            if (logged) RedirectToAction("Index","Home");
            return View();
        }
        [HttpPost]
        public IActionResult Index(User userModel)
        {
            // login ...
            // login is valid add token to local
            bool success = true;
            if (success) return RedirectToAction("Index", "Home");
            return View();
        }
        [HttpGet]
        public IActionResult Register(bool logged = false)
        {
            if (logged) RedirectToAction("Index", "Home");
            User userModel = new User();
            return View(userModel);
        }
    }
}
