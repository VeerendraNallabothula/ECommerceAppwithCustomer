using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ECommerceApp.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        [StringLength(30,ErrorMessage ="Order Number must be less than 30 characters")]
        public string OrderNumber { get; set; }

        [Required]
        public DateTime OrderDate { get; set; }

        [Required(ErrorMessage ="Customer ID is required")]
        public int CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "Billing Address Id is required")]
        public int BillingAddressId { get; set; }

        [ForeignKey("BillingAddressId")]
        public Address BillingAddress { get; set; }

        [Required(ErrorMessage ="Shipping address id required")]
        public int ShippingAddressId { get; set; }

        [ForeignKey("ShippingAddressId")]
        public Address ShippingAddress { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 100000.00, ErrorMessage = "Total amount must be between $0.01 and $100,000.00")]
        public decimal TotalBaseAmount { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00, 100000.00, ErrorMessage ="Total Discount amount must be between $0.00 and $100,000.00")]
        public decimal TotalDiscoutAmount { get; set; }

        
        [Range(0.00,10000.00, ErrorMessage ="Shipping const must between $0.00 and $100,000.00 ")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ShippingCost { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0.00,110000.00, ErrorMessage = "Total amount must be between $0.00 and $110,000.00")]
        public decimal TotalAmount { get; set; }


        [Required]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid order status")]
        public OrderStatus orderStatus { get; set; }

        [Required]
        public ICollection<OrderItem> OrderItems { get; set; }
        public Payment Payment { get; set; }
        public Cancellation Cancellation { get; set; }


    }
}
