using System.ComponentModel.DataAnnotations;
namespace ECommerceApp.DTOs.ProductDTOs
{
    public class ProductCreateDTO
    {
        [Required(ErrorMessage = "Product name is required.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Product name must be 3 to 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters.")]
        public string Description { get; set; }

        [Range(0.01, 10000.00, ErrorMessage = "Price must be between $0.01 and $10000.00.")]
        public decimal Price { get; set; }

        [Range(0, 1000, ErrorMessage = "Stock quantity must be between 0 and 1000.")]
        public int StockQuantity { get; set; }

        [Url(ErrorMessage = "Image URL must be a valid URL.")]
        public string ImageUrl { get; set; }

        [Range(0, 100, ErrorMessage = "Discount percentage must be between 0% and 100%.")]
        public int DiscountPercentage { get; set; }

        [Required(ErrorMessage = "Category ID is required.")]
        public int CategoryId { get; set; }
    }
        
}
