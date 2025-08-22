namespace BLL.Model.DTO.Product.IncludedModels.ProductReview;

public class ProductReviewDTO
{
    public int ProductId { get; set; }
    
    public string AuthorName  { get; set; }
    
    public string Title { get; set; }
    public string Description { get; set; }
    public string Advantages { get; set; }
    public string Disadvantages { get; set; }
    
    public DateOnly CreatedAt  { get; set; }
    
    public int Rating { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
}