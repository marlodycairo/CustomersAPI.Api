using CustomersAPI.Api.Services;
using CustomersAPI.Api.Services.IServices;
using Moq;

namespace CustomersAPI.Tests.Services
{
    public class BasicOperationsServiceTests
    {
        private readonly Mock<IBasicOperationsService> _mockBasicOp;
        private readonly OperationsService _operationsService;

        public BasicOperationsServiceTests()
        {
            _mockBasicOp = new Mock<IBasicOperationsService>();
            _operationsService = new OperationsService(_mockBasicOp.Object);
        }

        [Fact]
        public async Task AddTwoNumbers_Returns_Result()
        {
            int num1 = 5;
            int num2 = 8;
            //int total = 13;

            _mockBasicOp.Setup(x => x.Add(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(num1 + num2);

            var response = await _operationsService.Sum(num1, num2);

            Assert.NotEqual(0, response);
            Assert.Equal(13, response);
        }

        [Fact]
        public async Task SubstractNumbers_ThrowException()
        {
            int num1 = 1000;
            int num2 = 750;

            _mockBasicOp.Setup(x => x.Substrac(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(num1 - num2);

            var response = await _operationsService.Substrae(num1, num2);

            Assert.Equal((num1 - num2), response);
        }

        [Fact]
        public async Task DivideNumbers_Exception_Zero()
        {
            int num1 = 22;
            int num2 = 0;

            _mockBasicOp.Setup(x => x.Divide(It.IsAny<int>(), It.IsAny<int>())).Throws<DivideByZeroException>();

            //var response = await _operationsService.Divide(num1, num2);

            await Assert.ThrowsAsync<DivideByZeroException>(async () => await _operationsService.Divide(num1, num2));
        }
    }
}
