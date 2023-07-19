using thomasgregcorewebapi.Models;

namespace thomasgregcorewebapi.Repositories
{
    public interface ICustomerAddressesRepository : IGenericRepository<CustomerAddress>
    {
        Task<IEnumerable<CustomerAddress>> GetAllAddresses();
        Task<IEnumerable<CustomerAddress>> GetCustomerAddresses(int customerId);
    }
}