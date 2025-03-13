using CustomersAPI.Api.Services;
using CustomersAPI.Api.Services.IServices;
using Moq;

namespace CustomersAPI.Tests.Services
{
    public class ProductsServiceTests
    {
        private readonly Mock<IDiscountProvider> _mockDiscountProvider;
        private readonly ProductsService _productsService;

        public ProductsServiceTests()
        {
            _mockDiscountProvider = new Mock<IDiscountProvider>();

            _productsService = new ProductsService(_mockDiscountProvider.Object);
        }

        [Fact]
        public async Task GetFinalPrice_ReturnsCorrectPrice_WhenDiscountIsFiftyPercent()
        {
            _mockDiscountProvider.Setup(x => x.GetDiscountPercentageAsync(It.IsAny<int>())).ReturnsAsync(50.0m);

            var finalPrice = await _productsService.GetFinalPriceAsync(1, 50000);

            Assert.Equal(25000.00m, finalPrice);
            _mockDiscountProvider.Verify(x => x.GetDiscountPercentageAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetFinalPrice_ReturnsOriginalPrice_WhenDiscountIsZero()
        {
            _mockDiscountProvider.Setup(x => x.GetDiscountPercentageAsync(It.IsAny<int>())).ReturnsAsync(0.0m);

            var finalPrice = await _productsService.GetFinalPriceAsync(1, 50000);

            Assert.Equal(50000.00m, finalPrice);
            _mockDiscountProvider.Verify(x => x.GetDiscountPercentageAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetFinalPrice_ThrowsArgumentException_WhenOriginalPriceIsNegative()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _productsService.GetFinalPriceAsync(1, -25000.00m));
        }
    }
}
