using thomasgregcorewebapi.Models;

namespace thomasgregcorewebapi.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        Task<IEnumerable<Customer>> CustomerExists(Customer customer);
    }
}