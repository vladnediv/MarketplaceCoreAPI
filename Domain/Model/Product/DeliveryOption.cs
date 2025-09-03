namespace Domain.Model.Product
{
    public class DeliveryOption
    {
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
    }

    public enum DeliveryStatus
    {
        Planned,
        OnTheWay,
        InDepartment,
        StoredForFee,
        Received,
        ReceiverRefused,
        ReturnOnTheWay,
        ReturnInDepartment
    }
}