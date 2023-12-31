using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Semestralni_prace_Silovy_trojboj.Database;
using Semestralni_prace_Silovy_trojboj.Models;

namespace Semestralni_prace_Silovy_trojboj.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController() {
            _context = new ApplicationDbContext();
        }

        public IActionResult Index()
        {
            var categories = _context.categories;
            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if (string.IsNullOrWhiteSpace(category.Name) || !double.TryParse(category.Weight.ToString(), out double weight) || category.Weight <= 30 || category.Weight > 120)
            {

                if (string.IsNullOrWhiteSpace(category.Name))
                {
                    ModelState.AddModelError("Name", "Name is required.");
                }
                if (!double.TryParse(category.Weight.ToString(), out _) || category.Weight <= 30 || category.Weight > 120)
                {
                    ModelState.AddModelError("Weight", "Weight must be between 30 and 120.");
                }

                return View(category);
            }
            _context.AddCategory(category.Name, category.Weight);
            return RedirectToAction("Index");
        }
    }
}
