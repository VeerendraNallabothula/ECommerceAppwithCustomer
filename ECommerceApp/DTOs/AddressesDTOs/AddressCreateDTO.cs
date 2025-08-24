using System.ComponentModel.DataAnnotations;
namespace ECommerceApp.DTOs.AddressesDTOs
{
    public class AddressCreateDTO
    {
        [Required(ErrorMessage = "CustomerId is required")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "AddressLine1 is required")]
        [StringLength(100, ErrorMessage = "AddressLine1 can't be longer than 100 characters")]
        public string AddressLine1 { get; set; }

        [Required(ErrorMessage ="AddressLine2 is required")]
        [StringLength(100, ErrorMessage = "AddressLine2 can't be longer than 100 characters")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City can't be longer than 50 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State can't be longer than 50 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "PostalCode is required")]
        [RegularExpression(@"^\d{4,6}$", ErrorMessage = "PostalCode must be between 4 and 6 digits or Invalid PostalCode")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country can't be longer than 50 characters")]
        public string Country { get; set; }

    }
}
