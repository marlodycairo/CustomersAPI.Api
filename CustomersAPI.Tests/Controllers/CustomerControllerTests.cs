using CustomersAPI.Api.Controllers;
using CustomersAPI.Api.Models;
using CustomersAPI.Api.Services.IServices;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CustomersAPI.Tests.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICustomerService> _customerServiceMock;
        private readonly CustomersController _controller;

        public CustomerControllerTests()
        {
            _customerServiceMock = new Mock<ICustomerService>();
            _controller = new CustomersController(_customerServiceMock.Object);
        }

        [Fact]
        public async Task GetCustomer_ReturnsOk_WhenCustomerExists()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer { Id = customerId, Name = "Test Customer", PhoneNumber = "5555555555" };
            _customerServiceMock.Setup(s => s.GetCustomerByIdAsync(customerId))
                               .ReturnsAsync(customer);

            // Act
            var result = await _controller.GetById(customerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            // El valor del resultado es un objeto anónimo con una propiedad 'response'
            var responseObject = okResult.Value;

            // Extraemos la propiedad 'response' del objeto anónimo
            var propertyInfo = responseObject.GetType().GetProperty("response");
            Assert.NotNull(propertyInfo); // Aseguramos que existe la propiedad

            var returnedCustomer = propertyInfo.GetValue(responseObject) as Customer;
            Assert.NotNull(returnedCustomer); // Aseguramos que es un Customer

            Assert.Equal(customerId, returnedCustomer.Id);
        }
    }
}
