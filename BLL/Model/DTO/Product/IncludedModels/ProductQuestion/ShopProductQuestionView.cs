using BLL.Model.DTO.Product.IncludedModels.ProductQuestionAnswer;

namespace BLL.Model.DTO.Product.IncludedModels.ProductQuestion;

public class ShopProductQuestionView
{
    public int Id { get; set; }
    
    public string AuthorName { get; set; }
    public string Email { get; set; }
    public int UserId { get; set; }
    
    public int ProductId { get; set; }
    public ShopProductView Product { get; set; }
    
    public string Question { get; set; }
    
    public List<ProductMediaDTO>? MediaFiles { get; set; }
    
    public bool IsNotify { get; set; }
    
    public List<ProductQuestionAnswerDTO>? Answers { get; set; }
    public DateOnly CreatedAt { get; set; }
}