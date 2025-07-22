using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Model.Product;

namespace DAL.Repository.DTO;

public class ProductMediaDTO
{
    [Required]
    [DefaultValue(0)]
    public MediaType MediaType { get; set; }
    [Required]
    public string Url { get; set; }
}