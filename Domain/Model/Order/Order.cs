namespace Domain.Model.Order;

public class Order
{
    public int Id { get; set; }
    public IEnumerable<OrderItem> OrderItems { get; set; }
    public int UserId { get; set; }
}