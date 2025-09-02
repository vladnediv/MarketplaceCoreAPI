namespace Domain.Model.Order;

public class Order
{
    public int Id { get; set; }
    public IEnumerable<OrderItem> OrderItems { get; set; }
    public int UserId { get; set; }
    public int AddressId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Note { get; set; }
}

public enum OrderStatus
{
    New,
    Accepted,
    Done,
    Canceled,
    Payed
}

public enum PaymentMethod
{
    Prepayment,
    PaymentBeforeDelivery
}