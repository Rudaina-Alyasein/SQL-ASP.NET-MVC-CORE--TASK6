using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLMVC99.Models;
using System.Diagnostics;

namespace SQLMVC99.Controllers
{
    public class AdminController : Controller
    {
        private readonly MyDbContext _context;
        public AdminController(MyDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Users()
        {
            var UsersInfo = _context.Usernews.ToList();
            return View(UsersInfo);
        }



        [HttpPost]
        public IActionResult Create(IFormCollection form)
        {
            var name = form["Name"];
            var email = form["Email"];
            var password = form["Password"];
            var role = form["Role"];

            var newUser = new Usernew
            {
                Name = name,
                Email = email,
                Password = password,
                Role = role
            };

            _context.Usernews.Add(newUser);
            _context.SaveChanges();

            return RedirectToAction("Users");
        }
        // صفحة تعديل بيانات المستخدم
        public IActionResult Edit(int id)
        {
            var user = _context.Usernews.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        [HttpPost]
        public IActionResult Edit(int id, Usernew updatedUser)
        {
            if (id != updatedUser.Id)
            {
                return BadRequest();
            }

            var user = _context.Usernews.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            // Update user details
            user.Name = updatedUser.Name;
            user.Email = updatedUser.Email;
            user.Password = updatedUser.Password;
            user.Role = updatedUser.Role;

            _context.SaveChanges();

            return RedirectToAction("Users");
        }

        public ActionResult Delete(int Id)
        {
            var user = _context.Usernews.Find(Id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Usernews.Remove(user);
            _context.SaveChanges();
            return RedirectToAction("Users");
        }
        //Product management
        public IActionResult Products()
        {
            var ProductsInfo = _context.Products.ToList();
            return View(ProductsInfo);
        }
        [HttpPost]
        public IActionResult CreateProduct(IFormCollection form)
        {
            var name = form["Name"];
            var price = form["Price"];
            var description = form["Description"];
            var file = form.Files["ProductImage"]; 

            string imagePath = null;

            if (file != null && file.Length > 0)
            {
                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                imagePath = $"/uploads/{uniqueFileName}";
            }

            var newProduct = new Product
            {
                Name = name,
                Price =  price,
                Description = description,
               Image = imagePath 
            };

            _context.Products.Add(newProduct);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
        public ActionResult EditProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View( product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProduct(int id, Product updatedProduct)
        {
            if (id != updatedProduct.Id)
            {
                return NotFound();
            }

          
                var existingProduct = _context.Products.Find(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                existingProduct.Name = updatedProduct.Name;
                existingProduct.Description = updatedProduct.Description;
                existingProduct.Price = updatedProduct.Price;

              _context.SaveChanges();

                return RedirectToAction(nameof(Index));
         
        }
        public ActionResult DeleteProduct(int Id)
        {
            var product =_context.Products.Find(Id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return RedirectToAction("Products");
        }


    }
}

