using CustomersAPI.Api.Services.IServices;

namespace CustomersAPI.Api.Services
{
    public class ProductsService(IDiscountProvider discountProvider)
    {
        private readonly IDiscountProvider _discountProvider = discountProvider;

        public async Task<decimal> GetFinalPriceAsync(int productId, decimal originalPrice)
        {
            if (originalPrice < 0)
                throw new ArgumentException("Price cannot be negative");

            var discount = await _discountProvider.GetDiscountPercentageAsync(productId);
            return originalPrice - (originalPrice * (discount / 100));
        }
    }
}
