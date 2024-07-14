using ASPNETCoreDbFirst.DbModels;
using ASPNETCoreDbFirst.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Newtonsoft.Json;
using System.Drawing;
using System.Text;
using System.Text.RegularExpressions;

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
            try
            {
                if (ModelState.IsValid)
                {
                    var orderlist = JsonConvert.DeserializeObject<List<OrderItem>>(HttpContext.Session.GetString("Key"));
                    if (orderlist == null || orderlist.Count == 0)
                    {
                        ModelState.AddModelError("", "Kindly Add Product");
                        //TempData["Alert_MsgCode"] = "6";
                        //goto viewpage;
                    }
                    if (orderTab.OrderItemId > 0)
                    {
                        OrderItem item = new OrderItem();
                        using (var httpClient = new HttpClient())
                        {
                            using (var response = await httpClient.GetAsync(_apiUrl + orderTab.OrderItemId))
                            {
                                string apiResponse = await response.Context.ReadAsStringAsync();
                                item = JsonConvert.DeserializeObject<OrderItem>(apiResponse);

                            }
                        }
                        if (item == null)
                        {
                            return NotFound();
                        }
                        using (var httpClient = new HttpClient())
                        {
                            StringContent content = new StringContent(JsonConvert.SerializeObject(orderTab), encoding.UTF3, "application/json");
                            using (var response = await httpClient.PutAsync(_apiUrl + orderTab.OrderItemId, content))
                            {
                                string apiResponse = await response.Context.ReadAsStringAsync();
                                orderTab = JsonConvert.DeserializeObject<OrderItem>(apiResponse);
                            }
                        }
                    }
                    else
                    {
                        using (var httpClient = new HttpClient())
                        {
                            StringContent content = new StringContent(JsonConvert.SerializeObject(orderTab), encoding.UTF3, "application/json");
                            using (var response = await httpClient.PostAsync(_apiUrl, content))
                            {
                                string apiResponse = await response.Context.ReadAsStringAsync();
                                orderTab = JsonConvert.DeserializeObject<OrderItem>(apiResponse);
                            }
                        }

                    }
                }
                return View();
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