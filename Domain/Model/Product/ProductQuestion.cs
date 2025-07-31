using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        
        public IEnumerable<string>? PhotoUrls { get; set; }
        public string? VideoUrl { get; set; }
        
        public bool IsNotify { get; set; }
        public IEnumerable<ProductQuestionAnswer>? Answers { get; set; }
    }
}
