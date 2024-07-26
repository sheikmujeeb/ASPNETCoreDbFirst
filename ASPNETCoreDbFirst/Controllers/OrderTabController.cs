using ASPNETCoreDbFirst.DbModels;
using ASPNETCoreDbFirst.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ASPNETCoreDbFirst.Controllers
{
    public class OrderTabController : Controller
    {
        private readonly R2hErpDbContext _context;

        public OrderTabController(R2hErpDbContext context)
        {
            _context = context;
        }
        // GET: OrderTabController
        public IActionResult List()
        {
            var show = _context.OrderTabs.Include(o => o.Customer).Include(p => p.Status).Where(s => !s.IsDeleted == true).ToList();
            return View("List", show);

        }
        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.OrderTabs.Include(o => o.Customer).Include(o => o.Status).FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var orderItems = await _context.OrderItems.Where(oi => oi.OrderId == id && !oi.IsDeleted).Include(oi => oi.Product).ToListAsync();

            var orderItemsDetails = orderItems.Select(oi => new
            {
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                TotalAmount = oi.TotalAmount,
                UnitPrice = oi.UnitPrice
            }).ToList();

            var viewModel = new
            {
                OrderNumber = order.OrderNumber,
                CustomerName = order.Customer.Name,
                OrderDate = order.OrderDate,
                SubTotal = order.SubTotal,
                Discount = order.Discount,
                ShippingFee = order.ShippingFee,
                NetTotal = order.NetTotal,
                StatusName = order.Status.StatusName,
                OrderItems = orderItemsDetails
            };

            return Json(viewModel);
        }
        private List<OrderTabVM> GetOrderItemsFromSession()
        {
            var items = HttpContext.Session.GetString("Order");
            if (items != null)
            {
                return JsonConvert.DeserializeObject<List<OrderTabVM>>(items);
            }
            return new List<OrderTabVM>();
        }

        private void SaveOrderItemsToSession(List<OrderTabVM> items)
        {
            var sessionData = JsonConvert.SerializeObject(items);
            HttpContext.Session.SetString("Order", sessionData);
        }

        // GET: OrderTabController/Create

        [HttpGet]
        public async Task<ActionResult> Create(int id)
        {
            var result = _context.Customers.Where(p => !p.IsDeleted && p.IsActive).ToList();
            var response = _context.Products.Where(p => !p.IsDeleted && p.IsActive).ToList();
            var find = _context.StatusTabs.ToList();
            ViewBag.CustomerId = new SelectList(result, "CustomerId", "Name");
            ViewBag.ProductId = new SelectList(response, "ProductId", "Name");
            ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");


            if (id == 0)
            {
                var date = new OrderTabVM
                {
                    OrderDate = DateTime.Now
                };
                return View(date);
            }

            var order = await _context.OrderTabs.Include(o => o.Customer).Include(o => o.Status).Include(o => o.OrderItemTabs).ThenInclude(oi => oi.Product).FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }

            var orderItems = order.OrderItemTabs.Where(oi => !oi.IsDeleted).Select(oi => new OrderTabVM { ProductId = oi.ProductId, ProductName = oi.Product.Name, Quantity = oi.Quantity, UnitPrice = oi.UnitPrice, TotalAmount = oi.TotalAmount }).ToList();

            SaveOrderItemsToSession(orderItems);

            var viewModel = new OrderTabVM
            {
                OrderId = order.OrderId,
                OrderNumber = order.OrderNumber,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                SubTotal = order.SubTotal,
                Discount = order.Discount,
                ShippingFee = order.ShippingFee,
                NetTotal = order.NetTotal,
                StatusId = order.StatusId

            };

            ViewBag.OrderItems = JsonConvert.SerializeObject(orderItems);
            ViewBag.CustomerId = new SelectList(_context.Customers.Where(c => !c.IsDeleted && c.IsActive).ToList(), "CustomerId", "Name", order.CustomerId);
            ViewBag.ProductId = new SelectList(_context.Products.Where(p => !p.IsDeleted && p.IsActive).ToList(), "ProductId", "Name");
            ViewBag.StatusId = new SelectList(_context.StatusTabs.ToList(), "StatusId", "StatusName", order.StatusId);

            return View(viewModel);
        }

        // POST: OrderTabController/Create
        [HttpPost]
        public async Task<ActionResult> Create(OrderTabVM ordervm)
        {
            var customers = _context.Customers.Where(p => !p.IsDeleted && p.IsActive).ToList();
            var products = _context.Products.Where(p => !p.IsDeleted && p.IsActive).ToList();
            var statuses = _context.StatusTabs.ToList();
            ViewBag.CustomerId = new SelectList(customers, "CustomerId", "Name");
            ViewBag.ProductId = new SelectList(products, "ProductId", "Name");
            ViewBag.StatusId = new SelectList(statuses, "StatusId", "StatusName");

            if (ordervm.OrderId == 0)
            {
                if (!string.IsNullOrEmpty(ordervm.OrderNumber))
                {
                    var existingOrder = await _context.OrderTabs
                        .AnyAsync(o => o.OrderNumber == ordervm.OrderNumber && (o.IsDeleted == false || o.IsDeleted == null));

                    if (existingOrder)
                    {
                        ModelState.AddModelError("OrderNumber", "Order number already exists.");
                        return View(ordervm);
                    }
                }

                OrderTab order = new OrderTab
                {

                    OrderNumber = ordervm.OrderNumber,
                    CustomerId = ordervm.CustomerId,
                    OrderDate = DateTime.Now,
                    SubTotal = ordervm.SubTotal,
                    IsDeleted = false,
                    Discount = ordervm.Discount,
                    ShippingFee = ordervm.ShippingFee,
                    NetTotal = ordervm.NetTotal,
                    StatusId = ordervm.StatusId,
                };


                _context.OrderTabs.Add(order);
                await _context.SaveChangesAsync();

                var temp = (HttpContext.Session.GetString("Order"));
                var orderItem = JsonConvert.DeserializeObject<List<OrderItem>>(temp);
                if (orderItem != null)
                {
                    foreach (var item in orderItem)
                    {
                        item.OrderId = order.OrderId;
                        _context.OrderItems.Add(item);
                    }
                    await _context.SaveChangesAsync();
                }
                HttpContext.Session.Remove("Order");
                return RedirectToAction(nameof(List));
            }
            else
            {
                var order = await _context.OrderTabs.Include(o => o.OrderItemTabs).FirstOrDefaultAsync(o => o.OrderId == ordervm.OrderId);

                if (order == null)
                {
                    return NotFound();
                 }


                order.OrderNumber = ordervm.OrderNumber;
                order.CustomerId = ordervm.CustomerId;
                order.OrderDate = ordervm.OrderDate;
                order.SubTotal = ordervm.SubTotal;
                order.Discount = ordervm.Discount;
                order.ShippingFee = ordervm.ShippingFee;
                order.NetTotal = ordervm.NetTotal;
                order.StatusId = ordervm.StatusId;

                _context.Update(order);

                var existingOrderItems = GetOrderItemsFromSession();
                foreach (var item in existingOrderItems)
                {
                    var orderItem = order.OrderItemTabs.FirstOrDefault(oi => oi.ProductId == item.ProductId);
                    if (orderItem != null)
                    {
                        orderItem.Product.Name = item.ProductName;
                        orderItem.Quantity = item.Quantity;
                        orderItem.UnitPrice = item.UnitPrice;
                        orderItem.TotalAmount = item.TotalAmount;
                        _context.OrderItems.Update(orderItem);
                    }
                    else
                    {
                        orderItem = new OrderItem
                        {
                            OrderId = order.OrderId,
                            ProductId = item.ProductId,
                            Quantity = item.Quantity,
                            UnitPrice = item.UnitPrice,
                            TotalAmount = item.TotalAmount
                        };
                        _context.OrderItems.Add(orderItem);
                    }
                }

                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("Order");
                return RedirectToAction(nameof(List));
            }
        }

        [HttpGet]
        public JsonResult GetProductByUnitPrice(int productId)
        {

            var product = _context.Products.Where(option => option.ProductId == productId);

            return Json(product);

        }
       

        // POST: OrderTabController/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _context.OrderTabs.Include(o => o.OrderItemTabs).FirstOrDefaultAsync(o => o.OrderId == id);
            if (order == null)
            {
                return NotFound();
            }
            order.IsDeleted = true;
            foreach (var item in order.OrderItemTabs)
            {
                item.IsDeleted = true;
            }
            await _context.SaveChangesAsync();
            return View(List);
        }


        public IActionResult AddItem(OrderTabVM newItem)
        {
            var items = GetOrderItemsFromSession();

            var product = _context.Products.Find(newItem.ProductId);

            if (product != null)
            {
                newItem.ProductName = product.Name;

                // Checking if the product already exists in session or not
                var existingItem = items.FirstOrDefault(x => x.ProductId == newItem.ProductId);

                if (existingItem != null)
                {
                    existingItem.Quantity += newItem.Quantity;
                    existingItem.TotalAmount += existingItem.Quantity * existingItem.UnitPrice;

                }
                else
                {
                    newItem.TotalAmount = newItem.Quantity * newItem.UnitPrice;
                    items.Add(newItem);
                }

                SaveOrderItemsToSession(items);
            }

            return Json(items);
        }
        public IActionResult RemoveItem(int productid)
        {

            var session = GetOrderItemsFromSession();

            var items = session.FirstOrDefault(i => i.ProductId == productid);
            if (items != null)
            {
                session.Remove(items);
                SaveOrderItemsToSession(session);
            }
   
            return Json(session); 
        }

        [HttpGet]
        public async Task<JsonResult> CheckOrderNumber(string orderNumber)
        {
            if (string.IsNullOrEmpty(orderNumber))
            {
                return Json(new { exists = false });
            }

            var existingOrder = await _context.OrderTabs
                .AnyAsync(o => o.OrderNumber == orderNumber && (o.IsDeleted == false || o.IsDeleted == null));

            return Json(new { exists = existingOrder });
        }

        public IActionResult GetExistingItems(int orderId)
        {
            var orderItems = _context.OrderItems.Include(o => o.Product).Where(o => o.OrderId == orderId).ToList();
            var items = orderItems.Select(o => new OrderTabVM
            {
                ProductId = o.ProductId,
                ProductName = o.Product.Name,
                Quantity = o.Quantity,
                UnitPrice = o.UnitPrice,
                TotalAmount = o.Quantity * o.UnitPrice
            }).ToList();

            return Json(items);
        }
    }
    
}
