using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Models
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int Id { get; set; }
        [Required]

        public int OrderId { get; set; }
        [Required]

        public Orders Order { get; set; }
     
        public int BookId { get; set; }
        [Required]

        public Book Book { get; set; }
      
        public int Quantity { get; set; }
        [Required]
    
        public double UnitPrice { get; set; }
       
    }
}
