namespace Domain.Model.Product
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public bool IsReviewed { get; set; }
        public bool IsApproved { get; set; }
        public bool IsActive { get; set; }

        public DateOnly CreatedAt { get; set; }
        
        //In case if the shop is a distributor
        public string? BrandName { get; set; }

        public decimal? DiscountValue { get; set; }

        public int ProductBrandId { get; set; }

        public int CategoryId { get; set; }
        public Category.Category Category { get; set; }

        public IEnumerable<ProductMedia>? MediaFiles { get; set; }

        public IEnumerable<ProductCharacteristic> Characteristics { get; set; }

        public IEnumerable<DeliveryOption> ProductDeliveryOptions { get; set; }

        public IEnumerable<ProductReview>? Reviews { get; set; }
        public IEnumerable<ProductQuestion>? Questions { get; set; }
        
        public ProductStatus Status { get; set; }
    }

    public enum ProductStatus
    {
        Active,
        Draft,
        Awaiting,
        Deleted
    }
}