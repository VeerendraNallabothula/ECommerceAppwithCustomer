using System.ComponentModel.DataAnnotations;
namespace ECommerceApp.DTOs.ProductDTOs
{
    public class ProductUpdateDTO
    {
        [Required(ErrorMessage = "Product ID is required.")]
        public int Id { get; set; }

        [Required(ErrorMessage ="Product Name is required")]
        [StringLength(100,MinimumLength =3,ErrorMessage ="Name must be 3 to 100 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage ="Description is required")]
        [StringLength(10,ErrorMessage ="Descriptioin cannot exceeds 10 characters.")]
        public string Description { get; set; }

        [Range(0.01,1000.00,ErrorMessage ="Price must be between $0.01 and $1000.00.")]
        public decimal  Price { get; set; }

        [Range(0,1000, ErrorMessage ="Stock Qunatity must be between 0 and 1000")]
        public int StockQuantity { get; set; }

        [Url(ErrorMessage ="Invalid image Url.")]
        public string ImageUrl { get; set; }

        [Range(0,100, ErrorMessage ="Discount Percentate must be between 1% to 100%")]
        public int DiscountPercentage { get; set; }

        [Required(ErrorMessage ="Category Id is required")]
        public int CategoryId { get; set; }

    }
}
