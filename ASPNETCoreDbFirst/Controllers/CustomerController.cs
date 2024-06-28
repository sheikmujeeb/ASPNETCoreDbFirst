using ASPNETCoreDbFirst.DbModels;
using ASPNETCoreDbFirst.Models;
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
        public async Task <IActionResult> List()
        {
           var show= Context.Customers.Where(p=>!p.IsDeleted.Value==true).ToList();
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
        public async Task<IActionResult> Create(CustomerVM customervm)
        {
            if(ModelState.IsValid)
            {
                Customer customer=new Customer();
                customer.Name = customervm.Name;
                customer.Email = customervm.Email;
                customer.PhoneNumber = customervm.PhoneNumber;
                customer.IsActive = customervm.IsActive;
                customer.CreatedOn = DateTime.Now;
                customer.UpdatedOn = null;
                customer.IsDeleted = false;
                Context.Add(customer);

                await Context.SaveChangesAsync();
                return RedirectToAction(nameof(List));
            }
            return View(customervm);
        }

        // GET: CustomerController/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var show = await Context.Customers.FindAsync(id);
            CustomerVM customervm = new CustomerVM();
            if (show != null)
            {
                customervm.CustomerId = show.CustomerId;
                customervm.Name = show.Name;
                customervm.Email = show.Email;
                customervm.PhoneNumber = show.PhoneNumber;
                customervm.IsActive = show.IsActive;
                return View(customervm);
            }
            return View();
        }

        // POST: CustomerController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerVM customervm)
        {
            if (ModelState.IsValid)
            {
                    Customer existingCustomer = Context.Customers.Find(customervm.CustomerId);
                
                    existingCustomer.Name = customervm.Name;
                    existingCustomer.PhoneNumber = customervm.PhoneNumber;
                    existingCustomer.Email = customervm.Email;
                    existingCustomer.IsActive = customervm.IsActive;
                    existingCustomer.UpdatedOn = DateTime.Now;
                    Context.Update(existingCustomer);
                    await Context.SaveChangesAsync();
                    return RedirectToAction(nameof(List));
            }
            return View(customervm);
        }

        // GET: CustomerController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var show = await Context.Customers.FindAsync(id);
            return View("Delete",show);
        }

        // POST: CustomerController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(Customer customer)
        {
            var Customer=await Context.Customers.FindAsync(customer.CustomerId);
            if (Customer != null)
            {
                Customer.IsDeleted = true;
                Context.Customers.Update(Customer);
                Context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(List));
        }
    }
}
