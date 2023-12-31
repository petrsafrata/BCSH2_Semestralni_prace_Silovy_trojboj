using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Semestralni_prace_Silovy_trojboj.Database;
using Semestralni_prace_Silovy_trojboj.Models;
using System;
using System.Linq;
using System.Xml.Linq;

namespace Semestralni_prace_Silovy_trojboj.Controllers
{
    public class ResultsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ResultsController()
        {
            _context = new ApplicationDbContext();
        }

        public IActionResult Index(string disciplineName, string score)
        {

            var results = _context.disciplines;

            if (results == null)
            {
                return Problem("Error");
            }

            var disciplines = from d in results select d;

            if (!string.IsNullOrEmpty(disciplineName))
            {
                disciplines = disciplines.Where(d => d.Name.Contains(disciplineName));
            }

            if (!string.IsNullOrEmpty(score))
            {
                disciplines = disciplines.Where(d => d.Results.Any(r => r.Score.ToString().Equals(score)));
            }

            return View(disciplines.ToList());

        }

        public IActionResult Edit(int id)
        {
            var result = _context.results.FirstOrDefault(c => c.ResultId == id);
            if (result == null)
            {
                return NotFound();
            }

            return View(result);
        }
        
        [HttpPost]
        public IActionResult Edit(Result result)
        {
            if (!int.TryParse(result.Score.ToString(), out int score))
            {
                ModelState.AddModelError("Score", "Score is required");
                return View(result);
            }

            if(result.Score < 0)
            {
                ModelState.AddModelError("Score", "Score must be greater than or equal to 0.");
                return View(result);
            }
            _context.UpdateResults(result);
            return RedirectToAction("Index");
        }
    }
}
