using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceApp.Models
{
    public class Address
    {
        public int id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "AddressLine1 is required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "AddressLine1 must be between 5 and 100 characters")]
        public string AddressLine1 { get; set; }

        [StringLength(100, ErrorMessage = "AddressLine2 must be less than 100 characters")]
        public string AddressLine2 { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50,ErrorMessage = "City cannot be excceds 50 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "State cannot be excceds 50 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "postal code is required")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters")]
        public string Country { get; set; }

    }
}
