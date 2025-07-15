using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Domain.Model.Product
{
    public class ProductReview
    {
        public int Id { get; set; }
        
        public string AuthorName { get; set; }
        public int UserId { get; set; }
        
        public int ProductId { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }
        public string Advantages { get; set; }
        public string Disadvantages { get; set; }
        
        public DateOnly CreatedAt { get; set; }
        
        public int Rating { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}