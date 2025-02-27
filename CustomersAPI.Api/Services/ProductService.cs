using CustomersAPI.Api.Data;
using CustomersAPI.Api.Models;
using CustomersAPI.Api.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CustomersAPI.Api.Services
{
    public class ProductService(AppDbContext context) : IProductService
    {
        private readonly AppDbContext _context = context;
        public async Task<Product> AddProductAsync(Product product)
        {
            if (product is null || product.Name is null)
            {
                return null;
            }

            await _context.Products.AddAsync(product);

            await _context.SaveChangesAsync();

            return product;
        }

        public async Task<bool> DeleteProduct(int id)
        {
            var response = await _context.Products.FirstOrDefaultAsync(x => x.Id_Product == id);

            if (response is null)
            {
                return false;
            }

            _context.Products.Remove(response);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditProduct(int id, Product product)
        {
            var productExists = await _context.Products.FirstOrDefaultAsync(x => x.Id_Product == id && x.Id_Product == product.Id_Product);

            if (productExists is null)
            {
                return false;
            }

            productExists.Name = product.Name;
            productExists.Category = product.Category;

            //_context.Products.Update(customer);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            var response = await _context.Products.FirstOrDefaultAsync(x => x.Id_Product == id);

            return response;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            var response = await _context.Products.ToListAsync();

            return response;
        }
    }
}
