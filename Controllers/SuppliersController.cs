using InsaatYonetimi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsaatYonetimi.Controllers
{
    public class SuppliersController : Controller
    {
        private readonly InsaatYonetimiDbContext _context;

        public SuppliersController(InsaatYonetimiDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var suppliers = await _context.Suppliers
                .Where(x => x.CompanyId == companyId)
                .ToListAsync();

            return View(suppliers);
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
    [Bind("Name,Phone,Address")] Supplier supplier)
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ModelState.Remove(nameof(Supplier.Company));

            if (ModelState.IsValid)
            {
                supplier.CompanyId = companyId.Value;

                _context.Suppliers.Add(supplier);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(supplier);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(x =>
                    x.SupplierId == id &&
                    x.CompanyId == companyId.Value);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(x =>
                    x.SupplierId == id &&
                    x.CompanyId == companyId.Value);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
    int id,
    [Bind("SupplierId,Name,Phone,Address")] Supplier supplier)
        {
            if (id != supplier.SupplierId)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            ModelState.Remove(nameof(Supplier.Company));

            var existingSupplier = await _context.Suppliers
                .FirstOrDefaultAsync(x =>
                    x.SupplierId == id &&
                    x.CompanyId == companyId.Value);

            if (existingSupplier == null)
                return NotFound();

            if (ModelState.IsValid)
            {
                existingSupplier.Name = supplier.Name;
                existingSupplier.Phone = supplier.Phone;
                existingSupplier.Address = supplier.Address;

                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(supplier);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(x =>
                    x.SupplierId == id &&
                    x.CompanyId == companyId.Value);

            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            int? companyId = HttpContext.Session.GetInt32("CompanyId");

            if (companyId == null)
                return RedirectToAction("Login", "Account");

            var supplier = await _context.Suppliers
                .FirstOrDefaultAsync(x =>
                    x.SupplierId == id &&
                    x.CompanyId == companyId.Value);

            if (supplier != null)
            {
                _context.Suppliers.Remove(supplier);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}