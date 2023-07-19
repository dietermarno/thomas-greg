
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

        public async Task<IEnumerable<CustomerAddress>> GetCustomerAddresses(int id)
        {
            return await context.CustomerAddresses.Where(address => address.CustomerId == id).AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<Customer>> GetCustomersAddresses(List<Customer> customers)
        {
            foreach (Customer customer in customers)
            {
                customer.CustomerAddresses = await context.CustomerAddresses
                    .Where(address => address.CustomerId == customer.Id).AsNoTracking().ToListAsync();
            };
            return customers;
        }
    }
}