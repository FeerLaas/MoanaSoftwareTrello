using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoanaSoftwareTrello.Models;
using MoanaSoftwareTrello.Services;
using System.Diagnostics;
using System.Dynamic;

namespace MoanaSoftwareTrello.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private ApiService _apiService;
        public string[] Status;
        public HomeController(ILogger<HomeController> logger, ApiService apiService)
        {
            _logger = logger;
            _apiService = apiService;
            Status = new string[] { "Pending", "In_progress", "Blocked", "Done" };

        }
        //need adding authorizated
        public async Task<IActionResult> Index()
        {
            List<GetAllCardResponse> cards;
            if (HttpContext.Session.GetString("jwt") is null) return RedirectToAction("Index", "Login");
            var value = HttpContext.Session.GetString("jwt");

            try
            {
                cards = await _apiService.GetAllCard(value);
                dynamic model = new ExpandoObject();
                model.cards = cards;
                model.status = Status;
                return View(model);

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