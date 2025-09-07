



using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookShop.Models
{
    [Table("Books")]
    public class Book
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(40)]
        public string BookName { get; set; }

        public string AuthorName { get; set; }
        [Required]
        public int GenreId { get; set; }
        public Genre Genre { get; set; }
        public string Image { get; set; }
        
        public double Price { get; set; }

        //public Orders Order { get; set; }
        //public int OrderId { get; set; }

        public List<OrderDetail> OrderDetail { get; set; }

        public List<CartDetails> CartDetail { get; set; }
        [NotMapped]
        public string GenreName { get; set; } 
    }
} 
