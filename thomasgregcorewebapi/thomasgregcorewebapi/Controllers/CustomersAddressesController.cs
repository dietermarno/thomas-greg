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
    public class CustomersAddressesController : Controller
    {
        private readonly ICustomerAddressesRepository repository;
        public CustomersAddressesController(ICustomerAddressesRepository _context)
        {
            repository = _context;
        }

        [HttpGet("GetAddresses")]
        public async Task<ActionResult<IEnumerable<CustomerAddress>>> GetAllAddresses()
        {
            var customersAddresses = await repository.GetAll();
            if (customersAddresses == null)
            {
                return BadRequest();
            }
            return Ok(customersAddresses.ToList());
        }

        [HttpGet("GetAddresses/{customerId}")]
        public async Task<ActionResult<IEnumerable<CustomerAddress>>> GetCustomerAddresses(int customerId)
        {
            var customersAddresses = await repository.GetCustomerAddresses(customerId);
            if (customersAddresses == null)
            {
                return NotFound($"No addresses found for customer Id {customerId}");
            }
            return Ok(customersAddresses.ToList());
        }

        [HttpGet("GetAddress/{id}")]
        public async Task<ActionResult<Customer>> GetCustomerAddress(int id)
        {
            var customerAddress = await repository.GetById(id);
            if (customerAddress == null)
            {
                return NotFound($"Customer Address Id {id} not found");
            }
            return Ok(customerAddress);
        }

        // POST api/<controller>  
        [HttpPost]
        public async Task<IActionResult> PostCustomerAddress([FromBody] CustomerAddress customerAddress)
        {
            if (customerAddress == null)
            {
                return BadRequest("Customer address is null");
            }
            await repository.Insert(customerAddress);
            return CreatedAtAction(nameof(GetCustomerAddress), new { Id = customerAddress.Id }, customerAddress);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerAddress(int id, CustomerAddress customerAddress)
        {
            if (id != customerAddress.Id)
            {
                return BadRequest($"Customer Address Id {id} is invalid");
            }
            try
            {
                await repository.Update(id, customerAddress);
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
            return Ok($"Customer address Id {id} update succefully");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CustomerAddress>> DeleteCustomer(int id)
        {
            var customerAddress = await repository.GetById(id);
            if (customerAddress == null)
            {
                return NotFound($"Customer address Id {id} not found");
            }
            await repository.Delete(id);
            return Ok(customerAddress);
        }
    }
}
