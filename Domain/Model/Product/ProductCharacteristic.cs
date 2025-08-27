namespace Domain.Model.Product
{
    public class ProductCharacteristic
    {
        public int Id { get; set; }

        public int ProductId { get; set; }
        public Product Product { get; set; }

        public string? Name { get; set; }
        public IEnumerable<KeyValue> Characteristics { get; set; }
    }
}