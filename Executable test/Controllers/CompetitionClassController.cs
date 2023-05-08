using BussinessLayer.Models;
using BussinessLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Xml.Linq;

namespace Executable_test.Controllers
{
    public class CompetetiveClassController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CompetetiveClassService _competetiveClassService;
        private readonly IMemoryCache _memeoryCache;

        public CompetetiveClassController(ILogger<HomeController> logger, IMemoryCache memeoryCache, CompetetiveClassService competetiveClassService)
        {
            _logger = logger;
            _competetiveClassService = competetiveClassService;
            _memeoryCache = memeoryCache;
        }
        // GET: CompetetiveClassController
        public ActionResult Index(string name)
        {
            if (name != null)
            {
                _memeoryCache.Set<string>("competition", name, TimeSpan.FromMinutes(450));
            }

            var competitionName = (string)_memeoryCache.Get("competition");

            var all = _competetiveClassService.GetAll().Where(c => c.CompetitionName == competitionName).ToList();

            var classes = all.Select(a =>
            new CompetetiveClassViewModel()
            {
                Name = a.Name,
                CompetitionName = a.CompetitionName,
                CreatedOn = a.CreatedOn,
                UpdatedOn = a.UpdatedOn,
                DeletedOn = a.DeletedOn
            });


            ViewData["CompetitionName"] = competitionName;
            return View(classes);
        }

        // GET: CompetetiveClassController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompetetiveClassController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompetetiveClassController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CompetetiveClass competetiveClass)
        {
            try
            {
                _competetiveClassService.Create(competetiveClass.Name, _memeoryCache.Get("competition").ToString());
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompetetiveClassController/Edit/5
        public ActionResult Edit(string name)
        {
            var competetiveClass = _competetiveClassService.Get(name);

            return View(new CompetetiveClassViewModel()
            {
                Name = competetiveClass.Name,
                NewName = competetiveClass.Name,
                CompetitionName = competetiveClass.CompetitionName,
                CreatedOn = competetiveClass.CreatedOn,
                DeletedOn = competetiveClass.DeletedOn,
                UpdatedOn = competetiveClass.UpdatedOn
            });
        }

        // POST: CompetetiveClassController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompetetiveClassViewModel model)
        {
            try
            {
                _competetiveClassService.Update(model.Name, new CompetetiveClass()
                {
                    Name = model.NewName,
                    CompetitionName = model.CompetitionName,
                    CreatedOn = model.CreatedOn,
                    UpdatedOn = model.UpdatedOn,
                    DeletedOn = model.DeletedOn
                });

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompetetiveClassController/Delete/5
        public ActionResult Delete(string name)
        {
            var competetiveClass = _competetiveClassService.Get(name);

            return View(new CompetetiveClassViewModel()
            {
                Name = competetiveClass.Name
            });
        }

        // POST: CompetetiveClassController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string name, bool notUsed)
        {
            try
            {
                _competetiveClassService.Delete(name);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
