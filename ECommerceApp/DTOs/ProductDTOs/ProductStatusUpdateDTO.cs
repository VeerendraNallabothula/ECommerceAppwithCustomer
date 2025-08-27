using System.ComponentModel.DataAnnotations;
namespace ECommerceApp.DTOs.ProductDTOs
{
    public class ProductStatusUpdateDTO
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Availability status is required.")]
        public bool IsAvailable { get; set; }
    }
}
