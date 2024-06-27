using ASPNETCoreDbFirst.DbModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASPNETCoreDbFirst.Controllers
{
    public class CustomerController : Controller
    {
        // GET: CustomerController
        public readonly R2hErpDbContext Context;
        public CustomerController(R2hErpDbContext context)
        {
            Context = context;
        }
        public async Task<IActionResult> List()
        {
           var show= await Context.Customers.ToListAsync();
           return View("List",show);
        }

        // GET: CustomerController/Details/5
        public ActionResult Details(int id)
        {
            var show = Context.Customers.Find(id);
            return View("Details",show);
        }

        // GET: CustomerController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer Signin)
        {
            try
            {
                Signin.CreatedOn=   DateTime.Now;
                await Context.Customers.AddAsync(Signin);
                Context.SaveChanges();
                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Edit/5
        public IActionResult Edit(int id)
        {
            var show = Context.Customers.Find(id);
            return View("Edit",show);
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Customer model)
        {
            try
            {
                var existingCustomer = Context.Customers.Find(model.CustomerId);

                if (existingCustomer != null)
                {
                    existingCustomer.Name = model.Name;
                    existingCustomer.PhoneNumber = model.PhoneNumber;
                    existingCustomer.Email = model.Email;
                    existingCustomer.IsActive = model.IsActive;
                    existingCustomer.UpdatedOn = DateTime.Now;
                    Context.SaveChanges();
                }
                return RedirectToAction(nameof(List));

            }
            catch
            {
                return View();
            }
        }

        // GET: CustomerController/Delete/5
        public ActionResult Delete(int id)
        {
            var show = Context.Customers.Find(id);
            return View("Delete",show);
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Customer collection)
        {
            try
            {
                Context.Customers.Remove(collection);
                Context.SaveChanges();
                return RedirectToAction(nameof(List));
            }
            catch
            {
                return View();
            }
        }
    }
}
