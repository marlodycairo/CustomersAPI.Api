namespace CustomersAPI.Api.Services.IServices
{
    public interface IPaymentGateway
    {
        Task<bool> ProcessPaymentAsync(string cardNumber, decimal amount);
    }
}
