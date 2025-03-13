using CustomersAPI.Api.Models;
using CustomersAPI.Api.Utils;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Text;

namespace CustomersAPI.Tests.Services
{
    public class ProductServiceV2Tests
    {
        private readonly Mock<HttpMessageHandler> _mockMessageHandler;
        private readonly Mock<IHttpClientFactory> _mockHttpClientFactory;
        private readonly HttpClient _httpClient;

        public ProductServiceV2Tests()
        {
            _mockMessageHandler = new Mock<HttpMessageHandler>();
            _mockHttpClientFactory = new Mock<IHttpClientFactory>();
            _httpClient = new HttpClient(_mockMessageHandler.Object);
        }
        //MétodoAProbar_EscenarioEsperado_ResultadoEsperado
        [Fact]
        public async Task GetProducts_ReturnProductsList_ReturnsOK()
        {
            //Arrange
            var url = "https://localhost:44329/api/ProductsAPI";

            var expectedProducts = new List<ProductResponse>
            {
                new ProductResponse { ProductId = 1, Name = "Product A", Price = 10.99, Description = "Desc A", CategoryName = "Cat A", ImageUrl = "imgA.jpg" },
                new ProductResponse { ProductId = 2, Name = "Product B", Price = 15.50, Description = "Desc B", CategoryName = "Cat B", ImageUrl = "imgB.jpg" }
            };

            var jsonResponse = JsonConvert.SerializeObject(expectedProducts);

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            _mockHttpClientFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            var utilities = new Utilities<ProductResponse>(_mockHttpClientFactory.Object);

            //Act
            var response = await utilities.ExternalServiceUtility(url);

            //Assert
            Assert.NotNull(response.Result);
            Assert.Equal(expectedProducts.Count, response.Result.Count);
        }

        [Fact]
        public async Task GetProducts_IsNotOk_WhenReturnsNull()
        {
            //Arrange
            var url = "https://localhost:44329/api/ProductsAPI";

            var expectedResult = new List<ProductResponse>
            {
                null
            };

            var jsonResponse = JsonConvert.SerializeObject(expectedResult);

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            _mockHttpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            var utilities = new Utilities<ProductResponse>(_mockHttpClientFactory.Object);

            //Act
            var response = await utilities.ExternalServiceUtility(url);

            //Assert
            Assert.Null(response.Result[0]);
            Assert.Equal(expectedResult, response.Result);
        }

        [Fact]
        public async Task GetProductById_ShouldReturnProduct_ReturnProductOk()
        {
            var url = "https://localhost:44329/api/ProductsAPI/9";

            var productExpected = new GenericResponse<ProductResponse>
            {
                Result = new ProductResponse
                {
                    ProductId = 9,
                    Name = "Product 9",
                    Description = "Description Product 9",
                    Price = 19990.50d,
                    CategoryName = "Accesories",
                    ImageUrl = "/path/image.jpg"
                }
            };

            var jsonResponse = JsonConvert.SerializeObject(productExpected);

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(), 
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent(jsonResponse),
                    StatusCode = System.Net.HttpStatusCode.OK
                });

            _mockHttpClientFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            var utilities = new Utilities<ProductResponse>(_mockHttpClientFactory.Object);

            var response = await utilities.ExternalServiceUtilityByEntityById(url);

