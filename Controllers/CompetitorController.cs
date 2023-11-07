using Microsoft.AspNetCore.Mvc;
using Semestralni_prace_Silovy_trojboj.Database;
using Semestralni_prace_Silovy_trojboj.Models;

namespace Semestralni_prace_Silovy_trojboj.Controllers
{
    public class CompetitorController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompetitorController()
        {
            _context = new ApplicationDbContext();
        }

        public IActionResult Index()
        {
            var competitors = _context.GetAllCompetitors();
            return View(competitors);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Competitor competitor)
        {
            if (ModelState.IsValid)
            {
                _context.AddCompetitor(competitor.CompetitorFirstName, competitor.CompetitorLastName, competitor.CategoryId, competitor.Weight);
                return RedirectToAction("Index");
            }
            return View(competitor);
        }

        public IActionResult Edit(int id)
        {
            var competitor = _context.GetAllCompetitors().FirstOrDefault(c => c.CompetitorId == id);
            if (competitor == null)
            {
                return NotFound();
            }
            return View(competitor);
        }

        [HttpPost]
        public IActionResult Edit(Competitor competitor)
        {
            if (ModelState.IsValid)
            {
                _context.UpdateCompetitor(competitor);
                return RedirectToAction("Index");
            }
            return View(competitor);
        }

        public IActionResult Delete(int id)
        {
            var competitor = _context.GetAllCompetitors().FirstOrDefault(c => c.CompetitorId == id);
            if (competitor == null)
            {
                return NotFound();
            }
            return View(competitor);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _context.DeleteCompetitor(id);
            return RedirectToAction("Index");
        }
    }
}
