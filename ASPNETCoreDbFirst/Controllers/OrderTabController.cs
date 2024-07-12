using ASPNETCoreDbFirst.DbModels;
using ASPNETCoreDbFirst.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ASPNETCoreDbFirst.Controllers
{
    public class OrderTabController : Controller
    {
        private readonly R2hErpDbContext Context;
       

        public OrderTabController(R2hErpDbContext context)
        {
            Context = context;
        }
        // GET: OrderTabController
        public IActionResult List()
        {
            return View("Create");
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
            var find = Context.StatusTabs.ToList();
            ViewBag.CustomerId = new SelectList(result, "CustomerId", "Name");
            ViewBag.ProductId = new SelectList(search, "ProductId", "Name");
            ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
            return View();
        }

        // POST: OrderTabController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderTabVM orderTab)
        {
            if (ModelState.IsValid)
            {
                Product pro=new Product();
                OrderTabVM order = new OrderTabVM();
                order.OrderNumber = orderTab.OrderNumber;   
                order.CustomerId = orderTab.CustomerId;
                order.OrderDate = orderTab.OrderDate;
                order.ProductId = orderTab.ProductId;
                order.Quantity = orderTab.Quantity;
                order.UnitPrice = pro.UnitPrice;
                Context.Add(order);

                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View(orderTab);
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


public static class SessionExtensions
{
    public static void SetObject(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }

    public static T GetObject<T>(this ISession session, string key)
    {
        var value = session.GetString(key);
        return value == null ? default : JsonConvert.DeserializeObject<T>(value);
    }
}