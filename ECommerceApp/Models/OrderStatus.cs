using System.Text.Json.Serialization;
namespace ECommerceApp.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum OrderStatus
    {
       Pending = 1,
       Processing = 2,
       Shipped = 3,
       Dilivered = 4,
       Cancelled = 5,
    }
}
