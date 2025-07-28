using System.ComponentModel.DataAnnotations;
using Domain.Model.Product;

namespace DAL.Repository.DTO;

public class ProductCharacteristicDTO
{
    [Required]
    public string Name { get; set; }
    [Required]
    public List<KeyValueDTO> Characteristics { get; set; }
}