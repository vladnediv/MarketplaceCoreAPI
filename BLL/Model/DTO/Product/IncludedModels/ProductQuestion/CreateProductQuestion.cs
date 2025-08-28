using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BLL.Model.DTO.Product.IncludedModels.ProductQuestion;

public class CreateProductQuestion
{
    [JsonIgnore]
    public int UserId { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    [Required]
    public string AuthorName { get; set; }
    
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    public string Question { get; set; }
    
    public List<ProductMediaDTO>? MediaFiles { get; set; }
    
    [Required]
    [DefaultValue(false)]
    public bool IsNotify { get; set; }
}