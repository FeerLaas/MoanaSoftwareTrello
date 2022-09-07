using Microsoft.AspNetCore.Mvc;
using MoanaSoftwareTrello.Models;

namespace MoanaSoftwareTrello.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index(bool logged = false)
        {
            User userModel = new User();
            return View();
        }
    }
}
