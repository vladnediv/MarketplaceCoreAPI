namespace DAL.Repository.DTO;

public class ProductQuestionDTO
{
    public int ProductId { get; set; }
    
    public string AuthorName  { get; set; }
    
    public string Question { get; set; }
    
    public List<string>? PhotoUrls { get; set; }
    public string? VideoUrl { get; set; }
    
    public List<ProductQuestionAnswerDTO>? Answers { get; set; }
}