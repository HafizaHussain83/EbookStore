using System.ComponentModel.DataAnnotations;

namespace BookShop.Models.DTOs
{
    public class CheckoutModel
    {
        [Required(ErrorMessage = "Name is required")]
        [MaxLength(30)]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "Name can only contain letters and spaces")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [RegularExpression(@"^\d{10}$", ErrorMessage = "Mobile number must be 10 digits")]
        public string MobileNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Payment method is required")]
        public string PaymentMethod { get; set; }
    }
}

