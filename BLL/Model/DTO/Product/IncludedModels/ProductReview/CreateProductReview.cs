using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BLL.Model.DTO.Product.IncludedModels.ProductReview;

public class CreateProductReview
{
    [JsonIgnore]
    public int UserId { get; set; }
    [Required]
    public string AuthorName { get; set; }
    [Required]
    public string Email { get; set; }
        
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    public string Description { get; set; }
    
    [Required]
    public DateOnly CreatedAt { get; set; }
    
    public List<ProductMediaDTO>? MediaFiles { get; set; }
    
    [Required]
    [DefaultValue(5)]
    [Range(0, 5)]
    public int Rating { get; set; }
}