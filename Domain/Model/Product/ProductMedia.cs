using System.Text.Json.Serialization;

namespace Domain.Model.Product;

public class ProductMedia
{
    public int Id { get; set; }
    
    public int ProductId { get; set; }
    
    [JsonIgnore]
    public Product? Product { get; set; }
    
    public MediaType MediaType { get; set; }
    
    public string Url { get; set; }
}

public enum MediaType
{
    Image,
    Video
}