using ASPNETCoreDbFirst.DbModels;
using ASPNETCoreDbFirst.Models;
using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Session;
using NuGet.Versioning;
using static NuGet.Packaging.PackagingConstants;



namespace ASPNETCoreDbFirst.Controllers
{
    public class OrderTabController : Controller
    {
        private readonly R2hErpDbContext Context;

        public OrderTabController(R2hErpDbContext context)
        {
            Context = context;
        }
        [HttpGet]
        public IActionResult List()
        {

            var show = Context.OrderTabs.Where(p => !p.IsDeleted.Value == true).Include(o => o.Customer).Include(o => o.Status).ToList();

            return View("List", show);
        }

        // GET: OrderTabController/Details/5
        public ActionResult Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orderTab = Context.OrderTabs.FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderTab == null)
            {
                return NotFound();
            }

            return View(orderTab);
        }

        // GET: OrderTabController/Create
        public ActionResult Create()
        {
            var result = Context.Customers.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
            var response = Context.Products.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
            var find = Context.StatusTabs.ToList();

            ViewBag.CustomerId = new SelectList(result, "CustomerId", "Name");
            ViewBag.ProductId = new SelectList(response, "ProductId", "Name");
            ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
            return View();
        }

        // POST: OrderTabController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(OrderTabVM ordertabvm)
        {
            if (ModelState.IsValid)
            {
                var result = Context.Customers.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
                var response = Context.Products.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
                var find = Context.StatusTabs.ToList();
                var order = new OrderTab();
                {
                    order.OrderNumber = ordertabvm.OrderNumber;
                    order.CustomerId = ordertabvm.CustomerId;
                    order.OrderDate = DateTime.Now;
                    order.SubTotal = ordertabvm.SubTotal;
                    order.Discount = ordertabvm.Discount;
                    order.ShippingFee = ordertabvm.ShippingFee;
                    order.NetTotal = ordertabvm.NetTotal;
                    order.StatusId = ordertabvm.StatusId;
                    Context.OrderTabs.Add(order);
                    Context.SaveChanges();

                };
                var items = JsonConvert.DeserializeObject<List<OrderItem>>(HttpContext.Session.GetString("order"));

                if (items != null)
                {
                    foreach (var item in items)
                    {
                        item.OrderId = order.OrderId;
                        Context.OrderItems.Add(item);
                    }
                    Context.SaveChanges();
                }

                ViewBag.CustomerId = new SelectList(result, "CustomerId", "Name");
                ViewBag.ProductId = new SelectList(response, "ProductId", "Name");
                ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
            }
            return RedirectToAction(nameof(List));

        }

        [HttpGet]
        public JsonResult GetItems()
        {
            var itemsJson = HttpContext.Session.GetString("order");
            if (itemsJson != null)
            {
                var items = JsonConvert.DeserializeObject<OrderTabVM>(itemsJson);
                return Json(items);
            }
            return Json(new List<OrderTabVM>());
        }
        [HttpPost]
        public JsonResult AddItem(OrderTabVM newItem)
        {

            var itemsJson = HttpContext.Session.GetString("order");
            var items = itemsJson != null ? JsonConvert.DeserializeObject<List<OrderTabVM>>(itemsJson) : new List<OrderTabVM>();

            items.Add(newItem);

            HttpContext.Session.SetString("order", JsonConvert.SerializeObject(items));

            return Json(items);



        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var result = Context.Customers.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
            var response = Context.Products.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
            var find = Context.StatusTabs.ToList();



            OrderTabVM order = new OrderTabVM();

            var execute = await Context.OrderTabs.FindAsync(id);
            if (execute == null)
            {
                return View(List);
            }

            order.OrderNumber = execute.OrderNumber;
            order.CustomerId = execute.CustomerId;
            order.OrderDate = execute.OrderDate;
            order.SubTotal = execute.SubTotal;
            order.Discount = execute.Discount;
            order.ShippingFee = execute.ShippingFee;
            order.NetTotal = execute.NetTotal;
            order.StatusId = execute.StatusId;

            ViewBag.CustomerId = new SelectList(result, "CustomerId", "Name");
            ViewBag.ProductId = new SelectList(response, "ProductId", "Name");
            ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
            var itemsJson = HttpContext.Session.GetString("order");
            var items = itemsJson != null ? JsonConvert.DeserializeObject<List<OrderTabVM>>(itemsJson) : new List<OrderTabVM>();
            return View("Create", order);
        }
        //public async Task<IActionResult> Edit(int id)
        //{
        //    var result = Context.Customers.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
        //    var respose = Context.Products.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
        //    var find = Context.StatusTabs.ToList();
        //    OrderTabVM orderTVM = new OrderTabVM();
        //    if (id == null)
        //    {
        //        return View(List);
        //    }
        //    var order = await Context.OrderTabs.FindAsync(id);
        //    //var orderitem = await _context.OrderItemTabs.FindAsync(id);
        //    var orderitem = await Context.OrderItems.Where(x => x.OrderId == id).ToListAsync();
        //    if (order == null)
        //    {
        //        return View(List);
        //    }
        //    orderTVM.OrderNumber = order.OrderNumber;
        //    orderTVM.CustomerId = order.CustomerId;
        //    orderTVM.OrderDate = order.OrderDate;
        //    orderTVM.SubTotal = order.SubTotal;
        //    orderTVM.Discount = order.Discount;
        //    orderTVM.ShippingFee = order.ShippingFee;
        //    orderTVM.NetTotal = order.NetTotal;
        //    orderTVM.StatusId = order.StatusId;

        //    foreach (var item in orderitem)
        //    {

        //        orderTVM.ProductId = item.ProductId;
        //        orderTVM.Quantity = item.Quantity;
        //        orderTVM.UnitPrice = item.UnitPrice;
        //        orderTVM.TotalAmount = item.TotalAmount;


        //    }

        //    ViewBag.SelectedProductId = orderTVM.ProductId;


        //    ViewBag.CustomersId = new SelectList(result, "CustomerId", "Name");
        //    ViewBag.ProductId = new SelectList(respose, "ProductId", "Name");
        //    ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
        //    var itemsJson = HttpContext.Session.GetString("order");
        //    var items = itemsJson != null ? JsonConvert.DeserializeObject<List<OrderTabVM>>(itemsJson) : new List<OrderTabVM>();

        //    return View("List", orderTVM);

        //}
        [HttpPost]
        public async Task<IActionResult> Edit(OrderTabVM orderTab)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the original order from session
                    var originalOrder = HttpContext.Session.GetObject<OrderTab>("Create");

                    if (originalOrder != null)
                    {
                        originalOrder.CustomerId = orderTab.CustomerId;
                        originalOrder.OrderNumber = orderTab.OrderNumber;
                        originalOrder.OrderDate = orderTab.OrderDate;
                        originalOrder.NetTotal = orderTab.NetTotal;
                        originalOrder.StatusId = orderTab.StatusId;

                        Context.Update(originalOrder);
                        await Context.SaveChangesAsync();

                        // Clear session after saving
                        HttpContext.Session.Remove("Create");
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch
                {
                    throw;

                }
                return RedirectToAction(nameof(List));
            }
            return View(orderTab);
        }
        //[HttpDelete]
        //public JsonResult Remove(OrderTabVM id)
        //{
        //    var itemsJson = HttpContext.Session.GetString("order");
        //    var items = itemsJson != null ? JsonConvert.DeserializeObject<List<OrderTabVM>>(itemsJson) : new List<OrderTabVM>();


        //    items.Remove(id);

        //    HttpContext.Session.SetString("order", JsonConvert.SerializeObject(items));

        //    return Json(items);
        //}

        public JsonResult GetProductByUnitPrice(int productId)
        {
            var result = (Context.Products.Where(option => option.ProductId == productId));
            return Json(result);
        }


        [HttpGet]
        public IActionResult Delete(int id)
        {
            try
            {
                if (id != null)
                {
                    var order = Context.OrderTabs.Find(id);
                    if (order != null)
                    {
                        order.IsDeleted = true;
                        Context.SaveChanges();

                    }
                }
                return RedirectToAction(nameof(List));
            }
            catch (Exception ex)
            {
                return Json(ex);
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
