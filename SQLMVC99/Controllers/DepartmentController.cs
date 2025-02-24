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


        public ActionResult Index()
        {
            var Departments = _context.Departments.ToList();
            return View(Departments);
        }
        public ActionResult Create()
        {
            Department newDepartment = new Department();
            return View(newDepartment);
        }



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

        public ActionResult Edit(int id)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        public ActionResult Edit(int id, Department updatedDepartment)
        {
            if (id != updatedDepartment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                
                    _context.Update(updatedDepartment);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
              
               
                
                return RedirectToAction(nameof(Index));
            }
            return View(updatedDepartment);
        }

        public ActionResult Delete(int Id)
        {
            var department = _context.Departments.Find(Id);
            if (department == null)
            {
                return NotFound();
            }

            _context.Departments.Remove(department);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        
        public ActionResult Details(int id)
        {
            var department = _context.Departments.Find(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }
    }
}

