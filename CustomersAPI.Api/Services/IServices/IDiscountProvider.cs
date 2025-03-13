namespace CustomersAPI.Api.Services.IServices
{
    public interface IDiscountProvider
    {
        Task<decimal> GetDiscountPercentageAsync(int productId);
    }
}
