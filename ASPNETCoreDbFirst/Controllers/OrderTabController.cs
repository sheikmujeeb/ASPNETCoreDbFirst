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
        private int _apiUrl;

        public OrderTabController(R2hErpDbContext context)
        {
            Context = context;
        }
        // GET: OrderTabController
        public IActionResult List()
        {
            return View();
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
        public async Task<IActionResult> Create([FromBody] OrderTabVM orderTab)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (orderTab.Flag== 1)
                    {
                        OrderTab order = new OrderTab();
                        using (var httpClient = new HttpClient())
                        {
                            using (var response = await httpClient.GetAsync(_apiSettings.Value.BaseUrl + "Order/" + orderTab.OrderId))
                            {
                                string apiResponse = await response.Content.ReadAsStringAsync();
                                order = JsonConvert.DeserializeObject<OrderTab>(apiResponse);
                            }
                        }
                        if (order == null || order.OrderId == 0)
                        {
                            ModelState.AddModelError("OrderName", "Please add valid Product!");
                            return Json(ModelState);
                        }
                        OrderTab tab = new OrderTab();
                        tab.OrderNumber = order.OrderNumber;
                        tab.CustomerId = order.CustomerId;
                        tab.OrderDate = order.OrderDate;
                        tab.SubTotal = order.SubTotal;
                        tab.Discount = order.Discount;
                        tab.ShippingFee = order.ShippingFee; ;
                        tab.NetTotal = Math.Round((decimal)(order.SubTotal - order.Discount + order.ShippingFee);
                        tab.StatusId = order.StatusId;

                        List<OrderTab> orderlist = new List<OrderTab>();
                        if (HttpContext.Session.GetString("Key") == null)
                            orderlist.Add(tab);
                        else
                        {
                            orderlist = JsonConvert.DeserializeObject<List<OrderTab>>(HttpContext.Session.GetString("Key"));
                            orderlist.Add(tab);
                        }
                        var serializedRecords = JsonConvert.SerializeObject(orderlist);
                        HttpContext.Session.SetString("POI", serializedRecords);

                        return Json(orderlist.ToList());
                    }
                }
                else if (orderTab.Flag == 2)
                {
                    var orderlist = JsonConvert.DeserializeObject<List<OrderTab>>(HttpContext.Session.GetString("Key"));
                    var serializedRecords = JsonConvert.SerializeObject(orderlist);
                    HttpContext.Session.SetString("POI", serializedRecords);

                    return Json(orderlist);
                }
                else
                {
                    List<OrderTab> list = new List<OrderTab>();
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.GetAsync(_apiSettings.Value.BaseUrl + "PurchaseOrderItem/GetPurchaseOrderItems?poId=" + vm.PoId))
                        {
                            string apiResponse = await response.Content.ReadAsStringAsync();
                            list = JsonConvert.DeserializeObject<List<OrderTab>>(apiResponse);
                        }
                    }
                    var serializedRecords = JsonConvert.SerializeObject(list);
                    HttpContext.Session.SetString("Key", serializedRecords);

                    return Json(list);
                }

                return Json(ModelState);
            }
            
            catch (Exception ex)
            {
                return Json("Error: " + ex.Message);

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