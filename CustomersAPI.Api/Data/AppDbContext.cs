using CustomersAPI.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomersAPI.Api.Data
{
    public class AppDbContext : DbContext, IAppDbContext
    {
        /*
        public Guid InstanceId { get; } = Guid.NewGuid(); // Identificador único para cada instancia

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Console.WriteLine($"Nueva instancia de DbContext creada: {InstanceId}");
        }
        */

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Product> Products { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
