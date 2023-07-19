
using Microsoft.EntityFrameworkCore;
using thomasgregcorewebapi.Models;

namespace thomasgregcorewebapi.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private AppDbContext context = null;

        public CustomerRepository(AppDbContext repositoryContext)
             : base(repositoryContext)
        {
            context = repositoryContext;
        }
        public async Task<IEnumerable<Customer>> CustomerExists(Customer customer)
        {
            return await context.Customers
                .Where(dbcustomer => dbcustomer.Id != customer.Id && dbcustomer.Email == customer.Email)
                .AsNoTracking().ToListAsync();
        }
    }
}