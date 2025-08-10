using DAL.Repository.DTO;

namespace BLL.Service.Model.DTO.Cart;

public class CartItemDTO
{
    public int ProductId { get; set; }
    
    public int Quantity  { get; set; }
}