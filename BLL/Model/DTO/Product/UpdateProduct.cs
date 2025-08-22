using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BLL.Model.DTO.Product.IncludedModels;

namespace BLL.Model.DTO.Product;

public class UpdateProduct
{
    [Required]
    public int Id { get; set; }
    
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; }
    
    [Required]
    [DefaultValue(0)]
    public decimal Price { get; set; }
    
    [Required]
    [DefaultValue(0)]
    public int Stock { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
    
    [DefaultValue(0)]
    public decimal? DiscountValue { get; set; }
    
    public List<ProductMediaDTO>? MediaFiles { get; set; }
    
    [Required]
    public List<ProductCharacteristicDTO> Characteristics { get; set; }
}