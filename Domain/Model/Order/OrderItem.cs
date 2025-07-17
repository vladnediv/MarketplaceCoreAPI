using System.Text.Json.Serialization;

namespace Domain.Model.Order;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    [JsonIgnore]
    public Order? Order { get; set; }
    public int ProductId { get; set; }
    [JsonIgnore]
    public Product.Product? Product { get; set; }
    public int Quantity { get; set; }
}