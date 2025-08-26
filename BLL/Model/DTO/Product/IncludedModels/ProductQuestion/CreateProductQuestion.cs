using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BLL.Model.DTO.Product.IncludedModels.ProductQuestion;

public class CreateProductQuestion
{
    [JsonIgnore]
    public int UserId { get; set; }
    public int ProductId { get; set; }
    public string AuthorName { get; set; }
    public string Question { get; set; }
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
    public List<ProductMediaDTO>? MediaFiles { get; set; }
    public string? VideoUrl { get; set; }
    public bool IsNotify { get; set; }
}