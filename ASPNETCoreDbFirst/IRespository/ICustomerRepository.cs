using ASPNETCoreDbFirst.DbModels;


namespace ASPNETCoreDbFirst.IRespository
{
    public interface ICustomerRepository
    {
        public IEnumerable<Customer> Showall();
    }
}
