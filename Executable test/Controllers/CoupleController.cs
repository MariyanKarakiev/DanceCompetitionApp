using BussinessLayer.Models;
using BussinessLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Executable_test.Controllers
{
    public class CoupleController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CoupleService _coupleService;
        private readonly IMemoryCache _memeoryCache;

        public CoupleController(ILogger<HomeController> logger, IMemoryCache memoryCache, CoupleService coupleService)
        {
            _logger = logger;
            _coupleService = coupleService;
            _memeoryCache = memoryCache;
        }
        // GET: CompetitionController
        public ActionResult Index(string name)
        {
            if (name != null )
            {
                _memeoryCache.Set<string>("CompetetiveClass", name, TimeSpan.FromMinutes(450));
            }

            var competetiveClass = (string)_memeoryCache.Get("CompetetiveClass");


            var all = _coupleService.GetAll(competetiveClass);

            var couplesModel = all.Select(c => new CoupleViewModel()
            {
                Name = c.Name,
                CompetetiveClass = c.CompetetiveClass
            }).AsEnumerable();

            ViewData["CompetetiveClass"] = competetiveClass;
            return View(couplesModel);
        }

        // GET: CompetitionController/Details/5
        public ActionResult Details(string competition)
        {

            return View();
        }

        // GET: CompetitionController/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CoupleViewModel model)
        {
            var competetiveClass = (string)_memeoryCache.Get("CompetetiveClass");

            _coupleService.Create(new Couple() { Name = model.Name, CompetetiveClass = competetiveClass },3);

            return RedirectToAction(nameof(Index));
        }

        // GET: CompetetiveClassController/Delete/5
        public ActionResult Delete(string name)
        {
            var competetiveClass = (string)_memeoryCache.Get("CompetetiveClass");

            var couple = _coupleService.Get(name, competetiveClass);

            var mapped = new CoupleViewModel()
            {
                Name = couple.Name,
                CompetetiveClass = couple.CompetetiveClass
            };
            return View(mapped);
        }

        // POST: CompetetiveClassController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Couple couple)
        {
                var competetiveClass = (string)_memeoryCache.Get("CompetetiveClass");

                _coupleService.Delete(couple.Name, competetiveClass);
                return RedirectToAction(nameof(Index)); 
        }
    }
}
