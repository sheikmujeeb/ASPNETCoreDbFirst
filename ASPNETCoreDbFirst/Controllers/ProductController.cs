using ASPNETCoreDbFirst.DbModels;
using ASPNETCoreDbFirst.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


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
            var show=Context.Products.ToList();
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
        public async Task<IActionResult> Edit(ProductVM productvm)
        {

            if (ModelState.IsValid)
            {
                Product existingProduct = Context.Products.Find(productvm.ProductId);

                existingProduct.Name = productvm.Name;
                existingProduct.Code=productvm.Code;
                existingProduct.IsActive = productvm.IsActive;
                existingProduct.UpdatedOn = DateTime.Now;
                Context.Update(existingProduct);
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(productvm);
        }

        // GET: ProductController/Delete/5
        public ActionResult Delete(int id)
        {
            var search = Context.Products.Find(id);
            return View("Delete",search);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Product collection)
        {
            try
            {
                Context.Products.Remove(collection);
                Context.SaveChanges();
                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View();
            }
        }
    }
}
