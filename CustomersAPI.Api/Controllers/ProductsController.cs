using CustomersAPI.Api.Models;
using CustomersAPI.Api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CustomersAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

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
    }
}
