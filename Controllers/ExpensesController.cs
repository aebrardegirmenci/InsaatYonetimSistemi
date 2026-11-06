using InsaatYonetimi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsaatYonetimi.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly InsaatYonetimiDbContext _context;

        public ExpensesController(InsaatYonetimiDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var expenses = _context.Expenses
                .Include(x => x.Project)
                .Where(x => x.Project.CompanyId == companyId.Value);

            return View(await expenses.ToListAsync());
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
        public async Task<IActionResult> Create(
            [Bind("ExpenseId,Name,Amount,Date,ProjectId")] Expense expense)
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ModelState.Remove(nameof(Expense.Project));

            var project = await _context.Projects
                .FirstOrDefaultAsync(x =>
                    x.ProjectId == expense.ProjectId &&
                    x.CompanyId == companyId.Value);

            if (project == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Expenses.Add(expense);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Projects = await _context.Projects
                .Where(x => x.CompanyId == companyId.Value)
                .ToListAsync();

            return View(expense);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var expense = await _context.Expenses
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x =>
                    x.ExpenseId == id &&
                    x.Project.CompanyId == companyId.Value);

            if (expense == null)
                return NotFound();

            return View(expense);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var expense = await _context.Expenses
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x =>
                    x.ExpenseId == id &&
                    x.Project.CompanyId == companyId.Value);

            if (expense == null)
                return NotFound();

            ViewBag.Projects = await _context.Projects
                .Where(x => x.CompanyId == companyId.Value)
                .ToListAsync();

            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("ExpenseId,Name,Amount,Date,ProjectId")] Expense expense)
        {
            if (id != expense.ExpenseId)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ModelState.Remove(nameof(Expense.Project));

            var existingExpense = await _context.Expenses
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x =>
                    x.ExpenseId == id &&
                    x.Project.CompanyId == companyId.Value);

            if (existingExpense == null)
                return NotFound();

            var project = await _context.Projects
                .FirstOrDefaultAsync(x =>
                    x.ProjectId == expense.ProjectId &&
                    x.CompanyId == companyId.Value);

            if (project == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existingExpense.Name = expense.Name;
                existingExpense.Amount = expense.Amount;
                existingExpense.Date = expense.Date;
                existingExpense.ProjectId = expense.ProjectId;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Projects = await _context.Projects
                .Where(x => x.CompanyId == companyId.Value)
                .ToListAsync();

            return View(expense);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var expense = await _context.Expenses
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x =>
                    x.ExpenseId == id &&
                    x.Project.CompanyId == companyId.Value);

            if (expense == null)
                return NotFound();

            return View(expense);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var expense = await _context.Expenses
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x =>
                    x.ExpenseId == id &&
                    x.Project.CompanyId == companyId.Value);

            if (expense != null)
            {
                _context.Expenses.Remove(expense);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}