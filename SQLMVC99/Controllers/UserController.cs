using Microsoft.AspNetCore.Mvc;
using SQLMVC99.Models;

namespace SQLMVC99.Controllers
{
    public class UserController : Controller
    {
        private readonly MyDbContext _context;

        public UserController()
        {
            _context = new MyDbContext();
        }

        // Action لعرض قائمة المستخدمين
        public ActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        // Action لعرض نموذج التعديل
        public ActionResult Edit(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // Action لحفظ التعديلات
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, User updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(updatedUser);
                    _context.SaveChanges();
                }
                catch (Exception)
                {
                    if (!_context.Users.Any(u => u.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(updatedUser);
        }

        // Action لعرض نموذج الحذف
        public ActionResult Delete(int id)
        {
            var user = _context.Users.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Action لعرض نموذج إضافة مستخدم جديد
        public ActionResult Create()
        {
            return View();
        }

        // Action لحفظ بيانات المستخدم الجديد
        [HttpPost]
        public ActionResult Create(User newUser)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(newUser);
                _context.SaveChanges();
                return RedirectToAction("Index"); 
            }
            return View(newUser);
        }
    }
}

