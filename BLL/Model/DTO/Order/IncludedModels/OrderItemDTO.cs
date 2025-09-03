using Domain.Model.Product;

namespace BLL.Model.DTO.Order.IncludedModels;

public class OrderItemDTO
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    
    public DeliveryStatus DeliveryStatus { get; set; }
}