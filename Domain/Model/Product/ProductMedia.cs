namespace Domain.Model.Product;

public class ProductMedia
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    public Product Product { get; set; }
    
    
    public int? ProductReviewId { get; set; }
    public ProductReview? ProductReview { get; set; }
    
    
    public int? ProductQuestionId { get; set; }
    public ProductQuestion? ProductQuestion { get; set; }
    
    public MediaType MediaType { get; set; }
    
    public string Url { get; set; }
}

public enum MediaType
{
    Image,
    Video
}