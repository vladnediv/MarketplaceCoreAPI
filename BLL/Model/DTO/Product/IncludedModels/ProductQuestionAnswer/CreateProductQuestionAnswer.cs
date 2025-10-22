using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BLL.Model.DTO.Product.IncludedModels.ProductQuestionAnswer;

public class CreateProductQuestionAnswer
{
    [Required]
    public int QuestionId { get; set; }
    [JsonIgnore]
    public int AuthorId { get; set; }
    [Required]
    public string Answer { get; set; }
    [Required]
    public string AuthorName  { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; }
}