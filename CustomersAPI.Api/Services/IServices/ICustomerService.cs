using CustomersAPI.Api.Models;

namespace CustomersAPI.Api.Services.IServices
{
    public interface ICustomerService
    {
        public Task<List<Customer>> GetCustomersAsync();
        public Task<Customer> GetCustomerByIdAsync(int id);
        public Task<Customer> AddCustomerAsync(Customer customer);
        public Task<bool> EditCustomer(int id, Customer customer);
        public Task<bool> DeleteCustomer(int id);
    }
}
