using Domain.Model.Product;

namespace BLL.Model.DTO.Product.IncludedModels.ProductReview;

public class ShopProductReviewView
{
    public int Id { get; set; }
    
    //personal data
    public int UserId { get; set; }
    public string AuthorName { get; set; }
    public string Email { get; set; }
    
    public string ProductName  { get; set; }
    
    public string Description { get; set; }
    
    public DateOnly CreatedAt { get; set; }
    
    public List<ProductMediaDTO> MediaFiles { get; set; }
    
    public int Rating { get; set; }
}