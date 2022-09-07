using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoanaSoftwareTrello.Models;
using MoanaSoftwareTrello.Services;
using System.Diagnostics;

namespace MoanaSoftwareTrello.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApiService _apiService;

        public HomeController(ILogger<HomeController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
        }
        //need adding authorizated
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("jwt") is null) return RedirectToAction("Index", "Login");
            
                var value = Request.Cookies["jwt"];
            try
            {
                var x = await _apiService.GetAllCard(value);
                ;
            }
            catch (Exception e)
            {

                _logger.LogError(e.Message);
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}