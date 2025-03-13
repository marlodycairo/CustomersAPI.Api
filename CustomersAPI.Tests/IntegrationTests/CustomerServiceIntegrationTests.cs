using CustomersAPI.Api.Data;
using CustomersAPI.Api.Data.IRepository;
using CustomersAPI.Api.Models;
using CustomersAPI.Api.Services;
using CustomersAPI.Api.Services.IServices;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace CustomersAPI.Tests.IntegrationTests
{
    public class CustomerServiceIntegrationTests : IDisposable
    {
        private readonly AppDbContext _context;
        private readonly Mock<ICustomerRepository> _mockCustomerRepository;
        private readonly ICustomerService _customerService;

        public CustomerServiceIntegrationTests(AppDbContext context)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = context;
            _mockCustomerRepository = new Mock<ICustomerRepository>();
            _customerService = new CustomerService(_mockCustomerRepository.Object);

            _context.Customers.AddRange(
                new Customer { Id = 1, Name = "Jhon Doe", PhoneNumber = "13205557784"},
                new Customer { Id = 2, Name = "Mary Start", PhoneNumber = "12035558748"});

            _context.SaveChanges();
        }

        [Fact]
        public async Task CreateCustomer_ReturnsCustomerAdded()
        {
            var customer = new Customer { Id = 3, Name = "Sara Doe", PhoneNumber = "13215558799" };

            var response = await _customerService.AddCustomerAsync(customer);

            Assert.NotNull(response);
            Assert.Equal(customer, response);
        }

        [Fact]
        public async Task GetCustomers_IsNotNull()
        {
            var response = await _customerService.GetCustomersAsync();

            Assert.NotNull(response);
            Assert.Equal(_context.Customers.Count(), response.Count);
        }

        [Fact]
        public async Task GetCustomerById_IsCustomerNull()
        {
            var customerId = 3;

            var response = await _customerService.GetCustomerByIdAsync(customerId);

            Assert.Null(response);
        }


        [Fact]
        public async Task CustomerAdded()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase("customerDB")
                    .Options;

            var context = new AppDbContext(options);

            var customer = new Customer { Id = 1, Name = "John Doe", PhoneNumber = "13215558799" };

            var response = await _customerService.AddCustomerAsync(customer);

            Assert.NotNull(response);
            Assert.Equal(customer, response);
        }


        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
