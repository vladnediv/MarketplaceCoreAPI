using BLL.Model.DTO.Product.IncludedModels.ProductQuestionAnswer;

namespace BLL.Model.DTO.Product.IncludedModels.ProductQuestion;

public class ProductQuestionDTO
{
    public int ProductId { get; set; }
    
    public string AuthorName  { get; set; }
    public string Email { get; set; }
    
    public string Question { get; set; }
    public string Id { get; set; }
    
    public List<ProductMediaDTO>? MediaFiles { get; set; }
    
    public List<ProductQuestionAnswerDTO>? Answers { get; set; }
}