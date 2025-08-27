namespace Domain.Model.Product
{
    public class ProductQuestion
    {
        public int Id { get; set; }
        public string AuthorName { get; set; }
        public string Email { get; set; }
        public int UserId { get; set; }
        
        public bool IsApproved { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        
        public string Question { get; set; }
        
        public IEnumerable<ProductMedia>? MediaFiles { get; set; }
        public string? VideoUrl { get; set; }
        
        public bool IsNotify { get; set; }
        public IEnumerable<ProductQuestionAnswer>? Answers { get; set; }
    }
}