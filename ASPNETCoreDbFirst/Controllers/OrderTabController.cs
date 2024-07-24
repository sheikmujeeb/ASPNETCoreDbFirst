using ASPNETCoreDbFirst.DbModels;
using ASPNETCoreDbFirst.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

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
            var show = _context.OrderTabs.Include(o => o.Customer).Include(p => p.Status).Where(o => !o.IsDeleted == true).ToList();
            return View("List", show);
        }

        //public async Task<IActionResult> Index()
        //{
        //    var result = _context.Customers.ToList().Where(p => !p.Isdeleted == true).Where(o => !o.IsActive == false);
        //    var response = _context.Products.ToList().Where(p => !p.Isdeleted == true).Where(o => !o.IsActive == false);
        //    var find = _context.StatusTabs.ToList();

        //    ViewBag.CustomersId = new SelectList(result, "CustomersId", "Name");
        //    ViewBag.ProductId = new SelectList(response, "ProductsId", "Name");
        //    ViewBag.StatusId = new SelectList(find, "StatusId", "StatusName");
        //    return View("Create");


        //}

        public async Task<IActionResult> Details(int id)
        {
            var order = await _context.OrderTabs.Include(o => o.Customer).Include(o => o.Status).Where(o => !o.IsDeleted == true).FirstOrDefaultAsync(o => o.OrderId == id);

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
                order.OrderNumber,
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
            var itemsJson = HttpContext.Session.GetString("order");
            return itemsJson != null ? JsonConvert.DeserializeObject<List<OrderTabVM>>(itemsJson) : new List<OrderTabVM>();
        }

        private void SaveOrderItemsToSession(List<OrderTabVM> items)
        {
            HttpContext.Session.SetString("order", JsonConvert.SerializeObject(items));
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
                OrderTab order = new OrderTab();
                order.OrderNumber = ordervm.OrderNumber;
                order.CustomerId = ordervm.CustomerId;
                order.OrderDate = ordervm.OrderDate;
                order.SubTotal = ordervm.SubTotal;
                order.Discount = ordervm.Discount;
                order.ShippingFee = ordervm.ShippingFee;
                order.NetTotal = ordervm.NetTotal;
                order.StatusId = ordervm.StatusId;

                _context.OrderTabs.Add(order);
                await _context.SaveChangesAsync();

                var orderItem = JsonConvert.DeserializeObject<List<OrderItem>>(HttpContext.Session.GetString("order"));
                if (orderItem != null)
                {
                    foreach (var item in orderItem)
                    {
                        item.OrderId = order.OrderId;
                        _context.OrderItems.Add(item);
                    }
                    await _context.SaveChangesAsync();
                }
                HttpContext.Session.Remove("order");
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
                        orderItem.Quantity = item.Quantity;
                        orderItem.UnitPrice = item.UnitPrice;
                        orderItem.TotalAmount = item.TotalAmount;
                        _context.Update(orderItem);
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
                HttpContext.Session.Remove("order");
                return RedirectToAction(nameof(List));
            }
        }

        [HttpGet]

        public async Task<IActionResult> GetProductByUnitPrice(int productId)

        {

            var product = await _context.Products.FindAsync(productId);

            if (product == null)

            {

                return NotFound();

            }

            return View(product.UnitPrice);

        }
        [HttpPost]
        public JsonResult AddItem(OrderTabVM newItem)
        {
            var items = GetOrderItemsFromSession();
            var product = _context.Products.Find(newItem.ProductId);
            if (product != null)
            {
                newItem.ProductName = product.Name;
                items.Add(newItem);
                SaveOrderItemsToSession(items);
            }
           return Json(items);
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
    }

}
