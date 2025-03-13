using CustomersAPI.Api.Services;
using CustomersAPI.Api.Services.IServices;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomersAPI.Tests.Services
{
    public class PaymentServiceTests
    {
        private readonly Mock<IPaymentGateway> _mockPaymentGateway;
        private readonly PaymentService _paymentService;

        public PaymentServiceTests()
        {
            _mockPaymentGateway = new Mock<IPaymentGateway>();
            _paymentService = new PaymentService(_mockPaymentGateway.Object);
        }

        [Fact]
        public async Task MakePayment_IsSuccess()
        {
            string cardNumber = "8080254196305566";
            decimal amount = 25000.95m;

            //_mockPaymentGateway.Setup(x => x.ProcessPaymentAsync(It.IsAny<string>(), It.IsAny<decimal>())).ReturnsAsync(true);

            _mockPaymentGateway.Setup(x => x.ProcessPaymentAsync(cardNumber, amount)).ReturnsAsync(true);

            var response = await _paymentService.MakePaymentAsync(cardNumber, amount);

            //Assert.NotNull(response);
            Assert.Equal("Payment successful", response);
            _mockPaymentGateway.Verify(x => x.ProcessPaymentAsync(cardNumber, amount));
        }

        [Fact]
        public async Task MakePayment_IsNotSuccess()
        {
            string cardNumber = "8080254196305566";
            decimal amount = 25000.95m;

            _mockPaymentGateway.Setup(x => x.ProcessPaymentAsync(It.IsAny<string>(), It.IsAny<decimal>())).ReturnsAsync(false);

            var response = await _paymentService.MakePaymentAsync(cardNumber, amount);

            //Assert.NotNull(response);
            Assert.Equal("Payment failed", response);
            _mockPaymentGateway.Verify(x => x.ProcessPaymentAsync(cardNumber, amount));
        }

        [Fact]
        public async Task MakePayment_ThrowsArgumentException_WhenAmountIsNegative()
        {
            string cardNumber = "8080254196305566";
            decimal amount = -25000.95m;

            await Assert.ThrowsAsync<ArgumentException>(() => _paymentService.MakePaymentAsync(cardNumber, amount));
        }
    }
}
