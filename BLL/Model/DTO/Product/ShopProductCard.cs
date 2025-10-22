namespace BLL.Model.DTO.Product;

public class ShopProductCard
{
    public int Id { get; set; }
    public string PhotoUrl { get; set; }
    public string Name { get; set; }
    public string? BrandName { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public string CategoryName { get; set; }
    public decimal? DiscountValue { get; set; }
    public List<string> ProductDeliveryOptions { get; set; }
    public DateOnly CreatedAt { get; set; }
    public bool IsReviewed { get; set; }
}