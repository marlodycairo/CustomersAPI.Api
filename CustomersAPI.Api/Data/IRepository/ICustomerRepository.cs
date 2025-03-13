using CustomersAPI.Api.Models;

namespace CustomersAPI.Api.Data.IRepository
{
    public interface ICustomerRepository
    {
        public Task<List<Customer>> GetCustomersAsync();
        public Task<Customer> GetCustomerByIdAsync(int id);
        public Task<Customer> AddCustomerAsync(Customer customer);
        public Task<bool> EditCustomer(int id, Customer customer);
        public Task<bool> DeleteCustomer(int id);
        public Task<List<Customer>> GetCustomersOrderDescending();
    }
}
