using CustomersAPI.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomersAPI.Api.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {

        /*
public Guid InstanceId { get; } = Guid.NewGuid(); // Identificador único para cada instancia

public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
{
   Console.WriteLine($"Nueva instancia de DbContext creada: {InstanceId}");
}
*/


        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
    }
}
