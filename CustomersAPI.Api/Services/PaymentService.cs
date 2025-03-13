using CustomersAPI.Api.Services.IServices;

namespace CustomersAPI.Api.Services
{
    public class PaymentService(IPaymentGateway paymentGateway)
    {
        private readonly IPaymentGateway _paymentGateway = paymentGateway;

        public async Task<string> MakePaymentAsync(string cardNumber, decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Amount must be greater than zero");

            bool success = await _paymentGateway.ProcessPaymentAsync(cardNumber, amount);

            return success ? "Payment successful" : "Payment failed";
        }
    }
}
