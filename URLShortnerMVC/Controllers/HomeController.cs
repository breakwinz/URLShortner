using Microsoft.AspNetCore.Mvc;
using System;
using System.Diagnostics;
using URLShortnerMVC.Models;
using DataLibrary.Interfaces;


namespace URLShortnerMVC.Controllers
{
    public class HomeController : Controller
    { 
        private readonly IShortUrlProcessor _shortUrlProcessor;

        public HomeController(IShortUrlProcessor shortUrlProcessor)
        {

            _shortUrlProcessor = shortUrlProcessor;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ShortenUrl(ShortUrlModel model)
        {
            if (ModelState.IsValid)
            {
                ViewBag.currentdomain = GetCurrentAbsoluteUrl();
                Tuple<bool, string> response = _shortUrlProcessor.CreateShortUrl(model.originalURL, model.shortURL);
                if (response.Item1)
                {
                    TempData["success"] = true;
                    TempData["shortlink"] = ViewBag.currentdomain + response.Item2;
                }
                else
                {
                    TempData["success"] = false;
                }
            }
            return RedirectToAction("ShortenUrl");
        }

        [HttpGet]
        [Route("{*id}")]
        public IActionResult ShortenUrl(string id)
        {
            ViewBag.currentdomain = GetCurrentAbsoluteUrl();

            if (TempData["success"] != null) // Checks if it has come here from a redirection from post
            {
                ViewBag.ShortenURLSuccess = TempData["success"];
                ViewBag.shortlink = TempData["shortlink"];
                return View();
            }

            if (id == null)
            {
                return View();
            }

            var originalUrl = _shortUrlProcessor.GetOriginalUrl(id);
            if (originalUrl == null)
            {
                return Redirect("~/");
            }
            return Redirect(originalUrl);
        }

        public string GetCurrentAbsoluteUrl()
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
