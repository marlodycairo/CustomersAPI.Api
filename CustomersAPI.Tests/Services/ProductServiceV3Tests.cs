using CustomersAPI.Api.Models;
using CustomersAPI.Api.Utils;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;

namespace CustomersAPI.Tests.Services
{
    public class ProductServiceV3Tests
    {
        private readonly Mock<HttpMessageHandler> _mockMessageHandler;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly HttpClient _httpClient;

        public ProductServiceV3Tests()
        {
            _mockMessageHandler = new Mock<HttpMessageHandler>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _httpClient = new HttpClient(_mockMessageHandler.Object);
        }

        // var url = "https://localhost:44329/api/ProductsAPI";
        // MethodName_StateUnderTest_ExpectedBehavior
        [Fact]
        public async Task GetAll_WhenCalled_ReturnsProductList()
        {
            var url = "https://localhost:44329/api/ProductsAPI";

            var expectedProducts = new List<ProductResponse>
            {
                new ProductResponse{ ProductId = 1, Name = "Product 1", Description = "Lorem ipsum1", Price = 29999.99, CategoryName = "Category 1", ImageUrl = "/image/image1.jpg"},
                new ProductResponse{ ProductId = 2, Name = "Product 2", Description = "Lorem ipsum2", Price = 52990.50, CategoryName = "Category 2", ImageUrl = "/image/image2.jpg"}
            };

            var jsonResponse = JsonConvert.SerializeObject(expectedProducts);

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            _mockHttpClientFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            var utilities = new Utilities<ProductResponse>(_mockHttpClientFactory.Object);

            var response = await utilities.ExternalServiceUtility(url);

            Assert.NotEmpty(response.Result);
            Assert.NotNull(response.Result);
            Assert.Equal(expectedProducts.Count, response.Result.Count);
        }
    }
}
