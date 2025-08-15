using System.ComponentModel.DataAnnotations;
using BLL.Service.Model.DTO.Order.IncludedModels;
using Domain.Model.Order;

namespace BLL.Service.Model.DTO.Order;

public class CreateOrder
{
    [Required]
    public int UserId { get; set; }
    [Required]
    public int AddressId { get; set; }
    [Required]
    public DateTime OrderDate { get; set; }
    [Required]
    public decimal TotalPrice { get; set; }
    [Required]
    public List<OrderItemDTO> OrderItems { get; set; }
}