using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LorryLogAdmin.Models;
using Microsoft.Extensions.Configuration;

namespace LorryLogAdmin.Controllers
{
    public class HomeController : Controller
    {
        private IConfiguration _configuration;
        private readonly ILogger<HomeController> _logger;

        public HomeController(IConfiguration configuration)
        {
            //_logger = logger;
            _configuration = configuration;

        }

        public IActionResult Index()
        {
            ViewBag.SlotName = _configuration.GetValue<string>("slot_name");
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
