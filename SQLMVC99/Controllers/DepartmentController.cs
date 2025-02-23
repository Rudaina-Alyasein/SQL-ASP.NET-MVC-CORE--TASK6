using Microsoft.AspNetCore.Mvc;
using SQLMVC99.Models;

namespace SQLMVC99.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly MyDbContext _context;

        public DepartmentController()
        {
            _context = new MyDbContext();
        }


        // Action لعرض قائمة المستخدمين
        public ActionResult Index()
        {
            var Departments = _context.Departments.ToList();
            return View(Departments);
        }

        // Action لعرض نموذج التعديل
        public ActionResult Edit(int id)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        // Action لحفظ التعديلات
        [HttpPost]
        public ActionResult Edit(int id, Department updatedDepartment)
        {
            if (id != updatedDepartment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(updatedDepartment);
                    _context.SaveChanges();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    if (!_context.Departments.Any(u => u.Id == id))
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
            return View(updatedDepartment);
        }

        // Action لعرض نموذج الحذف
        public ActionResult Delete(int id)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        // Action لعرض نموذج إضافة مستخدم جديد
        public ActionResult Create()
        {
            var newDepartment = new Department(); // Initialize a new empty department
            return View(newDepartment); // Pass it to the view
        }


        // Action لحفظ بيانات المستخدم الجديد
        [HttpPost]
        public ActionResult Create(Department newDepartment)
        {
            if (ModelState.IsValid)
            {
                _context.Departments.Add(newDepartment);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(newDepartment);
        }
    }
}

