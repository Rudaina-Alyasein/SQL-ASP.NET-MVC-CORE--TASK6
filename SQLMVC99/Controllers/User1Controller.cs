using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Scripting;
using Microsoft.EntityFrameworkCore;
using SQLMVC99.Models;

namespace SQLMVC99.Controllers
{
    public class User1Controller : Controller
    {
        private readonly MyDbContext _context;

        public User1Controller(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            Usernew newUser = new Usernew();
            return View(newUser);
        }
        [HttpPost]
        public IActionResult Register(Usernew user)
        {
            if (ModelState.IsValid)
            {
                if (user.Password != user.ConfirmPassword)
                {
                    ModelState.AddModelError("ConfirmPassword", "The password and confirmation password do not match.");
                    return View("Index");
                }

                _context.Usernews.Add(user);
                _context.SaveChanges();
                return RedirectToAction("Login");
            }

            return View("Index");
        }


        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string Email, string Password)
        {
            // التحقق من وجود المستخدم في قاعدة البيانات
            var user = _context.Usernews.FirstOrDefault(u => u.Email == Email && u.Password == Password);

            if (user != null)
            {
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", user.Name); 
                HttpContext.Session.SetString("UserRole", user.Role);
                HttpContext.Session.SetString("UserPassword", user.Password);



                // التوجيه حسب دور المستخدم
                if (user.Role == "Admin")
                {
                    return RedirectToAction("Index", "Admin");
                }
                else if (user.Role == "User")
                {
                    return RedirectToAction("Index", "Product");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ViewBag.ErrorMessage = "Invalid email or password!";
            return View();
        }
        public IActionResult Profile()
        {
            // جلب البيانات من الـ Session
            var userEmail = HttpContext.Session.GetString("UserEmail");
            var userName = HttpContext.Session.GetString("UserName");
            var userRole = HttpContext.Session.GetString("UserRole");
            var userPassword = HttpContext.Session.GetString("UserPassword");


            // إرسال البيانات للـ View باستخدام ViewBag أو Model
            ViewBag.UserEmail = userEmail;
            ViewBag.UserName = userName;
            ViewBag.UserRole = userRole;
            var user = new Usernew
            {
                Email = userEmail,
                Name = userName,
                Role = userRole,
                Password = userPassword

            };
            return View(user);
        }
        public IActionResult EditProfile()
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login");
            }

            var user = _context.Usernews.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            return View("Profile",user);
        }
        [HttpPost]
        public IActionResult EditProfile(Usernew model)
        {
            var userEmail = HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(userEmail))
            {
                return RedirectToAction("Login");
            }

            var user = _context.Usernews.FirstOrDefault(u => u.Email == userEmail);
            if (user == null)
            {
                return NotFound();
            }

            // تحديث بيانات المستخدم
            user.Name = model.Name;
            user.Email = model.Email;

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.Password = model.Password;
            }

            _context.SaveChanges();

            HttpContext.Session.SetString("UserEmail", user.Email);
            HttpContext.Session.SetString("UserName", user.Name);
            HttpContext.Session.SetString("UserRole", user.Role);
            TempData["SuccessMessage"] = "Edit done succesfully!";

            return RedirectToAction("Profile"); 
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }


    }
}
