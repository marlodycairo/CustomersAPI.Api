namespace CustomersAPI.Api.Services.IServices
{
    public interface IBasicOperationsService
    {
        Task<int> Add(int num1, int num2);
        Task<int> Substrac(int num1, int num2);
        Task<int> Multiply(int num1, int num2);
        Task<int> Divide(int num1, int num2);
    }
}
