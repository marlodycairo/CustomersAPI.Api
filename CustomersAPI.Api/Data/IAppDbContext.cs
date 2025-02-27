using CustomersAPI.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomersAPI.Api.Data
{
    public interface IAppDbContext
    {
        DbSet<Customer> Customers { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
