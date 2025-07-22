using System.ComponentModel.DataAnnotations;

namespace DAL.Repository.DTO;

public class KeyValueDTO
{
    [Required]
    public string Key { get; set; }
    [Required]
    public string Value { get; set; }
}