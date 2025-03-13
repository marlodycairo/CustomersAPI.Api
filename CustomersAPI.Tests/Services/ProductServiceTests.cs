using CustomersAPI.Api.Models;
using CustomersAPI.Api.Services;
using CustomersAPI.Api.Utils;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace CustomersAPI.Tests.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private readonly Mock<IHttpClientFactory> _mockClientFactory;
        private readonly HttpClient _httpClient;

        public ProductServiceTests()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _mockClientFactory = new Mock<IHttpClientFactory>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
        }

        [Fact]
        public async Task ExternalServiceUtility_Returns_Valid_Response()
        {
            // Arrange
            var url = "https://localhost:44329/api/ProductsAPI";

            var expectedProducts = new List<ProductResponse>
            {
                new ProductResponse { ProductId = 1, Name = "Product A", Price = 10.99, Description = "Desc A", CategoryName = "Cat A", ImageUrl = "imgA.jpg" },
                new ProductResponse { ProductId = 2, Name = "Product B", Price = 15.50, Description = "Desc B", CategoryName = "Cat B", ImageUrl = "imgB.jpg" }
            };

            var jsonResponse = JsonConvert.SerializeObject(expectedProducts);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            _mockClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            var _utilities = new Utilities<ProductResponse>(_mockClientFactory.Object);

            // Act
            var result = await _utilities.ExternalServiceUtility(url);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Product A", result.Result[0].Name);
            Assert.Equal("Product B", result.Result[1].Name);
            Assert.Equal("Product A", result.Result[0].Name);
        }

        [Fact]
        public async Task GetProductById_CallApi_ReturnById()
        {
            var mockUtilities = new Mock<IUtilities<ProductResponse>>();

            var url = "https://localhost:44329/api/ProductsAPI/2";

            var expectedProduct = new GenericResponse<ProductResponse>
            {
                Result = new ProductResponse { ProductId = 1, Name = "Product 1", Description = "Description product" }
            };

            var jsonResponse = JsonConvert.SerializeObject(expectedProduct);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(jsonResponse)});

            _mockClientFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            var utilities = new Utilities<ProductResponse>(_mockClientFactory.Object);

            var result = await utilities.ExternalServiceUtilityByEntityById(url);

            Assert.NotNull(result.Result);
            Assert.Equal("Product 1", result.Result.Name);
        }

        [Fact]
        public async Task AddNewProduct_CallApi_ReturnsProductAdded()
        {
            var mockUtilities = new Mock<IUtilities<ProductResponse>>();

            var url = "https://localhost:44329/api/ProductsAPI";

            var productToPost = new ProductResponse
            {
                ProductId = 0, // Normalmente 0 para nuevos productos
                Name = "New Product",
                Price = 25.99,
                Description = "New product description",
                CategoryName = "New Category",
                ImageUrl = "newImg.jpg"
            };

            var expectedResponse = new ApiResponse<ProductResponse>
            {
                Result = new ProductResponse
                {
                    ProductId = 3, // El servidor asignaría un nuevo ID
                    Name = "New Product",
                    Price = 25.99,
                    Description = "New product description",
                    CategoryName = "New Category",
                    ImageUrl = "newImg.jpg"
                }
            };

            var jsonResponse = JsonConvert.SerializeObject(expectedResponse);

            var json = JsonConvert.SerializeObject(productToPost);

            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", 
                ItExpr.Is<HttpRequestMessage>(request => 
                    request.Method == HttpMethod.Post &&
                    request.RequestUri.ToString() == url), 
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            _mockClientFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            var utilities = new Utilities<ProductResponse>(_mockClientFactory.Object);

            var response = await utilities.ExternalServiceUtilityCreate(url, content);

            Assert.NotNull(response);
            Assert.Equal(productToPost.Name, response.Result.Name);
        }

        [Fact]
        public async Task UpdateProducts_ShouldReturnProductUpdated()
        {
            var mockUtilities = new Mock<Utilities<ProductResponse>>();

            var url = "https://localhost:44329/api/ProductsAPI";

            var productToUpdate = new ProductResponse
            {
                ProductId = 3,
                Name = "Update Product",
                Price = 33.99,
                Description = "Update product description",
                CategoryName = "Existing Category",
                ImageUrl = "ExistingImg.jpg"
            };

            var expectedResponse = new ApiResponse<ProductResponse>
            {
                Result = new ProductResponse
                {
                    ProductId = 3,
                    Name = "Update Product",
                    Price = 33.99,
                    Description = "Update product description",
                    CategoryName = "Existing Category",
                    ImageUrl = "ExistingImg.jpg"
                }
            };

            var jsonResponse = JsonConvert.SerializeObject(expectedResponse);

            var json = JsonConvert.SerializeObject(productToUpdate);

            HttpContent httpContent = new StringContent(json, Encoding.UTF8, "application/json");

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", 
                ItExpr.Is<HttpRequestMessage>(request =>
                    request.Method == HttpMethod.Put &&
                    request.RequestUri.ToString() == url), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            _mockClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            var utilities = new Utilities<ProductResponse>(_mockClientFactory.Object);

            var response = await utilities.ExternalServiceUtilityUpdate(url, httpContent);

            Assert.NotNull(response);
            Assert.Equal(productToUpdate.Name, response.Result.Name);
        }

        [Fact]
        public async Task DeleteProduct_ReturnTrue_WhenProductIsDeleted()
        {
            var url = "https://localhost:44329/api/ProductsAPI/3";

            var expectedResponse = new GenericResponse<GenericResponse<ProductResponse>> { Message = "Deleted success", IsSuccess = true, Error = null, Result = null };
            var jsonResponse = JsonConvert.SerializeObject(expectedResponse);

            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(request =>
                request.Method == HttpMethod.Delete &&
                request.RequestUri.ToString() == url), 
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(jsonResponse)
                });

            _mockClientFactory.Setup(c => c.CreateClient(It.IsAny<string>())).Returns(_httpClient);

            var utilities = new Utilities<GenericResponse<ProductResponse>>(_mockClientFactory.Object);

            var response = await utilities.ExternalServiceUtilityDelete(url);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task SendRequestAsync_ShouldReturnExpectedData()
        {
            // Arrange
            var mockUtilities = new Mock<IUtilities<ProductResponse>>();

            var fakeResponse = new GenericResponse<IList<ProductResponse>>
            {
                Result = new List<ProductResponse>
                {
                    new ProductResponse { ProductId = 1, Name = "Product 1" },
                    new ProductResponse { ProductId = 2, Name = "Product 2" }
                }
            };

            // Configurar el mock para devolver el resultado esperado
            mockUtilities.Setup(u => u.ExternalServiceUtility(It.IsAny<string>()))
                         .ReturnsAsync(fakeResponse);

            var service = new ProductExternalApiService(mockUtilities.Object);

            // Act
            var result = await service.SendRequestAsync("https://localhost:44329/api/ProductsAPI");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Result.Count);
            Assert.Equal("Product 1", result.Result[0].Name);
            Assert.Equal("Product 2", result.Result[1].Name);
        }

        [Fact]
        public async Task SendRequestAsync_ShouldHandleEmptyResponse()
        {
            // Arrange
            var mockUtilities = new Mock<IUtilities<ProductResponse>>();

            var fakeResponse = new GenericResponse<IList<ProductResponse>>
            {
                Result = new List<ProductResponse>() // Lista vacía
            };

            mockUtilities.Setup(u => u.ExternalServiceUtility(It.IsAny<string>()))
                         .ReturnsAsync(fakeResponse);

            var service = new ProductExternalApiService(mockUtilities.Object);

            // Act
            var result = await service.SendRequestAsync("https://localhost:44329/api/ProductsAPI");

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Result);
        }

        [Fact]
        public async Task SendRequestAsync_ShouldReturnValidResponse()
        {
            // Arrange
            var mockUtilities = new Mock<IUtilities<ProductResponse>>();
            var mockResponse = new GenericResponse<IList<ProductResponse>>
            {
                Result = new List<ProductResponse>
                {
                    new ProductResponse { ProductId = 1, Name = "Product 1" },
                    new ProductResponse { ProductId = 2, Name = "Product 2" }
                }
            };

            // Configuración del mock
            mockUtilities
                .Setup(u => u.ExternalServiceUtility(It.IsAny<string>()))
                .ReturnsAsync(mockResponse);

            var service = new ProductExternalApiService(mockUtilities.Object);

            // Act
            var result = await service.SendRequestAsync("https://localhost:44329/api/ProductsAPI");

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.Result);
            Assert.Equal(2, result.Result.Count);
        }


        [Fact]
        public async Task ExternalServiceUtility_ShouldThrowException_WhenUrlIsNull()
        {
            // Arrange
            var service = new Utilities<ProductResponse>(_mockClientFactory.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.ExternalServiceUtility(null!));
        }
    }
}
