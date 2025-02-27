using CustomersAPI.Api.Models;

namespace CustomersAPI.Api.Services.IServices
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetProductsAsync();
        public Task<Product> GetProductByIdAsync(int id);
        public Task<Product> AddProductAsync(Product product);
        public Task<bool> EditProduct(int id, Product product);
        public Task<bool> DeleteProduct(int id);
    }
}
