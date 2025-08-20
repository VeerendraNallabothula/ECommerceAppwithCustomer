using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ECommerceApp.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        [Required]
        [Range(1,100,ErrorMessage ="quantity must be between 1 and 100")]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 10000.00, ErrorMessage = "Unit price must be between $0.01 and $10000.00")]
        public decimal UnitPrice { get; set; }


        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00, 1000.00, ErrorMessage = "Discount must be between $0.00 and $1000.00")]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]    
        public decimal TotalPrice { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime UpdatedAt { get; set; }
    }
}
