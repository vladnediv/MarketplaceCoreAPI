namespace Domain.Model.Order;

public class Order
{
    public int Id { get; set; }
    public IEnumerable<OrderItem> OrderItems { get; set; }
    public int UserId { get; set; }
    public Address? Address { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
}