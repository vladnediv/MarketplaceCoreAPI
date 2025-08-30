namespace BLL.Model.DTO.Order.IncludedModels;

public class ShopOrderItemView
{
    public string PictureUrl { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}