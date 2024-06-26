using ASPNETCoreDbFirst.DbModels;
using ASPNETCoreDbFirst.IRespository;

namespace ASPNETCoreDbFirst.Repository
{
    public class CustomerRepository:ICustomerRepository
    {
        private readonly R2hErpDbContext context;
        public readonly ICustomerRepository Customers;
        public CustomerRepository(R2hErpDbContext _context,ICustomerRepository customer)
        {
            context = _context;
            Customers = customer;
        }
        public IEnumerable<Customer> Showall()
        {
            try
            {
                IEnumerable<Customer> customers = context.Customers.ToList();
                return customers;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
