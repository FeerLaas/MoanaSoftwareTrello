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
        private string? token;
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
            token = HttpContext.Session.GetString("jwt");
            if (token is null) return RedirectToAction("Index", "Login");
            

            try
            {
                cards = await _apiService.GetAllCard(token);
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
        [HttpGet]
        public IActionResult CreateCard()
        {
            AddCardRequest card = new AddCardRequest();
            return PartialView("CreateCard", card);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCard(AddCardRequest card)
        {
            token = HttpContext.Session.GetString("jwt");
            if (token is null) return RedirectToAction("Index", "Login");
            try
            {
               await _apiService.CreateCard(card,token);
                ViewBag.info = "Success: Create Card";
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            return PartialView("CreateCard", card);
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