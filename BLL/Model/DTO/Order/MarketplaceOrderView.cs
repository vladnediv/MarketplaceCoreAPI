using BLL.Model.DTO.Order.IncludedModels;
using Domain.Model.Order;

namespace BLL.Model.DTO.Order;

public class MarketplaceOrderView
{
    public int Id { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }
    public int AddressId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    public string? Note { get; set; }
}