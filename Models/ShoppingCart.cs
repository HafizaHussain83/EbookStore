
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Models
{
    [Table("ShoppingCarts")]
    public class ShoppingCart
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        // Navigation property
        public ICollection<CartDetails> CartDetails { get; set; }

        // Add this if you need to reference the user
        // public IdentityUser User { get; set; }
    }
}
