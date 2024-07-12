using ASPNETCoreDbFirst.DbModels;
using ASPNETCoreDbFirst.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;


namespace ASPNETCoreDbFirst.Controllers
{
    public class ProductController : Controller
    {
        public readonly R2hErpDbContext Context;
        public ProductController(R2hErpDbContext context)
        {
            Context = context;
        }
        // GET: ProductController
        public IActionResult List()
        {
            var show = Context.Products.Where(p => !p.IsDeleted.Value == true).ToList();
            return View("List",show);
        }

        // GET: ProductController/Details/5
        public ActionResult Details(int id)
        {
            var search=Context.Products.Find(id);
            return View("Details",search);
        }

        // GET: ProductController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductVM productvm)
        {
            if (ModelState.IsValid)
            {
                Product products = new Product();
                products.Name = productvm.Name;
                products.Code = productvm.Code;
                products.UnitPrice = productvm.UnitPrice;
                products.IsActive = productvm.IsActive;
                products.CreatedOn = DateTime.Now;
                products.UpdatedOn = null;
                products.IsDeleted = false;
                Context.Add(products);

                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(productvm);
        }

        // GET: ProductController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var show = await Context.Products.FindAsync(id);
            ProductVM productvm = new ProductVM();
            if (show != null)
            {
                productvm.Name = show.Name;
                productvm.Code = show.Code;
                productvm.IsActive = show.IsActive;
                return View(productvm);
            }
            return View();
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,ProductVM productvm)
        {

            if (ModelState.IsValid)
            {
                Product existingProduct = Context.Products.Find(id);
                if(existingProduct==null)
                {

                }
                existingProduct.Name = productvm.Name;
                existingProduct.Code= productvm.Code;
                existingProduct.IsActive = productvm.IsActive;
                existingProduct.UpdatedOn = DateTime.Now;
                Context.Update(existingProduct);
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(productvm);
        }

        // GET: ProductController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            var search =await Context.Products.FindAsync(id);
            return View("Delete",search);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id,Product product)
        {
            
            var ProductInOrder = Context.Orders.Any(o => o.ProductId == id);
            if (ProductInOrder==true)
            {
                return Content("The Product was in Order So we cannot able to delete the Product...!!");
            }
            else
            {
                var Product = await Context.Products.FindAsync(product.ProductId);
                Product.IsDeleted = true;
                Context.Products.Update(Product);
                Context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(List)); ;
              
        }
    }
}
