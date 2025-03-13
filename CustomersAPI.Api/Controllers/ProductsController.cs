using CustomersAPI.Api.Models;
using CustomersAPI.Api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CustomersAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService, IProductExternalApiService productExternalApi) : ControllerBase
    {
        private readonly IProductService _productService = productService;
        private readonly IProductExternalApiService _productExternalApi = productExternalApi;
        private readonly string url = "https://localhost:44329/api/ProductsAPI";

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Get()
        {
            var response = await _productService.GetProductsAsync();

            if (response is null)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _productService.AddProductAsync(product);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var response = await _productService.GetProductByIdAsync(id);

            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await _productService.EditProduct(id, product);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var response = await _productService.DeleteProduct(id);

            return Ok(response);
        }

        [HttpGet("SendRequestProducts")]
        public async Task<IActionResult> SendRequestProducts()
        {
            var response = await _productExternalApi.SendRequestAsync(url);

            return Ok(response);
        }
    }
}
