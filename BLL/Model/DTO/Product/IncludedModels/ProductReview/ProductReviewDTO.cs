namespace BLL.Model.DTO.Product.IncludedModels.ProductReview;

public class ProductReviewDTO
{
    public int Id { get; set; }
    public string AuthorName  { get; set; }

    public string Description { get; set; }
    
    public DateOnly CreatedAt  { get; set; }
    
    public List<ProductMediaDTO>? MediaFiles { get; set; }
    
    public int Rating { get; set; }
}