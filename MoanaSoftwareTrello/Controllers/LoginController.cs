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
        public async Task<IActionResult> Index(User userModel)
        {
            SignInResponse result;
            try
            {
               result = await _apiSerice.Login(userModel);
                if (result == null) throw new Exception("Error Auth");
                HttpContext.Session.SetString("jwt", result.Token);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View();
            }


        }
        [HttpPost]
        public async Task<IActionResult> Register(User userModel)
        {
            string result;
            try
            {
                result = await _apiSerice.RegisterUser(userModel);
                if (result == null) throw new Exception("Error Auth");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
                return View("Register", userModel);
            }
            return RedirectToAction("Index", "Login");
           
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return View("Index");
        }
    }
}
