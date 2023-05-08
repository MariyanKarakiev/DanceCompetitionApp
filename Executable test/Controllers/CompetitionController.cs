using BussinessLayer.Models;
using BussinessLayer.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Executable_test.Controllers
{
    public class CompetitionController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CompetitionService _competitionService;
        private readonly CompetetiveClassService _competetiveClassService;
        public CompetitionController(ILogger<HomeController> logger, CompetitionService competitionService, CompetetiveClassService competetiveClassService)
        {
            _logger = logger;
            _competitionService = competitionService;
            _competetiveClassService = competetiveClassService;
        }
        // GET: CompetitionController
        public ActionResult Index()
        {
            var all = _competitionService.GetAll();

            return View(all);
        }

        // GET: CompetitionController/Details/5
        public ActionResult Details(string competition)
        {
            return RedirectToRoute("CompetetiveClass", competition);
        }

        // GET: CompetitionController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompetitionController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Competition competition)
        {
            try
            {
                _competitionService.Create(competition.Name);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompetitionController/Edit/5
        public ActionResult Edit(string name)
        {
            var competition1 = _competitionService.Get(name);

            return View(new CompetitionViewModel()
            {
                Name = competition1.Name,
                NewName = competition1.Name,
                CreatedOn = competition1.CreatedOn,
                DeletedOn = competition1.DeletedOn,
                UpdatedOn = competition1.UpdatedOn
            });
        }

        // POST: CompetitionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(CompetitionViewModel model)
        {
            try
            {
                _competitionService.Update(model.Name, new Competition()
                {
                    Name = model.NewName,
                    CreatedOn = model.CreatedOn,
                    UpdatedOn = model.UpdatedOn
                });

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompetitionController/Delete/5
        public ActionResult Delete(string name)
        {
            var competition = _competitionService.Get(name);

            return View(new CompetitionViewModel()
            {
                Name = competition.Name
            });
        }

        // POST: CompetitionController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(string name, bool notUsed)
        {
            try
            {
                _competitionService.Delete(name);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
