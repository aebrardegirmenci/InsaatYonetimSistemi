using InsaatYonetimi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsaatYonetimi.Controllers
{
    public class IncomesController : Controller
    {
        private readonly InsaatYonetimiDbContext _context;

        public IncomesController(InsaatYonetimiDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var incomes = await _context.Incomes
                .Include(x => x.Project)
                .Where(x => x.Project.CompanyId == companyId.Value)
                .ToListAsync();

            return View(incomes);
        }

        public async Task<IActionResult> Create()
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Projects = await _context.Projects
                .Where(x => x.CompanyId == companyId.Value)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Income income)
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ModelState.Remove(nameof(Income.Project));

            if (ModelState.IsValid)
            {
                _context.Incomes.Add(income);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Projects = await _context.Projects
                .Where(x => x.CompanyId == companyId.Value)
                .ToListAsync();

            return View(income);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var income = await _context.Incomes
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x => x.IncomeId == id);

            if (income == null)
                return NotFound();

            return View(income);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var income = await _context.Incomes
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x => x.IncomeId == id);

            if (income == null)
                return NotFound();

            ViewBag.Projects = await _context.Projects
                .ToListAsync();

            return View(income);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Income income)
        {
            if (id != income.IncomeId)
                return NotFound();

            ModelState.Remove(nameof(Income.Project));

            if (ModelState.IsValid)
            {
                _context.Incomes.Update(income);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Projects = await _context.Projects
                .ToListAsync();

            return View(income);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var income = await _context.Incomes
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x => x.IncomeId == id);

            if (income == null)
                return NotFound();

            return View(income);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var income = await _context.Incomes
                .FirstOrDefaultAsync(x => x.IncomeId == id);

            if (income != null)
            {
                _context.Incomes.Remove(income);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}