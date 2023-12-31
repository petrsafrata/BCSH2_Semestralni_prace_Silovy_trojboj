using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var competitors = _context.competitors;
            return View(competitors);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.categories, "CategoryId", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Competitor competitor)
        {
            if (string.IsNullOrWhiteSpace(competitor.CompetitorFirstName) || string.IsNullOrWhiteSpace(competitor.CompetitorLastName) || !double.TryParse(competitor.Weight.ToString(), out double weight))
            {

                if (string.IsNullOrWhiteSpace(competitor.CompetitorFirstName))
                {
                    ModelState.AddModelError("CompetitorFirstName", "First name is required.");
                }

                if (string.IsNullOrWhiteSpace(competitor.CompetitorLastName))
                {
                    ModelState.AddModelError("CompetitorLastName", "Last name is required.");
                }

                if (!double.TryParse(competitor.Weight.ToString(), out _))
                {
                    ModelState.AddModelError("Weight", "Weight is required");
                }
                ViewBag.Categories = new SelectList(_context.categories, "CategoryId", "Name");
                return View(competitor);
            }
            _context.AddCompetitor(competitor.CompetitorFirstName, competitor.CompetitorLastName, competitor.CategoryId, competitor.Weight);
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var competitor = _context.competitors.FirstOrDefault(c => c.CompetitorId == id);
            if (competitor == null)
            {
                return NotFound();
            }
            ViewBag.Categories = new SelectList(_context.categories, "CategoryId", "Name", competitor.CategoryId);

            return View(competitor);
        }

        [HttpPost]
        public IActionResult Edit(Competitor competitor)
        {
            if (string.IsNullOrWhiteSpace(competitor.CompetitorFirstName) || string.IsNullOrWhiteSpace(competitor.CompetitorLastName) || !double.TryParse(competitor.Weight.ToString(), out double weight))
            {
                if (string.IsNullOrWhiteSpace(competitor.CompetitorFirstName))
                {
                    ModelState.AddModelError("CompetitorFirstName", "First name is required.");
                }

                if (string.IsNullOrWhiteSpace(competitor.CompetitorLastName))
                {
                    ModelState.AddModelError("CompetitorLastName", "Last name is required.");
                }

                if (!double.TryParse(competitor.Weight.ToString(), out _))
                {
                    ModelState.AddModelError("Weight", "Weight is required");
                }
                
                ViewBag.Categories = new SelectList(_context.categories, "CategoryId", "Name");
                return View(competitor);
            }
            _context.UpdateCompetitor(competitor);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            var competitor = _context.competitors.FirstOrDefault(c => c.CompetitorId == id);
            if (competitor == null)
            {
                return NotFound();
            }
            return View(competitor);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _context.DeleteCompetitor(id);
            return RedirectToAction("Index");
        }
    }
}
