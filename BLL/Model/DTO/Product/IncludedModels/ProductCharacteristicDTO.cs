using System.ComponentModel.DataAnnotations;

namespace BLL.Model.DTO.Product.IncludedModels;

public class ProductCharacteristicDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public List<KeyValueDTO> Characteristics { get; set; }
}