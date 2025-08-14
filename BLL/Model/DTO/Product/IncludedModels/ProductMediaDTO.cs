using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Domain.Model.Product;
using Microsoft.AspNetCore.Http;

namespace DAL.Repository.DTO;

public class ProductMediaDTO
{
    [Required]
    [DefaultValue(0)]
    public MediaType MediaType { get; set; }
    [DefaultValue("")]
    public string? Url { get; set; }
    public byte[]? MediaContent { get; set; }
    public IFormFile? File { get; set; }
}