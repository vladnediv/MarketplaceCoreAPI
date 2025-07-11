namespace Domain.Model.Product;

public class CurrentVariation
{
    public int Id { get; set; }

    public int ProductId { get; set; }
    public Product Product { get; set; }

    public KeyValue Variation { get; set; }
}