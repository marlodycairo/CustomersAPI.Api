using System.ComponentModel.DataAnnotations;

namespace CustomersAPI.Api.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
    }
}
