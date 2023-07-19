using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using thomasgregcorewebapi.Repositories;
using thomasgregcorewebapi.Models;
using Microsoft.AspNetCore.Authorization;

namespace thomasgregcorewebapi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : Controller
    {
        private readonly ICustomerRepository repository;
        public CustomersController(ICustomerRepository _context)
        {
            repository = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers = await repository.GetAll();
            if (customers == null)
            {
                return BadRequest();
            }
            return Ok(customers.ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id)
        {
            var customer = await repository.GetById(id);
            if (customer == null)
            {
                return NotFound($"Customer Id {id} not found");
            }
            return Ok(customer);
        }

        [HttpPost("CustomerExists")]
        public async Task<IEnumerable<Customer>> CustomerExists(Customer customer)
        {
            return await repository.CustomerExists(customer);
        }

        [HttpPost]
        public async Task<IActionResult> PostCustomer([FromBody] Customer customer)
        {
            if (customer == null)
            {
                return BadRequest("Customer is null");
            }
            await repository.Insert(customer);
            return CreatedAtAction(nameof(GetCustomer), new { Id = customer.Id }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer)
        {
            if (id != customer.Id)
            {
                return BadRequest($"Customer Id {id} is invalid");
            }
            try
            {
                await repository.Update(id, customer);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok($"Customer Id {id} succefully updated");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> DeleteCustomer(int id)
        {
            var customer = await repository.GetById(id);
            if (customer == null)
            {
                return NotFound($"Customer Id {id} not found");
            }
            await repository.Delete(id);
            return Ok(customer);
        }
    }
}
