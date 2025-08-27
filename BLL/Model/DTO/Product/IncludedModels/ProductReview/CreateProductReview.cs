using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BLL.Model.DTO.Product.IncludedModels.ProductReview;

public class CreateProductReview
{
    public int ProductId { get; set; }
    
    public string AuthorName { get; set; }
    [JsonIgnore]
    public int UserId { get; set; }
    public string Email { get; set; }

    public string Title { get; set; }
    public string Description { get; set; }
    public string Advantages { get; set; }
    public string Disadvantages { get; set; }
    
    public DateOnly CreatedAt { get; set; }
    
    [Range(0, 5)]
    public int Rating { get; set; }
}