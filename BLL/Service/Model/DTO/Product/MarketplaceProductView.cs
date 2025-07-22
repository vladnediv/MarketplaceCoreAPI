namespace DAL.Repository.DTO;

public class MarketplaceProductView
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsGroup { get; set; }
    public int? GroupId { get; set; }
    public DateOnly CreatedAt { get; set; }
    public decimal? DiscountValue { get; set; }
    public int ProductBrandId { get; set; }
    public List<ProductMediaDTO> MediaFiles { get; set; }
    public List<ProductCharacteristicDTO> Characteristics { get; set; }
    public List<DeliveryOptionDTO> DeliveryOptions { get; set; }
    public List<ProductReviewDTO> Reviews { get; set; }
    public List<ProductQuestionDTO> Questions { get; set; }
}