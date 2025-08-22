using BLL.Model.DTO.Category;
using BLL.Model.DTO.Product.IncludedModels;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestion;
using BLL.Model.DTO.Product.IncludedModels.ProductReview;

namespace BLL.Model.DTO.Product;

public class AdminProductView
{
    public int Id { get; set; }
    public int ProductBrandId { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public bool IsReviewed { get; set; }
    public bool IsApproved { get; set; }
    public bool IsActive { get; set; }
    public bool IsGroup { get; set; }
    public int? GroupId { get; set; }
    public DateOnly CreatedAt { get; set; }
    public decimal? DiscountValue { get; set; }
    public CategoryDTO Category { get; set; }
    public List<ProductMediaDTO>? MediaFiles { get; set; }
    public List<ProductCharacteristicDTO> Characteristics { get; set; }
    public List<DeliveryOptionDTO> ProductDeliveryOptions { get; set; }
    public List<ProductReviewDTO>? Reviews { get; set; }
    public List<ProductQuestionDTO>? Questions { get; set; }
}