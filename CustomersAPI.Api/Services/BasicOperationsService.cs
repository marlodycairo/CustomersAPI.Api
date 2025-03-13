using CustomersAPI.Api.Services.IServices;

namespace CustomersAPI.Api.Services
{
    public class BasicOperationsService : IBasicOperationsService
    {
        public async Task<int> Add(int num1, int num2)
        {
            return await Task.FromResult(num1 + num2);
        }

        public async Task<int> Divide(int num1, int num2)
        {
            return await Task.FromResult(num1 / num2);
        }

        public async Task<int> Multiply(int num1, int num2)
        {
            return await Task.FromResult(num1 * num2);
        }

        public async Task<int> Substrac(int num1, int num2)
        {
            return await Task.FromResult(num1 - num2);
        }
    }
}
