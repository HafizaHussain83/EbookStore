using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Models
{
    [Table("CartDetails")]
    public class CartDetails
    {
        public int Id { get; set; }
        [Required]
        public int ShoppingCartId { get; set; }
        [Required]
        public ShoppingCart ShoppingCart { get; set; }
        public int BookId { get; set; }
        [Required]
        public Book Book { get; set; }
        public int Quantity { get; set; }
    }
}
