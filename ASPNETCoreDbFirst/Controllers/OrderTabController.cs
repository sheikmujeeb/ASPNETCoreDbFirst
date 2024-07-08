using ASPNETCoreDbFirst.DbModels;
using ASPNETCoreDbFirst.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreDbFirst.Controllers
{
    public class OrderTabController : Controller
    {
        public readonly R2hErpDbContext Context;
        public OrderTabController(R2hErpDbContext context)
        {
            Context = context;
        }
        // GET: OrderTabController
        public IActionResult List()
        {
            var Show = Context.OrderTabs.Include(o => o.Customer).ToList();
            return View("List",Show);
        }

        // GET: OrderTabController/Details/5
        public ActionResult Details(int id)
        {
            var find = Context.OrderTabs.Find(id);
            return View("Details",find);
        }

        // GET: OrderTabController/Create
        public ActionResult Create()
        {
            var result = Context.Customers.ToList().Where(p => !p.IsDeleted.Value == true).Where(o => !o.IsActive == false);
            ViewBag.CustomerId = new SelectList(result, "CustomerId", "Name");
            return View();
        }

        // POST: OrderTabController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderTabVM ordertabvm)
        {
            OrderTab order=new OrderTab();
            if(order!=null)
            {
                order.OrderNumber = ordertabvm.OrderNumber;
                order.OrderId = ordertabvm.OrderId;
                order.OrderDate =DateTime.Now;
                order.ShippingFee = ordertabvm.ShippingFee;
                order.CustomerId = ordertabvm.CustomerId;
                order.SubTotal= ordertabvm.SubTotal;
                order.Discount = ordertabvm.Discount;
                order.NetTotal = ordertabvm.NetTotal;
                order.StatusId = ordertabvm.StatusId;
                Context.Add(order);
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }

            return View (ordertabvm);
           
        }

        // GET: OrderTabController/Edit/5
        public IActionResult Edit(int id)
        {
            var result= Context.OrderTabs.Find(id);
            OrderTabVM order=new OrderTabVM();
            if(result!=null)
            {
                order.OrderNumber = result.OrderNumber;
                order.OrderId = result.OrderId;
                order.OrderDate =result.OrderDate;
                order.ShippingFee = result.ShippingFee;
                order.CustomerId = result.CustomerId;
                order.SubTotal = result.SubTotal;
                order.Discount = result.Discount;
                order.NetTotal = result.NetTotal;
                order.StatusId = result.StatusId;

                var find = Context.Customers.ToList();
                ViewBag.CustomerId = new SelectList(find, "CustomerId", "Name");
                return View(order);
            }
            return View();
        }

        // POST: OrderTabController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Edit(int id, OrderTabVM ordertabvm)
        {
            var find = Context.OrderTabs.FindAsync(id);
            OrderTabVM order=new OrderTabVM();
            if(find!=null)
            {
                order.OrderNumber = ordertabvm.OrderNumber;
                order.OrderId = ordertabvm.OrderId;
                order.OrderDate = ordertabvm.OrderDate;
                order.ShippingFee = ordertabvm.ShippingFee;
                order.SubTotal = ordertabvm.SubTotal;
                order.Discount = ordertabvm.Discount;
                order.NetTotal = ordertabvm.NetTotal;
                order.StatusId = ordertabvm.StatusId;
                Context.Update(order);
                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(ordertabvm);
        }

        // GET: OrderTabController/Delete/5
        public ActionResult Delete(int id)
        {
            var find= Context.OrderTabs.Find(id);
            return View("Delete",find);
        }

        // POST: OrderTabController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, OrderTab order)
        {
            var find= Context.OrderTabs.Find(order.CustomerId);
            if (find != null)
            {
                Context.OrderTabs.Remove(find);
                Context.OrderTabs.Update(order);
                await Context.SaveChangesAsync();
                
            }
            return RedirectToAction(nameof(List));
        }
    }
}
