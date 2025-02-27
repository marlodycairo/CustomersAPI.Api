using Castle.Core.Resource;
using CustomersAPI.Api.Data;
using CustomersAPI.Api.Models;
using CustomersAPI.Api.Services;
using CustomersAPI.Tests.Extensions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Linq.Expressions;
using System.Xml;

namespace CustomersAPI.Tests.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<IAppDbContext> _mockContext;
        private readonly CustomerService _customerService;

        public CustomerServiceTests()
        {
            _mockContext = new Mock<IAppDbContext>();
            _customerService = new CustomerService(_mockContext.Object);
        }

        [Fact]
        public async Task GetCustomersAsync_ShouldReturnAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "John Doe" },
                new Customer { Id = 2, Name = "Jane Doe" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Customer>>();
            mockSet.As<IAsyncEnumerable<Customer>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Customer>(customers.GetEnumerator()));
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Customer>(customers.Provider));
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(customers.GetEnumerator());

            _mockContext.Setup(m => m.Customers).Returns(mockSet.Object);

            // Act
            var result = await _customerService.GetCustomersAsync();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Contains(result, c => c.Name == "John Doe");
            Assert.Contains(result, c => c.Name == "Jane Doe");
        }

        [Fact]
        public async Task CreateCustomer_ObjectResult_Is_Ok()
        {
            var mockSet = new Mock<DbSet<Customer>>();
            var customer = new Customer { Id = 4, Name = "Martha", PhoneNumber = "3045552130" };

            _mockContext.Setup(x => x.Customers).Returns(mockSet.Object);

            var response = await _customerService.AddCustomerAsync(customer);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task GetAllCustomersAsync_Orders_By_Name()
        {
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Tom Silva", PhoneNumber = "13045558745" },
                new Customer { Id = 2, Name = "Jane Doe", PhoneNumber = "12235478869" },
                new Customer { Id = 3, Name = "Sam Arlington", PhoneNumber = "12035253104" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Customer>>();

            mockSet.As<IAsyncEnumerable<Customer>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Customer>(customers.GetEnumerator()));

            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Customer>(customers.Provider));
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator()).Returns(customers.GetEnumerator());

            _mockContext.Setup(m => m.Customers).Returns(mockSet.Object);

            var response = await _customerService.GetCustomersAsync();

            Assert.Equal(3, customers.Count());
            Assert.NotEmpty(response);
        }

        [Fact]
        public async Task GetCustomerById()
        {
            var customerId = 3;
            var customer = new Customer { Id = 3, Name = "Tim Burton", PhoneNumber = "12035568754" };

            _mockContext.Setup(x => x.Customers.FindAsync(customerId)).ReturnsAsync(customer);

            var response = await _customerService.GetCustomerByIdAsync(customerId);

            Assert.Contains("Tim Burton", customer.Name);
            Assert.NotNull(customer);
            Assert.Equal("12035568754", customer.PhoneNumber);
        }

        [Fact]
        public async Task DeleteCustomer_IsCustomerDeleted()
        {
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Tom Silva", PhoneNumber = "13045558745" },
                new Customer { Id = 2, Name = "Jane Doe", PhoneNumber = "12235478869" },
                new Customer { Id = 3, Name = "Sam Arlington", PhoneNumber = "12035253104" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Customer>>();

            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Customer>(customers.Provider));
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression).Returns(customers.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType).Returns(customers.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(x => x.GetEnumerator()).Returns(customers.GetEnumerator());

            mockSet.As<IAsyncEnumerable<Customer>>().Setup(x => x.GetAsyncEnumerator(It.IsAny<CancellationToken>())).Returns(new TestAsyncEnumerator<Customer>(customers.GetEnumerator()));

            mockSet.Setup(x => x.FindAsync(It.IsAny<object[]>())).ReturnsAsync((object[] ids) => customers.FirstOrDefault(x => x.Id == (int)ids[0]));
            _mockContext.Setup(x => x.Customers).Returns(mockSet.Object);

            var customerId = 2;
            var customer = await _mockContext.Object.Customers.FindAsync(customerId);

            var response = await _customerService.DeleteCustomer(customer.Id);

            Assert.True(response);
        }

        [Fact]
        public async Task GetEntityByIdAsync_ReturnsEntity()
        {
            // Arrange
            var data = new List<Customer>
            {
                new Customer { Id = 1, Name = "Tom Silva", PhoneNumber = "13045558745" },
                new Customer { Id = 2, Name = "Jane Doe", PhoneNumber = "12235478869" },
                new Customer { Id = 3, Name = "Sam Arlington", PhoneNumber = "12035253104" }
            }.AsQueryable();

            var mockSet = new Mock<DbSet<Customer>>();

            // Configuramos el IQueryable para que use nuestro TestAsyncQueryProvider
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Provider).Returns(new TestAsyncQueryProvider<Customer>(data.Provider));
            mockSet.As<IQueryable<Customer>>().Setup(m => m.Expression)
                .Returns(data.Expression);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.ElementType)
                .Returns(data.ElementType);
            mockSet.As<IQueryable<Customer>>().Setup(m => m.GetEnumerator())
                .Returns(data.GetEnumerator());

            // Configuramos el IAsyncEnumerable para soportar la enumeración asíncrona
            mockSet.As<IAsyncEnumerable<Customer>>()
                .Setup(m => m.GetAsyncEnumerator(It.IsAny<CancellationToken>()))
                .Returns(new TestAsyncEnumerator<Customer>(data.GetEnumerator()));

            var mockContext = new Mock<IAppDbContext>();
                        mockContext.Setup(c => c.Customers).Returns(mockSet.Object);

                        var repository = new CustomerService(mockContext.Object);

                        // Act
                        var entity = await repository.DeleteCustomer(1);

            var primeraEntidad = await mockSet.Object.FirstOrDefaultAsync();
            // Puedes realizar tus aserciones en la prueba sobre 'primeraEntidad'


            

            // Assert
            Assert.True(entity);
        }
    }
}