            Assert.NotNull(response);
            Assert.Equal(productExpected.Result.Name, response.Result.Name);
        }

        [Fact]
        public async Task GetProductById_ShouldNotReturnProduct_WhenReturnsNotFound()
        {
            var url = "https://localhost:44329/api/ProductsAPI/9";

            var responseExpected = new GenericResponse<ProductResponse>()
            {
                Error = null, IsSuccess = false, Message = "", Result = null
            };

            var jsonResponse = JsonConvert.SerializeObject(responseExpected);

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", 
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()).ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent(jsonResponse),
                    StatusCode = System.Net.HttpStatusCode.OK
                });

            _mockHttpClientFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            var utilities = new Utilities<ProductResponse>(_mockHttpClientFactory.Object);

            // Act
            var response = await utilities.ExternalServiceUtilityByEntityById(url);

            // Assert
            Assert.Null(response.Result);
            Assert.Equal(responseExpected.Result, response.Result);
        }

        [Fact]
        public async Task AddProduct_ReturnsOk_WhenProductIsAdded()
        {
            // Arrange
            var url = "https://localhost:44329/api/ProductsAPI";

            var newProduct = new ProductResponse
            {
                ProductId = 0, Name = "New product added", Description = "lorem ipsum", Price = 25990.90, CategoryName = "Category product", ImageUrl = "/path/images/image.jpg"
            };

            var addedProduct = new GenericResponse<ProductResponse>
            {
                Result = new ProductResponse
                {
                    ProductId = 1,
                    Name = "New product added",
                    Description = "lorem ipsum",
                    Price = 25990.90,
                    CategoryName = "Category product",
                    ImageUrl = "/path/images/image.jpg"
                }
            };

            var jsonResult = JsonConvert.SerializeObject(addedProduct);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(newProduct), Encoding.UTF8, "application/json");

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(request =>
                request.Method == HttpMethod.Post &&
                request.RequestUri.ToString() == url), 
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { Content = new StringContent(jsonResult), StatusCode = System.Net.HttpStatusCode.OK});

            _mockHttpClientFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            var utilities = new Utilities<ProductResponse>(_mockHttpClientFactory.Object);

            // Act
            var response = await utilities.ExternalServiceUtilityCreate(url, content);

            // Assert
            Assert.NotNull(response.Result);
            Assert.Contains(newProduct.Name, response.Result.Name, StringComparison.OrdinalIgnoreCase);
            Assert.Equal(newProduct.Description, response.Result.Description);
        }

        [Fact]
        public async Task UpdateProduct_ReturnsOkProductUpdated_WhenProductIsUpdated()
        {
            // Arrange
            var url = "https://localhost:44329/api/ProductsAPI";

            var productToUpdate = new ProductResponse
            {
                ProductId = 1,
                Name = "Product 1",
                Description = "lorem ipsum",
                Price = 25990.90,
                CategoryName = "Category product",
                ImageUrl = "/path/images/image1.jpg"
            };

            var updatedProduct = new GenericResponse<ProductResponse>
            {
                Result = new ProductResponse
                {
                    ProductId = 1,
                    Name = "Product updated",
                    Description = "lorem ipsum",
                    Price = 32890.90,
                    CategoryName = "Category product",
                    ImageUrl = "/path/images/image2.jpg"
                }
            };

            var jsonResponse = JsonConvert.SerializeObject(updatedProduct);

            HttpContent content = new StringContent(JsonConvert.SerializeObject(productToUpdate), Encoding.UTF8, "application/json");

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", 
                ItExpr.Is<HttpRequestMessage>(request =>
                request.Method == HttpMethod.Put &&
                request.RequestUri.ToString() == url),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent(jsonResponse),
                    StatusCode = System.Net.HttpStatusCode.OK
                });

            _mockHttpClientFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            var utilities = new Utilities<ProductResponse>(_mockHttpClientFactory.Object);

            // Act
            var response = await utilities.ExternalServiceUtilityUpdate(url, content);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(updatedProduct.Result.Name, response.Result.Name);
        }

        [Fact]
        public async Task DeleteProduct_ProductIsDeleted_WhenProductIsDeletedReturnsTrue()
        {
            // Arrange
            var url = "https://localhost:44329/api/ProductsAPI/5";

            var apiResponse = new GenericResponse<ProductResponse>
            {
                Message = "Deleted success",
                IsSuccess = true
            };

            var jsonResponse = JsonConvert.SerializeObject(apiResponse);

            _mockMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(request =>
                request.Method == HttpMethod.Delete &&
                request.RequestUri.ToString() == url),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    Content = new StringContent(jsonResponse),
                    StatusCode = System.Net.HttpStatusCode.OK
                });

            _mockHttpClientFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            var utilities = new Utilities<GenericResponse<ProductResponse>>(_mockHttpClientFactory.Object);

            // Act
            var response = await utilities.ExternalServiceUtilityDelete(url);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(apiResponse.IsSuccess, response.IsSuccess);
            Assert.True(response.IsSuccess);
        }
    }
}
