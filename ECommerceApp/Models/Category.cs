using System.ComponentModel.DataAnnotations;
namespace ECommerceApp.Models
{
    public class Category
    {
        public  int Id { get; set; }

        [Required(ErrorMessage ="Category Name is required")]
        [StringLength(100,MinimumLength =3,ErrorMessage ="Category name must be between 3 to 100 characters")]
        public string Name { get; set; }
        [StringLength(500, ErrorMessage = "Description must be less than 500 characters")]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public ICollection <Product> Products { get; set; }
    }
}
