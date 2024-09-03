using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace onl.Models
{
    public class Cart
    {
        [Key]
        public int cart_id { get; set; }
        public string? Name { get; set; }
        [Required]
        public string? Thumbnail { get; set; }
        public double Price { get; set; }
        [Required]
        public string? Description { get; set; }
        
        public string? suser { get; set; }

        public string? IsPurchased {  get; set; }

        public string? OrderId { get; set; } // Added for storing Razorpay order ID

    }
}
