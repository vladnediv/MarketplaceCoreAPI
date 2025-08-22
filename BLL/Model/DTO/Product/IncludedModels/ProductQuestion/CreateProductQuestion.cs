namespace BLL.Model.DTO.Product.IncludedModels.ProductQuestion;

public class CreateProductQuestion
{
    public int UserId { get; set; }
    public string AuthorName { get; set; }
    public string Question { get; set; }
    public List<string>? PhotoUrls { get; set; }
    public string? VideoUrl { get; set; }
    public bool IsNotify { get; set; }
}