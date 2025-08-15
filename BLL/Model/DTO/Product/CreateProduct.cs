using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using Domain.Model.Product;

namespace DAL.Repository.DTO;

public class CreateProduct
{
    [JsonIgnore]
    public int ProductBrandId { get; set; }
    [Required]
    [StringLength(70, MinimumLength = 2)]
    public string Name { get; set; }
    
    [Required]
    [DefaultValue(0)]
    public decimal Price { get; set; }
    
    [Required]
    [DefaultValue(0)]
    public int Stock { get; set; }
    
    [Required]
    public DateOnly CreatedAt { get; set; }
    
    [DefaultValue(0)]
    public decimal? DiscountValue { get; set; }
    
    public List<ProductMediaDTO>? MediaFiles { get; set; }
    
    [Required]
    public List<ProductCharacteristicDTO> Characteristics { get; set; }
    
    [Required]
    public List<int> DeliveryOptionIds { get; set; }
}