using System.Text.Json.Serialization;

namespace Domain.Model.Product;

public class ProductQuestionAnswer
{
    public int Id { get; set; }
    public int QuestionId { get; set; }
    [JsonIgnore]
    public ProductQuestion? Question { get; set; }
    public string AuthorName { get; set; }
    public int AuthorId { get; set; }
    public DateOnly CreatedAt { get; set; }
    public string Answer { get; set; }
}