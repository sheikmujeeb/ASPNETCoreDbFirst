using ASPNETCoreDbFirst.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASPNETCoreDbFirst.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASPNETCoreDbFirst.Controllers
{
    public class OrderController : Controller
    {
        public readonly R2hErpDbContext Context;
        public OrderController(R2hErpDbContext context)
        {
            Context = context;
        }
        // GET: OrderController
        public IActionResult List()
        {
            var show=Context.Orders.ToList();
            return View("List",show);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: OrderController/Create
        public async Task <IActionResult> Create()
        {
            var result= Context.Customers.ToList();
            var search=Context.Products.ToList();
            ViewBag.CustomerId = new SelectList(result, "CustomerId","Name");
            ViewBag.ProductId = new SelectList(search, "ProductId","Name");
            return View();
        }

        // POST: OrderController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(OrderVM ordervm)
        {
            if(ModelState.IsValid)
            {
                Order order = new Order();
                order.OrderDate = DateTime.Now;
                order.Quantity = ordervm.Quantity;
                order.CreatedOn = DateTime.Now;
                order.Amount = ordervm.Amount;
                Context.Add(order);

                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(ordervm);
            
        }

        // GET: OrderController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OrderController/Edit/5
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

        // GET: OrderController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: OrderController/Delete/5
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
