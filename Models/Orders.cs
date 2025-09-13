using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Models
{
    [Table("Orders")]
    public class Orders
    {
        public int Id { get; set; }
        [Required]


        public string UserId { get; set; } = string.Empty;
        // public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int OrderStatusId { get; set; }
        
       
        public OrderStatus? OrderStatus { get; set; }
       // public decimal TotalAmount { get; set; }
        //public string? ShippingAddress { get; set; }
        public bool IsDeleted { get; set; } = false;
        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        [MaxLength(200)]
        public string Address { get; set; }
        [Required]
        public string PaymentMethod { get; set; }
        [Required]
        public bool IsPaid { get; set; } 
        public List<OrderDetail>? OrderDetail { get; set; }

       // public List<Book>? Books { get; set; }  // 👈 Add this
    }
}
