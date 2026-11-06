using InsaatYonetimi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsaatYonetimi.Controllers
{
    public class WorkersController : Controller
    {
        private readonly InsaatYonetimiDbContext _context;

        public WorkersController(InsaatYonetimiDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var workers = await _context.Workers
                .Where(x => x.CompanyId == companyId.Value)
                .ToListAsync();

            return View(workers);
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
        public async Task<IActionResult> Create(
            [Bind("Name,Job,DailyPay,Phone,Address")] Worker worker)
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ModelState.Remove(nameof(Worker.Company));

            if (ModelState.IsValid)
            {
                worker.CompanyId = companyId.Value;

                _context.Workers.Add(worker);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(worker);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var worker = await _context.Workers
                .FirstOrDefaultAsync(x =>
                    x.WorkerId == id &&
                    x.CompanyId == companyId.Value);

            if (worker == null)
                return NotFound();

            return View(worker);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var worker = await _context.Workers
                .FirstOrDefaultAsync(x =>
                    x.WorkerId == id &&
                    x.CompanyId == companyId.Value);

            if (worker == null)
                return NotFound();

            return View(worker);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("WorkerId,Name,Job,DailyPay,Phone,Address")] Worker worker)
        {
            if (id != worker.WorkerId)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ModelState.Remove(nameof(Worker.Company));

            var existingWorker = await _context.Workers
                .FirstOrDefaultAsync(x =>
                    x.WorkerId == id &&
                    x.CompanyId == companyId.Value);

            if (existingWorker == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existingWorker.Name = worker.Name;
                existingWorker.Job = worker.Job;
                existingWorker.DailyPay = worker.DailyPay;
                existingWorker.Phone = worker.Phone;
                existingWorker.Address = worker.Address;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(worker);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var worker = await _context.Workers
                .FirstOrDefaultAsync(x =>
                    x.WorkerId == id &&
                    x.CompanyId == companyId.Value);

            if (worker == null)
                return NotFound();

            return View(worker);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var worker = await _context.Workers
                .FirstOrDefaultAsync(x =>
                    x.WorkerId == id &&
                    x.CompanyId == companyId.Value);

            if (worker != null)
            {
                _context.Workers.Remove(worker);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}