using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DMTools.Web.Models;

namespace DMTools.Web.Controllers
{
    public class HomeController : Controller
    {
        private ILogger<HomeController> Logger { get; }

        public HomeController(ILogger<HomeController> logger)
        {
            this.Logger = logger;
        }

        public IActionResult Index()
        {
            this.Logger.LogDebug($"{nameof(this.Index)} THIS IS TEST DEBUG MESSAGE");
            return View();
        }

        public IActionResult Privacy()
        {
            this.Logger.LogWarning($"{nameof(this.Privacy)} THIS IS TEST WARN MESSAGE");
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}