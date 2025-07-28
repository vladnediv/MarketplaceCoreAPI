namespace DAL.Repository.DTO;

public class ProductQuestionAnswerDTO
{
    public string AuthorName  { get; set; }
    public DateOnly CreatedAt { get; set; }
    public string Answer { get; set; }
}