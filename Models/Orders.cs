using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Models
{
    [Table("Orders")]
    public class Orders
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        [Required]
        public int OrderStatusId { get; set; }
        
       
        public OrderStatus? OrderStatus { get; set; }
       // public decimal TotalAmount { get; set; }
        //public string? ShippingAddress { get; set; }
        public bool IsDeleted { get; set; } = false;
        public List<OrderDetail>? OrderDetail { get; set; }

       // public List<Book>? Books { get; set; }  // 👈 Add this
    }
}
