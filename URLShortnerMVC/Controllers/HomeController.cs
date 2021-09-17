using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using URLShortnerMVC.Models;
using Microsoft.AspNetCore.Http;
using DataLibrary.Interfaces;


namespace URLShortnerMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IShortUrlProcessor _shortUrlProcessor;

        public HomeController(ILogger<HomeController> logger, IShortUrlProcessor shortUrlProcessor)
        {
            _logger = logger;
            _shortUrlProcessor = shortUrlProcessor;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShortenURL(ShortURLModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.currentdomain = GetCurrentAbsoluteURL();
                Tuple<bool, string> response = _shortUrlProcessor.CreateShortURL(model.originalURL, model.shortURL);
                if (response.Item1)
                {
                    ViewBag.ShortenURLSuccess = true;           
                    ViewBag.shortlink = ViewBag.currentdomain + response.Item2;
                }
                else
                {
                    ViewBag.ShortenURLSuccess = false;
                }
            }
            return View();
        }

        [HttpGet]
        [Route("{*id}")]
        public IActionResult ShortenURL(string id)
        {
            ViewBag.currentdomain = GetCurrentAbsoluteURL();
            if (id == null)
            {
                return View();
            }

            var originalURL = _shortUrlProcessor.GetOriginalURL(id);
            if (originalURL == null)
            {
                return Redirect("~/");
            }
            return Redirect(originalURL);
        }

        public string GetCurrentAbsoluteURL()
        {
            var absoluteUri = string.Concat(
                       HttpContext.Request.Scheme,
                       "://",
                       HttpContext.Request.Host.ToUriComponent(),
                       HttpContext.Request.PathBase.ToUriComponent(),
                       HttpContext.Request.Path.ToUriComponent(),
                       HttpContext.Request.QueryString.ToUriComponent());
            return absoluteUri;
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
