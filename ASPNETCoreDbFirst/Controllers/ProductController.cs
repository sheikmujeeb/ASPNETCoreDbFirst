using ASPNETCoreDbFirst.DbModels;
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
        public ActionResult List()
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
        public async Task<IActionResult> Create(Product collection)
        {
            try
            {
                collection.CreatedOn = DateTime.Now;
                await Context.Products.AddAsync(collection);
                Context.SaveChanges();
                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View();
            }
        }

        // GET: ProductController/Edit/5
        public ActionResult Edit(int id)
        {
            var search = Context.Products.Find(id);
            return View("Edit",search);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product collection)
        {
            try
            {
                var existingproduct = Context.Products.Find(collection.ProductId);

                if (existingproduct != null)
                {
                    existingproduct.Name = collection.Name;
                    existingproduct.Code = collection.Code;
                    existingproduct.IsActive = collection.IsActive;
                    existingproduct.UpdatedOn = DateTime.Now;
                    Context.SaveChanges();
                }
                return RedirectToAction(nameof(List));

            }
            catch
            {
                return View();
            }
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
