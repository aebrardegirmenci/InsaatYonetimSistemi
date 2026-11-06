using InsaatYonetimi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace InsaatYonetimi.Controllers
{
    public class HomeController : Controller
    {
        private readonly InsaatYonetimiDbContext _context;

        public HomeController(InsaatYonetimiDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var projects = _context.Projects
                .Include(x => x.Expenses)
                .Include(x => x.Materials)
                .Include(x => x.Works)
                .ThenInclude(x => x.Worker)
                .Where(x => x.CompanyId == companyId.Value)
                .ToList();

            ViewBag.ProjectCount = projects.Count;

            decimal totalExpense = 0;

            foreach (var project in projects)
            {
                decimal materialExpense = project.Materials
                    .Sum(x => x.Quantity * x.Price);

                decimal workExpense = project.Works
                    .Sum(x => x.DayCount * x.Worker.DailyPay);

                decimal otherExpense = project.Expenses
                    .Sum(x => x.Amount);

                totalExpense += materialExpense + workExpense + otherExpense;
            }

            ViewBag.TotalExpense = totalExpense;

            ViewBag.TotalIncome = _context.Incomes
                .Where(x => x.Project.CompanyId == companyId.Value)
                .Sum(x => x.Amount);

            decimal totalBudget = projects.Sum(x => x.Budget);

            ViewBag.RemainingBudget =
                totalBudget - totalExpense;

            return View(projects);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(
            Duration = 0,
            Location = ResponseCacheLocation.None,
            NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id
                    ?? HttpContext.TraceIdentifier
            });
        }
    }
}