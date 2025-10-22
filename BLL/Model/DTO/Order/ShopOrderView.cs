using BLL.Model.DTO.Order.IncludedModels;
using Domain.Model.Order;

namespace BLL.Model.DTO.Order;

public class ShopOrderView
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public int AddressId { get; set; }
    
    public List<ShopOrderItemView> OrderItems { get; set; }
    
    public DateTime OrderDate { get; set; }
    
    public decimal TotalPrice { get; set; }
    
    public OrderStatus Status { get; set; }
    
    public PaymentMethod PaymentMethod { get; set; }
    
    public string? Note { get; set; }
}