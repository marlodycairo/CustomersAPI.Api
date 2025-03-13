using CustomersAPI.Api.Data.IRepository;
using CustomersAPI.Api.Models;
using CustomersAPI.Api.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CustomersAPI.Api.Services
{
    public class CustomerService(ICustomerRepository customerRepository) : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository = customerRepository;

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            if (customer is null || customer.Name is null)
            {
                return null;
            }

            var response = await _customerRepository.AddCustomerAsync(customer);

            return response;
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            var response = await _customerRepository.DeleteCustomer(id);

            if (!response)
            {
                return false;
            }
            
            return response;
        }

        public async Task<bool> EditCustomer(int id, Customer customer)
        {
            var response = await _customerRepository.EditCustomer(id, customer);

            if (!response)
            {
                return false;
            }

            return response;
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            var response = await _customerRepository.GetCustomerByIdAsync(id);

            if (response is null)
            {
                return null;
            }

            return response;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            var response = await _customerRepository.GetCustomersAsync();

            return response;
        }

        public async Task<List<Customer>> GetCustomersOrderDescending()
        {
            var response = await _customerRepository.GetCustomersOrderDescending();

            return response;
        }
    }
}
