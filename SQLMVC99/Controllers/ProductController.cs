using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLMVC99.Models;

namespace SQLMVC99.Controllers
{
    public class ProductController : Controller
    {
        private readonly MyDbContext dbContext;
        public ProductController()
        {
            dbContext = new MyDbContext();
        }
        public IActionResult Index()
        {
            var Product = dbContext.Products.ToList();

            return View(Product);
        }
        
        public ActionResult Details(int id)
        {
            var Product = dbContext.Products.Find(id);
            if (Product== null)
            {
                return NotFound();
            }
            return View(Product);
        }
        public ActionResult Edit(int id)
        {
            var product = dbContext.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product updatedProduct)
        {
            if (id != updatedProduct.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var existingProduct = dbContext.Products.Find(id);
                if (existingProduct == null)
                {
                    return NotFound();
                }

                existingProduct.Name = updatedProduct.Name;
                existingProduct.Description = updatedProduct.Description;
                existingProduct.Price = updatedProduct.Price;

                dbContext.SaveChanges(); 

                return RedirectToAction(nameof(Index));
            }

            return View(updatedProduct);
        }
        public ActionResult Delete(int Id)
        {
            var product= dbContext.Products.Find(Id);
            if (product == null)
            {
                return NotFound();
            }

            dbContext.Products.Remove(product);
            dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
