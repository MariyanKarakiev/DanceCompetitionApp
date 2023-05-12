using BussinessLayer;
using BussinessLayer.Models;
using BussinessLayer.Services;
using Executable_test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Diagnostics;

namespace Executable_test.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CoupleService _coupleService;
        private readonly CompetetiveClassService _competetiveClassService;
        public HomeController(ILogger<HomeController> logger, CoupleService coupleService, CompetetiveClassService competetiveClassService)
        {
            _logger = logger;  
            _coupleService = coupleService;
            _competetiveClassService = competetiveClassService;
        }

        public IActionResult Index()
        {
            return Redirect("Competition");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult SetUp()
        {
            _competetiveClassService.Create(new CompetetiveClass()
            {
                Name = "Set up class",
                JudgesCount = 10
            });

            _coupleService.Create(new Couple()
            {
                Name = "Set up couple",
                CompetetiveClass = "Set up"
            },10);

            return Ok();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}