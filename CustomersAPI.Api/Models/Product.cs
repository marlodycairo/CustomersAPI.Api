using System.ComponentModel.DataAnnotations;

namespace CustomersAPI.Api.Models
{
    public class Product
    {
        [Key]
        public int Id_Product { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }
}
