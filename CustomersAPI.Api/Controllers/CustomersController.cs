using CustomersAPI.Api.Models;
using CustomersAPI.Api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CustomersAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController
        //(
        ////IDbContextFactory<AppDbContext> dbContextFactory, 
        ////IDbContextFactory<AppDbContext> pooledDbContextFactory,
        //ICustomerService customerService) 
        : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }
        //private readonly IDbContextFactory<AppDbContext> _dbContextFactory = dbContextFactory;
        //private readonly IDbContextFactory<AppDbContext> _pooledDbContextFactory = pooledDbContextFactory;


        /*
        [HttpGet("test-factory")]
        public async Task<ActionResult<string>> TestDbContextFactory()
        {
            using var context = _dbContextFactory.CreateDbContext();
            Console.WriteLine($"📌 Request con DbContextFactory - Instance ID: {context.InstanceId}");
            await Task.Delay(3000); // Simula un proceso de 3 segundos
            Console.WriteLine($"✅ Request completada - Instance ID: {context.InstanceId}");
            return $"DbContextFactory Instance ID: {context.InstanceId}";
        }

        [HttpGet("test-pooled-factory")]
        public async Task<ActionResult<string>> TestPooledDbContextFactory()
        {
            using var context = _pooledDbContextFactory.CreateDbContext();
            Console.WriteLine($"📌 Request con PooledDbContextFactory - Instance ID: {context.InstanceId}");
            await Task.Delay(3000);
            Console.WriteLine($"✅ Request completada - Instance ID: {context.InstanceId}");
            return $"PooledDbContextFactory Instance ID: {context.InstanceId}";
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            using var context = await _context.CreateDbContextAsync();
            var response = await context.Customers.ToListAsync();

            return Ok(response);
        }
        */

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _customerService.GetCustomersAsync();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Customer customer)
        {
            var response = await _customerService.AddCustomerAsync(customer);

            return Ok(response != null ? new { message = "Customer created", result = response } : StatusCode(StatusCodes.Status400BadRequest));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _customerService.GetCustomerByIdAsync(id);

            return Ok(response != null ? new { response } : StatusCode(StatusCodes.Status404NotFound));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Customer customer)
        {
            var response = await _customerService.EditCustomer(id, customer);

            return Ok(response == true ? new { message = "Updated success" } : StatusCode(StatusCodes.Status404NotFound));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _customerService.DeleteCustomer(id);

            return Ok(response == true ? new { message = "Deleted success" } : StatusCode(StatusCodes.Status404NotFound));
        }

        //[HttpPatch("{id}")]
        //public async Task<IActionResult> PatchCustomer(int id, [FromBody] JsonPatchDocument<Customer> patchDocument)
        //{
        //    if (patchDocument is null)
        //    {
        //        return BadRequest(new { message = "Invalid patch request" });
        //    }

        //    var customer = await _context.Customers.FindAsync(id);

        //    if (customer is null)
        //    {
        //        return NotFound(new { message = "Invalid patch request" });
        //    }

        //    patchDocument.ApplyTo(customer, ModelState);

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    await _context.SaveChangesAsync();

        //    return Ok(new { message = "Customer updated successfully", customer });
        //}

        [HttpGet("error")]
        public Task<IActionResult> SimulateError()
        {
            throw new Exception("This is a simulated error!");
        }

        //[HttpGet("dbcontext-instance")]
        //public ActionResult<string> GetDbContextInstance()
        //{
        //    return $"DbContext Instance ID: {_context.InstanceId}";
        //}
    }
}
