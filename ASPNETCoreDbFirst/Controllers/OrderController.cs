using ASPNETCoreDbFirst.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ASPNETCoreDbFirst.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.Rendering;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.CodeAnalysis;

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
            var show = Context.Orders.Where(p => !p.IsDeleted.Value == true).Include(o => o.Customer).Include(o => o.Product).ToList();

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
            var result= Context.Customers.ToList().Where(p => !p.IsDeleted).Where(o => !o.IsActive == false);
            var search=Context.Products.ToList().Where(p => !p.IsDeleted).Where(o => !o.IsActive == false);
            ViewBag.CustomerId = new SelectList(result, "CustomerId", "Name");
            ViewBag.ProductId = new SelectList(search, "ProductId", "Name");

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
               
                
                order.CustomerId = ordervm.CustomerId;
                order.ProductId = ordervm.ProductId;
                order.OrderDate = DateTime.Now;
                order.Quantity = ordervm.Quantity;
                order.CreatedOn = DateTime.Now;
                order.Amount = ordervm.Amount;
                order.UpdatedOn = null;
                order.IsDeleted = false;
                order.TotalAmount = order.Quantity * order.Amount;
                Context.Add(order);

                Context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            
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
                ordervm.TotalAmount= find.TotalAmount;
                ordervm.OrderDate = find.OrderDate;

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

            if (id!=null)
            {
                Order existingorder = Context.Orders.Find(id);
                existingorder.Quantity =ordervm.Quantity;
                existingorder.Amount = ordervm.Amount;
                existingorder.UpdatedOn = DateTime.Now;
                existingorder.OrderDate=ordervm.OrderDate;
              

                existingorder.TotalAmount = ordervm.Quantity * ordervm.Amount;
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
        public async Task <IActionResult> Delete(int id,Order collection)
        {
            var record = await Context.Orders.FindAsync(collection.OrderId);
            if (record != null)
            {
                bool productExistsInOrder = Context.Orders.Any(o => o.ProductId == id);
                if (productExistsInOrder == false)
                {
                    record.IsDeleted = true;
                    Context.Orders.Update(record);
                    Context.SaveChanges();

                }
               
            }
            await Context.SaveChangesAsync();
            return RedirectToAction(nameof(List));
        }
    }
}
