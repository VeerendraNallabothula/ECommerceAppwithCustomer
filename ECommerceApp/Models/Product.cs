using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Product Name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be between 3 to 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Description Required")]
        [StringLength(128,ErrorMessage ="Description must be less than 128 characters")]
        public string Description { get; set; }

        [Range(0.01,10000.00,ErrorMessage ="Price must be between $0.01 and $10000.00")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [Range(0,1000,ErrorMessage ="Stock quantity must be between 0 and 1000")]
        public int StockQuanity { get; set; }

        public string ImageUrl { get; set; }

        [Range(0,100,ErrorMessage = "Discount percentage must be between 0% and 100%")]
        public int DiscoutPercentage { get; set; }
        public bool IsAvailable { get; set; }
        [Required(ErrorMessage = "CategoryId is required")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }

        public ICollection<Feedback> Feedbacks { get; set; }


    }
}
