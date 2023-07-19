
using Microsoft.EntityFrameworkCore;
using thomasgregcorewebapi.Models;

namespace thomasgregcorewebapi.Repositories
{
    public class CustomerAddressesRepository : GenericRepository<CustomerAddress>, ICustomerAddressesRepository
    {
        private AppDbContext context = null;

        public CustomerAddressesRepository(AppDbContext repositoryContext)
             : base(repositoryContext)
        {
            context = repositoryContext;
        }
        public async Task<IEnumerable<CustomerAddress>> GetAllAddresses()
        {
            return await context.CustomerAddresses.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<CustomerAddress>> GetCustomerAddresses(int customerId)
        {
            return await context.CustomerAddresses.Where(address => address.CustomerId == customerId).AsNoTracking().ToListAsync();
        }
    }
}