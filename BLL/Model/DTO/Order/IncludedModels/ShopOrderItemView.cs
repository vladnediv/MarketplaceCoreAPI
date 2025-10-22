using Domain.Model.Product;

namespace BLL.Model.DTO.Order.IncludedModels;

public class ShopOrderItemView
{
    public int Id { get; set; }
    public string PictureUrl { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public DeliveryStatus DeliveryStatus { get; set; }
}