using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using thomasgregcorewebapi.Models;
using thomasgregcorewebapi.Repositories;

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

        [HttpGet("GetCustomerAddresses/{id}")]
        public async Task<ActionResult<IEnumerable<CustomerAddress>>> GetCustomerAddresses(int id)
        {
            var customersAddresses = await repository.GetCustomerAddresses(id);
            if (customersAddresses == null)
            {
                return NotFound($"No addresses found for customer id {id}");
            }
            return Ok(customersAddresses.ToList());
        }

        [HttpPost("GetCustomersAddresses")]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomersAddresses(List<Customer> customers)
        {
            var customersAddresses = await repository.GetCustomersAddresses(customers);
            if (customersAddresses == null)
            {
                return NotFound($"No addresses found for customer list");
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
        public async Task<ActionResult<CustomerAddress>> DeleteCustomerAddress(int id)
        {
            var customerAddress = await repository.GetById(id);
            if (customerAddress == null)
            {
                return NotFound($"Customer address Id {id} not found");
            }
            await repository.Delete(id);
            return Ok(customerAddress);
        }

        [HttpDelete("DeleteAllAddresses/{id}")]
        public async Task<ActionResult<Customer>> DeleteAllCustomerAddress(int id)
        {
            var customerAddresses = await repository.GetCustomerAddresses(id);
            foreach (CustomerAddress address in customerAddresses)
            {
                await repository.Delete(address.Id);
            }
            return Ok($"Addresses removed: {customerAddresses.Count()}");
        }
    }
}
