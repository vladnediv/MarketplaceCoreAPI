namespace Domain.Model.Product
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public bool IsReviewed { get; set; }

        public bool IsGroup { get; set; }
        public int? GroupId { get; set; }

        public DateOnly CreatedAt { get; set; }

        public decimal? DiscountValue { get; set; }
        public int? DiscountPercent { get; set; }

        public int ProductBrandId { get; set; }

        public IEnumerable<string>? PhotoUrls { get; set; }
        public IEnumerable<string>? VideoUrls { get; set; }

        public IEnumerable<ProductAttribute> Descriptions { get; set; }
        public IEnumerable<ProductVariation>? Variations { get; set; }
        public IEnumerable<CurrentVariation>? CurrentVariation { get; set; }

        public IEnumerable<DeliveryOption> DeliveryOptions { get; set; }

        public IEnumerable<ProductReview>? Reviews { get; set; }
        public IEnumerable<ProductQuestion>? Questions { get; set; }
    }
}