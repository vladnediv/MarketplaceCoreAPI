namespace Domain.Model.Cart;

public class CartItem
{
    public int Id { get; set; }
    
    public int Quantity { get; set; }
    
    public Product.Product Product { get; set; }
    public int ProductId { get; set; }
    
    public int CartId { get; set; }
    public Cart Cart { get; set; }
}