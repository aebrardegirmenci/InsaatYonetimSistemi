using InsaatYonetimi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsaatYonetimi.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly InsaatYonetimiDbContext _context;

        public MaterialsController(InsaatYonetimiDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var materials = await _context.Materials
                .Include(x => x.Project)
                .Include(x => x.Supplier)
                .Where(x => x.Project.CompanyId == companyId.Value)
                .ToListAsync();

            return View(materials);
        }

        public async Task<IActionResult> Create()
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ViewBag.Projects = await _context.Projects
                .Where(x => x.CompanyId == companyId.Value)
                .ToListAsync();

            ViewBag.Suppliers = await _context.Suppliers
                .Where(x => x.CompanyId == companyId.Value)
                .ToListAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material material)
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ModelState.Remove(nameof(Material.Project));
            ModelState.Remove(nameof(Material.Supplier));

            if (ModelState.IsValid)
            {
                _context.Materials.Add(material);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Projects = await _context.Projects
                .Where(x => x.CompanyId == companyId.Value)
                .ToListAsync();

            ViewBag.Suppliers = await _context.Suppliers
                .Where(x => x.CompanyId == companyId.Value)
                .ToListAsync();

            return View(material);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            var material = await _context.Materials
                .Include(x => x.Project)
                .Include(x => x.Supplier)
                .FirstOrDefaultAsync(x => x.MaterialId == id);

            if (material == null)
                return NotFound();

            return View(material);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var material = await _context.Materials
                .Include(x => x.Project)
                .Include(x => x.Supplier)
                .FirstOrDefaultAsync(x => x.MaterialId == id);

            if (material == null)
                return NotFound();

            ViewBag.Projects = await _context.Projects
                .ToListAsync();

            ViewBag.Suppliers = await _context.Suppliers
                .ToListAsync();

            return View(material);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Material material)
        {
            if (id != material.MaterialId)
                return NotFound();

            ModelState.Remove(nameof(Material.Project));
            ModelState.Remove(nameof(Material.Supplier));

            if (ModelState.IsValid)
            {
                _context.Materials.Update(material);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Projects = await _context.Projects
                .ToListAsync();

            ViewBag.Suppliers = await _context.Suppliers
                .ToListAsync();

            return View(material);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var material = await _context.Materials
                .Include(x => x.Project)
                .Include(x => x.Supplier)
                .FirstOrDefaultAsync(x => x.MaterialId == id);

            if (material == null)
                return NotFound();

            return View(material);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var material = await _context.Materials
                .FirstOrDefaultAsync(x => x.MaterialId == id);

            if (material != null)
            {
                _context.Materials.Remove(material);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}