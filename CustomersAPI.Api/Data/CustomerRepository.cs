using CustomersAPI.Api.Data.IRepository;
using CustomersAPI.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomersAPI.Api.Data
{
    public class CustomerRepository(AppDbContext context) : ICustomerRepository
    {
        private readonly AppDbContext _context = context;

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
            var response = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id, CancellationToken.None);

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
            var customerExisting = await _context.Customers.FirstOrDefaultAsync(x => x.Id == id && x.Id == customer.Id, cancellationToken: default);

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

        public async Task<List<Customer>> GetCustomersOrderDescending()
        {
            var response = await _context.Customers.OrderByDescending(c => c.Name).ToListAsync();

            return response;
        }
    }
}
