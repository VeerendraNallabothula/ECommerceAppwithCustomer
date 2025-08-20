using System.Text.Json.Serialization;
namespace ECommerceApp.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum CancellationStatus
    {
        Pending = 1,
        Regected = 8,
        Approved = 9,
    }
}
