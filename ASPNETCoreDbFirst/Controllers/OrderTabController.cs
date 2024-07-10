using ASPNETCoreDbFirst.DbModels;
using ASPNETCoreDbFirst.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPNETCoreDbFirst.Controllers
{
    public class OrderTabController : Controller
    {
        private readonly R2hErpDbContext Context;
            public OrderTabController(R2hErpDbContext context)
        {
            Context= context;
        }
        // GET: OrderTabController
        public IActionResult List()
        {
            var show=Context.OrderTabs.ToList();
            var result= Context.OrderItems.ToList();
            return View("List");
        }
       

        // GET: OrderTabController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderTabController/Create
        public ActionResult Create()
        {
            var result = Context.Customers.ToList().Where(p => !p.IsDeleted.Value == true).Where(o => !o.IsActive == false);
            var search = Context.Products.ToList().Where(p => !p.IsDeleted.Value == true).Where(o => !o.IsActive == false);
            ViewBag.CustomerId = new SelectList(result, "CustomerId", "Name");
            ViewBag.ProductId = new SelectList(search, "ProductId", "Name");
            return View();
        }

        // POST: OrderTabController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderTabController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderTabController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: OrderTabController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderTabController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
