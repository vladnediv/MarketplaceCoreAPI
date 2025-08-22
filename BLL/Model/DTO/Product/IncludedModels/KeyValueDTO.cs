using System.ComponentModel.DataAnnotations;

namespace BLL.Model.DTO.Product.IncludedModels;

public class KeyValueDTO
{
    [Required]
    public string Key { get; set; }
    [Required]
    public string Value { get; set; }
}