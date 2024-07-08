using ASPNETCoreDbFirst.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETCoreDbFirst.Controllers
{
    public class OrderItemController : Controller
    {
        public readonly R2hErpDbContext Context;
        public OrderItemController(R2hErpDbContext context)
        {
            Context = context;
        }
        // GET: OrderItemController
        public IActionResult List()
        {
            var find=Context.OrderItems.ToList();
            return View("List",find);
        }

        // GET: OrderItemController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderItemController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrderItemController/Create
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

        // GET: OrderItemController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderItemController/Edit/5
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

        // GET: OrderItemController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderItemController/Delete/5
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
