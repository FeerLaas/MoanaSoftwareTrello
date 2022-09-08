﻿using Microsoft.AspNetCore.Authorization;
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
        public enum StatusEnum {Pending=0, In_progress,Blocked,Done};
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
                model.cards = cards.OrderBy(x=>x.Position).ToList();
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
        [HttpGet]
        public async Task<IActionResult> Edit(Guid? id)
        {
            token = HttpContext.Session.GetString("jwt");
            if (token is null) return RedirectToAction("Index", "Login");

            if (id == null) return NotFound();
            
            try
            {
                var card = await _apiService.GetCardById(id.ToString(), token);
                return PartialView("EditCard", card );

            }
            catch (Exception e)
            {
                return NotFound();
            }
        }
        [HttpPost]
        public async Task<IActionResult> EditCard(GetCardResponse card, bool pos = false)
        {
            token = HttpContext.Session.GetString("jwt");
            if (token is null) return RedirectToAction("Index", "Login");


            if (card == null)  return NotFound();
            
            UpdateCardRequest updateCard = new UpdateCardRequest();
            GetCardResponse originalCard;
            try
            {
                originalCard = await _apiService.GetCardById(card.Id.ToString(), token);

            }
            catch (Exception e)
            {
                return NotFound();
            }

            updateCard.Id = originalCard.Id;
            if (pos)
            {
                updateCard.Title = originalCard.Title;
                updateCard.Description = originalCard.Description;
                updateCard.AsigneeId = originalCard.AsigneeId;
                updateCard.Position = card.Position;
                updateCard.Status = card.Status;
            }
            else
            {
                updateCard.Title = card.Title;
                updateCard.Description = card.Description;
                updateCard.AsigneeId = originalCard.AsigneeId;
                updateCard.Position = originalCard.Position;
                updateCard.Status = originalCard.Status;
            }

            try
            {
                await _apiService.UpdateCard(updateCard, token);
                return PartialView("EditCard", card);

            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return PartialView("EditCard", card);
        }
        [HttpGet]
        public IActionResult ajaxReloadColumn(string column)
        {
            token = HttpContext.Session.GetString("jwt");
            if (token is null) return RedirectToAction("Index", "Login");
            Thread.Sleep(200); // bad idea
            var cards = _apiService.GetAllCard(token).Result;
            var columnNumber = (Status)System.Enum.Parse(typeof(StatusEnum), column);
            //var color = (Status)System.Enum.Parse(typeof(StatusEnum), column);
            ViewBag.data = cards.Where(x => x.Status == columnNumber).OrderBy(x => x.Position).ToList();
            return PartialView("ajaxReloadColumn");
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