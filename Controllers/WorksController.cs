using InsaatYonetimi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsaatYonetimi.Controllers
{
    public class WorksController : Controller
    {
        private readonly InsaatYonetimiDbContext _context;

        public WorksController(InsaatYonetimiDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var works = await _context.Works
                .Include(x => x.Worker)
                .Include(x => x.Project)
                .Where(x => x.Project.CompanyId == companyId)
                .ToListAsync();

            return View(works);
        }

        public async Task<IActionResult> Create()
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            await LoadLists(companyId.Value);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Work work)
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ModelState.Remove(nameof(Work.Worker));
            ModelState.Remove(nameof(Work.Project));

            var project = await _context.Projects
                .FirstOrDefaultAsync(x =>
                    x.ProjectId == work.ProjectId &&
                    x.CompanyId == companyId.Value);

            var worker = await _context.Workers
                .FirstOrDefaultAsync(x =>
                    x.WorkerId == work.WorkerId &&
                    x.CompanyId == companyId.Value);

            if (project == null || worker == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                _context.Works.Add(work);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await LoadLists(companyId.Value);

            return View(work);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var work = await _context.Works
                .Include(x => x.Worker)
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x =>
                    x.WorkId == id &&
                    x.Project.CompanyId == companyId);

            if (work == null)
                return NotFound();

            return View(work);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var work = await _context.Works
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x =>
                    x.WorkId == id &&
                    x.Project.CompanyId == companyId);

            if (work == null)
                return NotFound();

            await LoadLists(companyId.Value);

            return View(work);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Work work)
        {
            if (id != work.WorkId)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ModelState.Remove(nameof(Work.Worker));
            ModelState.Remove(nameof(Work.Project));

            var existingWork = await _context.Works
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x =>
                    x.WorkId == id &&
                    x.Project.CompanyId == companyId);

            if (existingWork == null)
                return NotFound();

            var project = await _context.Projects
                .FirstOrDefaultAsync(x =>
                    x.ProjectId == work.ProjectId &&
                    x.CompanyId == companyId);

            var worker = await _context.Workers
                .FirstOrDefaultAsync(x =>
                    x.WorkerId == work.WorkerId &&
                    x.CompanyId == companyId);

            if (project == null || worker == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existingWork.WorkerId = work.WorkerId;
                existingWork.ProjectId = work.ProjectId;
                existingWork.StartDate = work.StartDate;
                existingWork.DayCount = work.DayCount;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            await LoadLists(companyId.Value);

            return View(work);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var work = await _context.Works
                .Include(x => x.Worker)
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x =>
                    x.WorkId == id &&
                    x.Project.CompanyId == companyId);

            if (work == null)
                return NotFound();

            return View(work);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var work = await _context.Works
                .Include(x => x.Project)
                .FirstOrDefaultAsync(x =>
                    x.WorkId == id &&
                    x.Project.CompanyId == companyId);

            if (work != null)
            {
                _context.Works.Remove(work);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadLists(int companyId)
        {
            ViewBag.Workers = await _context.Workers
                .Where(x => x.CompanyId == companyId)
                .OrderBy(x => x.Name)
                .ToListAsync();

            ViewBag.Projects = await _context.Projects
                .Where(x => x.CompanyId == companyId)
                .OrderBy(x => x.Name)
                .ToListAsync();
        }
    }
}