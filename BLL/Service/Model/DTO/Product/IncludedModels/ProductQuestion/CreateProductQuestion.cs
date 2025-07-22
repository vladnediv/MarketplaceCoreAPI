namespace DAL.Repository.DTO;

public class CreateProductQuestion
{
    public string AuthorName { get; set; }
    public string Question { get; set; }
    public List<string>? PhotoUrls { get; set; }
    public string? VideoUrl { get; set; }
    public bool IsNotify { get; set; }
}