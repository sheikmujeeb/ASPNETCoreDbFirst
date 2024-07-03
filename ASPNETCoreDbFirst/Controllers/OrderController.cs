using ASPNETCoreDbFirst.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASPNETCoreDbFirst.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Rendering;
using Humanizer;
using Microsoft.EntityFrameworkCore;

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
            var show = Context.Orders.Where(p => !p.IsDeleted.Value == true).ToList();

            return View("List",show);
        }

        // GET: OrderController/Details/5
        public ActionResult Details(int id)
        {
            var show= Context.Orders.Find(id);
            return View("Details",show);
        }

        // GET: OrderController/Create
        public IActionResult Create()
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
            Order order = new Order();
            if (order!=null)
            {
               
                order.OrderId = ordervm.OrderId;
                order.CustomerId = ordervm.CustomerId;
                order.ProductId = ordervm.ProductId;
                order.OrderDate = DateTime.Now;
                order.Quantity = ordervm.Quantity;
                order.CreatedOn = DateTime.Now;
                order.Amount = ordervm.Amount;
                order.IsActive= ordervm.IsActive;
                order.UpdatedOn = null;
                order.IsDeleted = false;
                order.TotalAmount = order.Quantity * order.Amount;
                Context.Add(order);

                Context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            //ViewData["CustomerId"] = new SelectList(Context.Customers, "CustomerId", "CustomerId", order.CustomerId);
            //ViewData["ProductId"] = new SelectList(Context.Products, "ProductId", "ProductId", order.ProductId);
            return View(ordervm);
            
        }

        // GET: OrderController/Edit/5
        public IActionResult Edit(int id)
        {
            var find = Context.Orders.Find(id);
            OrderVM ordervm = new OrderVM();
            if (find != null)
            {
               ordervm.ProductId = find.ProductId;
                ordervm.CustomerId = find.CustomerId;
                ordervm.Quantity = find.Quantity;
                ordervm.Amount = find.Amount;
                ordervm.IsActive = find.IsActive;

                var result = Context.Customers.ToList();
                var search = Context.Products.ToList();
                ViewBag.CustomerId = new SelectList(result, "CustomerId", "Name");
                ViewBag.ProductId = new SelectList(search, "ProductId", "Name");
                return View(ordervm);
            }
            return View();
        }

        // POST: OrderController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,OrderVM ordervm)
        {

            if (ModelState.IsValid)
            {
                Order existingorder = Context.Orders.Find(id);
                if (existingorder == null)
                {

                }
                existingorder.Quantity =ordervm.Quantity;
                existingorder.Amount = ordervm.Amount;
                existingorder.IsActive = ordervm.IsActive;
                existingorder.UpdatedOn = DateTime.Now;
                existingorder.TotalAmount=ordervm.Quantity*ordervm.Amount;
                Context.Update(existingorder);
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(ordervm);
        }

        // GET: OrderController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var show = await Context.Orders.FindAsync(id);
            return View("Delete", show);
        }

        // POST: OrderController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Delete(Order collection)
        {
            var record = await Context.Orders.FindAsync(collection.OrderId);
            if (record != null)
            {
                record.IsDeleted = true;
                Context.Orders.Update(record);
                Context.SaveChanges();
            }
            await Context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }
    }
}
