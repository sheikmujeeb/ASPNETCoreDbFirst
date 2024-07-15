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
            var result = Context.Customers.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
            var response = Context.Products.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
            var find = Context.StatusTabs.ToList();
            ViewBag.CustomersId = new SelectList(result, "CustomersId", "Name");
            ViewBag.ProductId = new SelectList(response, "ProductsId", "Name");
            ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
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
            var result = Context.Customers.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
            var response = Context.Products.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
            var find = Context.StatusTabs.ToList();
            ViewBag.CustomersId = new SelectList(result, "CustomersId", "Name");
            ViewBag.ProductId = new SelectList(response, "ProductsId", "Name");
            ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
            return View("Create");
        }

        // POST: OrderTabController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(OrderTabVM oredertabvm)
        {
            var result = Context.Customers.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
            var response = Context.Products.ToList().Where(p => !p.IsDeleted == true).Where(o => !o.IsActive == false);
            var find = Context.StatusTabs.ToList();
            OrderTab order = new OrderTab();

            if (oredertabvm != null)
            {
                order.OrderNumber = oredertabvm.OrderNumber;
                order.CustomerId = oredertabvm.CustomerId;
                order.OrderDate = oredertabvm.OrderDate;
                order.SubTotal = oredertabvm.SubTotal;
                order.Discount = oredertabvm.Discount;
                order.ShippingFee = oredertabvm.ShippingFee;
                order.NetTotal = oredertabvm.NetTotal;
                order.StatusId = oredertabvm.StatusId;
                Context.Add(order);
                Context.SaveChangesAsync();
                return RedirectToAction(nameof(Create));

            }
            ViewBag.CustomersId = new SelectList(result, "CustomersId", "Name");
            ViewBag.ProductId = new SelectList(response, "ProductsId", "Name");
            ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
            return View(oredertabvm);
        }
        //public async Task<IActionResult> AddProduct(OrderTabVM vm)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            if (vm.OrderId == 1)
        //            {
        //                Product product = new Product();
        //                using (var httpClient = new HttpClient())
        //                {
        //                    using (var response = await httpClient.GetAsync(_apiSettings.Value.BaseUrl + "Product/" + vm.ProductId))
        //                    {
        //                        string apiResponse = await response.Content.ReadAsStringAsync();
        //                        product = JsonConvert.DeserializeObject<Product>(apiResponse);
        //                    }
        //                }
        //                if (product == null || product.ProductId == 0)
        //                {
        //                    ModelState.AddModelError("ProductName", "Please add valid Product!");
        //                    return Json(ModelState);
        //                }
        //                OrderedProduct op = new OrderedProduct();
        //                op.ProductId = product.ProductId; op.ProductCode = product.ProductCode;
        //                op.ProductDescription = product.ProductDescription;
        //                op.UoM = vm.UoM; op.UnitPrice = vm.UnitPrice;
        //                op.Quantity = vm.Quantity; op.IsActive = true;
        //                op.TotalAmount = Math.Round((decimal)(vm.UnitPrice * op.Quantity), 2);
        //                List<OrderedProduct> orderlist = new List<OrderedProduct>();
        //                if (HttpContext.Session.GetString("POI") == null) orderlist.Add(op);
        //                else
        //                {
        //                    orderlist = JsonConvert.DeserializeObject<List<OrderedProduct>>(HttpContext.Session.GetString("POI"));
        //                    orderlist.Add(op);
        //                }
        //                var serializedRecords = JsonConvert.SerializeObject(orderlist);
        //                HttpContext.Session.SetString("POI", serializedRecords);
        //                return Json(orderlist.Where(x => x.IsActive).ToList());
        //            }
        //            else if (vm.Flag == 2)
        //            {
        //                var orderlist = JsonConvert.DeserializeObject<List<OrderedProduct>>(HttpContext.Session.GetString("POI"));
        //                foreach (var oi in orderlist)
        //                {
        //                    if (oi.ProductId == vm.ProductId) oi.IsActive = false;
        //                }
        //                var serializedRecords = JsonConvert.SerializeObject(orderlist);
        //                HttpContext.Session.SetString("POI", serializedRecords);
        //                orderlist = orderlist.Where(x => x.IsActive).ToList();
        //                return Json(orderlist);
        //            }
        //            else
        //            {
        //                List<OrderedProduct> list = new List<OrderedProduct>();
        //                using (var httpClient = new HttpClient())
        //                {
        //                    using (var response = await httpClient.GetAsync(_apiSettings.Value.BaseUrl + "PurchaseOrderItem/GetPurchaseOrderItems?poId=" + vm.PoId))
        //                    {
        //                        string apiResponse = await response.Content.ReadAsStringAsync();
        //                        list = JsonConvert.DeserializeObject<List<OrderedProduct>>(apiResponse);
        //                    }
        //                }
        //                var serializedRecords = JsonConvert.SerializeObject(list);
        //                HttpContext.Session.SetString("POI", serializedRecords);
        //                return Json(list);
        //            }
        //        }
        //        return Json(ModelState);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json("Error: " + ex.Message);
        //    }
        //}

        public JsonResult getProductByUnitPrice(int ProductId)
        {
            var details = (Context.Products.Where(option => option.ProductId == ProductId));
            return Json(details);
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