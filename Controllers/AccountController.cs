using InsaatYonetimi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InsaatYonetimi.Controllers
{
    public class AccountController : Controller
    {
        private readonly InsaatYonetimiDbContext _context;

        public AccountController(InsaatYonetimiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string UserName, string Password)
        {
            var user = _context.Users
                .FirstOrDefault(x =>
                    x.UserName == UserName &&
                    x.Password == Password);

            if (user == null)
            {
                ViewBag.Error = "Kullanıcı adı veya şifre hatalı.";
                return View();
            }

            HttpContext.Session.SetInt32("UserId", user.UserId);
            HttpContext.Session.SetInt32("CompanyId", user.CompanyId);
            HttpContext.Session.SetInt32("RoleId", user.RoleId);
            HttpContext.Session.SetString("UserName", user.Name);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Settings()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login");

            var user = _context.Users
                .Include(x => x.Company)
                .FirstOrDefault(x => x.UserId == userId.Value);

            if (user == null)
                return RedirectToAction("Login");

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Settings(User user)
        {
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (userId == null)
                return RedirectToAction("Login");

            var existingUser = _context.Users
                .FirstOrDefault(x => x.UserId == userId.Value);

            if (existingUser == null)
                return RedirectToAction("Login");

            existingUser.Name = user.Name;
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;

            var company = _context.Companies
                .FirstOrDefault(x =>
                    x.CompanyId == existingUser.CompanyId);

            if (company != null && user.Company != null)
            {
                company.Name = user.Company.Name;
            }

            _context.SaveChanges();

            HttpContext.Session.SetString(
                "UserName",
                existingUser.Name);

            return RedirectToAction("Settings");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(string Email)
        {
            var user = _context.Users
                .FirstOrDefault(x => x.Email == Email);

            if (user == null)
            {
                ViewBag.Error = "Bu e-posta adresi bulunamadı.";
                return View();
            }

            HttpContext.Session.SetString("ForgotEmail", Email);
            HttpContext.Session.SetString("VerifyCode", "123456");

            return RedirectToAction("VerifyCode");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(
            string Name,
            string UserName,
            string CompanyName,
            string Email,
            string Password,
            string PasswordAgain)
        {
            if (Password != PasswordAgain)
            {
                ViewBag.Error = "Şifreler aynı değil.";
                return View();
            }

            var kontrol = _context.Users
                .FirstOrDefault(x => x.UserName == UserName);

            if (kontrol != null)
            {
                ViewBag.Error = "Bu kullanıcı adı zaten kullanılıyor.";
                return View();
            }

            User user = new User();

            user.Name = Name;
            user.UserName = UserName;
            user.Email = Email;
            user.Password = Password;

            Company company = new Company();

            company.Name = CompanyName;

            _context.Companies.Add(company);
            _context.SaveChanges();

            user.CompanyId = company.CompanyId;
            user.RoleId = 2;

            _context.Users.Add(user);
            _context.SaveChanges();

            TempData["Message"] = "Hesabınız başarıyla oluşturuldu.";

            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult VerifyCode()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyCode(string Code)
        {
            var verifyCode =
                HttpContext.Session.GetString("VerifyCode");

            if (Code != verifyCode)
            {
                ViewBag.Error = "Doğrulama kodu hatalı.";
                return View();
            }

            return RedirectToAction("ResetPassword");
        }

        [HttpGet]
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(
            string NewPassword,
            string NewPasswordAgain)
        {
            if (NewPassword != NewPasswordAgain)
            {
                ViewBag.Error = "Şifreler aynı değil.";
                return View();
            }

            var email =
                HttpContext.Session.GetString("ForgotEmail");

            var user = _context.Users
                .FirstOrDefault(x => x.Email == email);

            if (user == null)
            {
                ViewBag.Error = "Kullanıcı bulunamadı.";
                return View();
            }

            user.Password = NewPassword;

            _context.SaveChanges();

            HttpContext.Session.Remove("ForgotEmail");
            HttpContext.Session.Remove("VerifyCode");

            TempData["Message"] =
                "Şifreniz başarıyla güncellendi.";

            return RedirectToAction("Login");
        }
    }
}