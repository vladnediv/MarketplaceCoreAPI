namespace BLL.Model.DTO.Product.IncludedModels.ProductQuestionAnswer;

public class ProductQuestionAnswerDTO
{
    public string AuthorName  { get; set; }
    public string AuthorId  { get; set; }
    public string Email { get; set; }
    public DateOnly CreatedAt { get; set; }
    public string Answer { get; set; }
}