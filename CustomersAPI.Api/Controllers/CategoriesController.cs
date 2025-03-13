using CustomersAPI.Api.Models;
using CustomersAPI.Api.Services.IServices;
using Microsoft.AspNetCore.Mvc;

namespace CustomersAPI.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController(ICategoryService categoryService) : ControllerBase
    {
        private readonly ICategoryService _categoryService = categoryService;

        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            var response = await _categoryService.GetCategoriesAsync();

            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(Category category)
        {
            var response = await _categoryService.AddCategoryAsync(category);

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var response = await _categoryService.GetCategoryByIdAsync(id);

            return Ok(response);
        }
    }
}
