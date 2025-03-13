using CustomersAPI.Api.Models;

namespace CustomersAPI.Api.Services.IServices
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetCategoriesAsync();
        public Task<Category> GetCategoryByIdAsync(int id);
        public Task<Category> AddCategoryAsync(Category category);
    }
}
