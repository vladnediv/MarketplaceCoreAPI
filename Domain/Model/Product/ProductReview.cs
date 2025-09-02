namespace Domain.Model.Product
{
    public class ProductReview
    {
        public int Id { get; set; }

        
        //personal data
        public int UserId { get; set; }
        public string AuthorName { get; set; }
        public string Email { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        
        public string Description { get; set; }
        
        
        public DateOnly CreatedAt { get; set; }
        
        public bool IsReviewed { get; set; }
        public bool IsApproved { get; set; }
        
        
        public IEnumerable<ProductMedia>? MediaFiles { get; set; }
        
        public int Rating { get; set; }
    }
}