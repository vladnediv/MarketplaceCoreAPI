using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BLL.Model.DTO.Order.IncludedModels;
using Domain.Model.Order;

namespace BLL.Model.DTO.Order;

public class CreateOrder
{
    [JsonIgnore]
    public int UserId { get; set; }
    [Required]
    public int AddressId { get; set; }
    [Required]
    public DateTime OrderDate { get; set; }
    [Required]
    public decimal TotalPrice { get; set; }
    [Required]
    public List<OrderItemDTO> OrderItems { get; set; }
    public PaymentMethod PaymentMethod { get; set; }
    [DefaultValue("")]
    public string? Note { get; set; }
}