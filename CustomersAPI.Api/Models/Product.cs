using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CustomersAPI.Api.Models
{
    public class Product
    {
        [Key]
        public int Id_Product { get; set; }
        public string Name { get; set; }

        [Range(0, 100000)]
        public double Price { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
