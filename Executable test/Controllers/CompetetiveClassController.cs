using BussinessLayer.Models;
using BussinessLayer.Services;
using DanceCompetitionApp;
using Executable_test.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.Caching.Memory;
using System.Reflection;
using System.Text.Json;

namespace Executable_test.Controllers
{
    public class CompetetiveClassController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CompetetiveClassService _competetiveClassService;
        private readonly CoupleService _coupleService;
        private readonly IMemoryCache _memeoryCache;

        public CompetetiveClassController(
            ILogger<HomeController> logger,
            IMemoryCache memeoryCache,
            CompetetiveClassService competetiveClassService,
            CoupleService coupleService)
        {
            _logger = logger;
            _competetiveClassService = competetiveClassService;
            _memeoryCache = memeoryCache;
            _coupleService = coupleService;
        }
        // GET: CompetetiveClassController
        public ActionResult Index(string name)
        {
            if (name != null)
            {
                _memeoryCache.Set<string>("competition", name, TimeSpan.FromMinutes(450));
            }

            var competitionName = (string)_memeoryCache.Get("competition");

            if (competitionName == null)
            {
                return RedirectToAction("Index", "Competition");
            }
            var all = _competetiveClassService.GetAll(competitionName).ToList();

            var classes = all.Select(a =>
            new CompetetiveClassViewModel()
            {
                Name = a.Name,
                CouplesCount = a.CouplesCount,
                JudgesCount = a.JudgesCount,
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
        public ActionResult Create(CompetetiveClass model)
        {

            var competitionName = _memeoryCache.Get("competition").ToString();
            if (competitionName == null)
            {
                return RedirectToAction("Index", "Competition");
            }
            var competetiveClass = new CompetetiveClass()
            {
                Name = model.Name,
                JudgesCount = model.JudgesCount,
                CouplesCount = model.CouplesCount,
                CompetitionName = competitionName
            };

            _competetiveClassService.Create(competetiveClass);
            return RedirectToAction(nameof(Index));


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

        public ActionResult Judge(string name)
        {
            try
            {
                var competetiveClass = _competetiveClassService.Get(name);

                var model = new JudgingViewModel()
                {
                    CompetitionName = competetiveClass.Name,
                };

                var couples = _coupleService.GetAll(name);

                var judgesCount = couples.First().JudgesCount;

                for (int c = 0; c < int.Parse(judgesCount); c++)
                {
                    var judgeName = $"Judge{((char)(c + 65)).ToString()}";

                    model.JudgePlacing.Add(judgeName, couples.Select(c => ((string)c.Name, "0")).ToList());
                }

                return View(model);
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));

            }

        }

        [HttpPost]
        public ActionResult Judge([FromBody] JsonElement modelData)
        {
            var model = DataBind(modelData);
            var competitionName = model.CompetitionName;
            var couples = _coupleService.GetAll(competitionName);

            foreach (var c in couples)
            {
                var judgePlace = new List<(string, int)>();

                foreach (var jp in model.JudgePlacing)
                {
                    foreach (var couplePlace in jp.Value)
                    {
                        if (couplePlace.Couple == c.Name)
                        {
                            judgePlace.Add((jp.Key, int.Parse(couplePlace.Place)));
                        }

                    }
                }
                _coupleService.Adjuicate(c.Name, competitionName, judgePlace);
            }


            return Ok(model.CompetitionName);
        }

        public JudgingViewModel DataBind(JsonElement model)
        {
            var dict = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(model);
            var name = dict.Last().Value.ToString();

            dict.Remove("name");

            var modelToAdd = new JudgingViewModel()
            {
                CompetitionName = name,
            };

            foreach (var s in dict)
            {
                var judgePlaceArr = JsonSerializer.Deserialize<List<object>>(s.Value);
                var judgePlaceList = new List<(string, string)>();

                foreach (var kvp in judgePlaceArr)
                {
                    var str = JsonSerializer.Deserialize<List<string>>(kvp);

                    judgePlaceList.Add(((string)str[0], (string)str[1]));
                }

                modelToAdd.JudgePlacing.Add(s.Key, judgePlaceList);
            }
            return modelToAdd;
        }
    }
}
