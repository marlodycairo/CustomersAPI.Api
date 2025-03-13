using CustomersAPI.Api.Services.IServices;

namespace CustomersAPI.Api.Services
{
    public class OperationsService(IBasicOperationsService basicOperations)
    {
        private readonly IBasicOperationsService _basicOperationsService = basicOperations;

        public async Task<int> Sum(int num1, int num2)
        {
            var result = await _basicOperationsService.Add(num1, num2);

            return result;
        }

        public async Task<int> Substrae(int num1, int num2)
        {
            var result = await _basicOperationsService.Substrac(num1, num2);

            return result;
        }

        public async Task<int> Multi(int num1, int num2)
        {
            var result = await _basicOperationsService.Multiply(num1, num2);

            return result;
        }

        public async Task<int> Divide(int num1, int num2)
        {
            var result = await _basicOperationsService.Divide(num1, num2);

            return result;
        }
    }
}
