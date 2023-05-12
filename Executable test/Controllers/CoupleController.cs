using BussinessLayer.Models;
using BussinessLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace Executable_test.Controllers
{
    public class CoupleController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CoupleService _coupleService;
        private readonly CompetetiveClassService _competetiveClassService;
        private readonly IMemoryCache _memeoryCache;

        public CoupleController(ILogger<HomeController> logger, IMemoryCache memoryCache, CoupleService coupleService, CompetetiveClassService competetiveClassService)
        {
            _logger = logger;
            _coupleService = coupleService;
            _memeoryCache = memoryCache;
            _competetiveClassService = competetiveClassService;
        }

        public ActionResult Index(string name)
        {
            if (name != null)
            {
                _memeoryCache.Set<string>("CompetetiveClass", name, TimeSpan.FromMinutes(450));
            }

            var competetiveClass = (string)_memeoryCache.Get("CompetetiveClass");

            if (competetiveClass == null)
            {
                return RedirectToAction("Index", "CompetetiveClass");
            }
            //  _coupleService.Adjuicate();

            var all = _coupleService.GetAll(competetiveClass);

            var couplesModel = all.Select(c => new CoupleViewModel()
            {
                Name = c.Name,
                CompetetiveClass = c.CompetetiveClass,
                CreatedOn = DateTime.Parse(c.CreatedOn),
                Place = int.Parse(c.Place)
            }).AsEnumerable();

            ViewData["CompetetiveClass"] = competetiveClass;
            return View(couplesModel);
        }

        // GET: CompetitionController/Details/5
        public ActionResult Details(string name)
        {
            var competetiveClass = (string)_memeoryCache.Get("CompetetiveClass");
            if (competetiveClass == null)
            {
                return RedirectToAction("Index", "CompetetiveClass");
            }
            var couple = _coupleService.Get(name, competetiveClass);

            var mapped = new CoupleViewModel()
            {
                Name = couple.Name,
                CompetetiveClass = couple.CompetetiveClass,
                CreatedOn = DateTime.Parse(couple.CreatedOn),
                UpdatedOn = DateTime.Parse(couple.UpdatedOn),
                Sum = int.Parse(couple.Sum),
                Place = int.Parse(couple.Place)
            };

            var count = int.Parse(couple.JudgesCount);

            IDictionary<string, object?> coupleDict = couple;

            var t = new List<(string, object?)>();

            for (int i = 0; i < count; i++)
            {
                var c = ((char)(i + 65)).ToString();
                var val = coupleDict[$"Judge{c}"];
                t.Add(($"Judge{c}", val));
            }

            mapped.JudgePlaceList = t;
            return View(mapped);
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
            var competetiveClassName = (string)_memeoryCache.Get("CompetetiveClass");

            if (competetiveClassName == null)
            {
                return RedirectToAction("Index", "CompetetiveClass");
            }

            var competetiveClass = _competetiveClassService.Get(competetiveClassName);
            _coupleService.Create(new Couple() { Name = model.Name, CompetetiveClass = competetiveClassName }, competetiveClass.JudgesCount);

            return RedirectToAction(nameof(Index));
        }

        // GET: CompetetiveClassController/Delete/5
        public ActionResult Delete(string name)
        {
            var competetiveClass = (string)_memeoryCache.Get("CompetetiveClass");
            if (competetiveClass == null)
            {
                return RedirectToAction("Index", "CompetetiveClass");
            }
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
          
            if (competetiveClass == null)
            {
                return RedirectToAction("Index", "CompetetiveClass");
            }
            _coupleService.Delete(couple.Name, competetiveClass);
            return RedirectToAction(nameof(Index));
        }
    }
}
