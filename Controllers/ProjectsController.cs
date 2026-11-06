using InsaatYonetimi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsaatYonetimi.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly InsaatYonetimiDbContext _context;

        public ProjectsController(InsaatYonetimiDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var projects = await _context.Projects
                .Where(x => x.CompanyId == companyId.Value)
                .ToListAsync();

            return View(projects);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var project = await _context.Projects
                .Include(x => x.Materials)
                .Include(x => x.Works)
                .ThenInclude(x => x.Worker)
                .Include(x => x.Expenses)
                .FirstOrDefaultAsync(x =>
                    x.ProjectId == id &&
                    x.CompanyId == companyId.Value);

            if (project == null)
                return NotFound();

            return View(project);
        }

        public IActionResult Create()
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ModelState.Remove(nameof(Project.Company));

            if (ModelState.IsValid)
            {
                project.CompanyId = companyId.Value;

                _context.Projects.Add(project);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var project = await _context.Projects
                .FirstOrDefaultAsync(x =>
                    x.ProjectId == id &&
                    x.CompanyId == companyId.Value);

            if (project == null)
                return NotFound();

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            if (id != project.ProjectId)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ModelState.Remove(nameof(Project.Company));

            var existingProject = await _context.Projects
                .FirstOrDefaultAsync(x =>
                    x.ProjectId == id &&
                    x.CompanyId == companyId.Value);

            if (existingProject == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existingProject.Name = project.Name;
                existingProject.Address = project.Address;
                existingProject.StartDate = project.StartDate;
                existingProject.Budget = project.Budget;
                existingProject.Area = project.Area;
                existingProject.FloorCount = project.FloorCount;
                existingProject.ApartmentCount = project.ApartmentCount;
                existingProject.IsActive = project.IsActive;
                existingProject.Status = project.Status;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(project);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var project = await _context.Projects
                .FirstOrDefaultAsync(x =>
                    x.ProjectId == id &&
                    x.CompanyId == companyId.Value);

            if (project == null)
                return NotFound();

            return View(project);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var project = await _context.Projects
                .FirstOrDefaultAsync(x =>
                    x.ProjectId == id &&
                    x.CompanyId == companyId.Value);

            if (project == null)
                return NotFound();

            var works = await _context.Works
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            var materials = await _context.Materials
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            var expenses = await _context.Expenses
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            var incomes = await _context.Incomes
                .Where(x => x.ProjectId == id)
                .ToListAsync();

            _context.Works.RemoveRange(works);
            _context.Materials.RemoveRange(materials);
            _context.Expenses.RemoveRange(expenses);
            _context.Incomes.RemoveRange(incomes);

            _context.Projects.Remove(project);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}