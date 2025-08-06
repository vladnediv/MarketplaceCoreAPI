namespace Domain.Model.Cart;

public class Cart
{
    public int Id { get; set; }
    public IEnumerable<CartItem> CartItems { get; set; }
    public int UserId { get; set; }
}