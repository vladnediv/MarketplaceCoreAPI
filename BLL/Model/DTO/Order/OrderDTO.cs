using BLL.Service.Model.DTO.Order.IncludedModels;
using Domain.Model.Order;

namespace BLL.Model.DTO.Order;

public class OrderDTO
{
    public int Id { get; set; }
    public List<OrderItemDTO> OrderItems { get; set; }
    public int UserId { get; set; }
    public int AddressId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public OrderStatus Status { get; set; }
}