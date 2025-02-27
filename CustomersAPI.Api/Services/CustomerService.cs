using CustomersAPI.Api.Data;
using CustomersAPI.Api.Models;
using CustomersAPI.Api.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace CustomersAPI.Api.Services
{
    public class CustomerService(IAppDbContext context) : ICustomerService
    {
        private readonly IAppDbContext _context = context;

        public async Task<Customer> AddCustomerAsync(Customer customer)
        {
            if (customer is null || customer.Name is null)
            {
                return null;
            }

            await _context.Customers.AddAsync(customer);

            await _context.SaveChangesAsync();

            return customer;
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            var response = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (response is null)
            {
                return false;
            }

            _context.Customers.Remove(response);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EditCustomer(int id, Customer customer)
        {
            var customerExisting = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id && x.Id == customer.Id);

            if (customerExisting is null)
            {
                return false;
            }

            customerExisting.Name = customer.Name;
            customerExisting.PhoneNumber = customer.PhoneNumber;

            //_context.Customers.Update(customer);

            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Customer> GetCustomerByIdAsync(int id)
        {
            var response = await _context.Customers.FindAsync(id);

            return response;
        }

        public async Task<List<Customer>> GetCustomersAsync()
        {
            var response = await _context.Customers.ToListAsync();

            return response;
        }
    }
}
