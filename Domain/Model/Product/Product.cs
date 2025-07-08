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

        public List<string>? PhotoUrls { get; set; }
        public List<string>? VideoUrls { get; set; }

        public List<ProductAttribute> Descriptions { get; set; }
        public List<ProductVariation>? Variations { get; set; }

        public List<DeliveryOption> DeliveryOptions { get; set; }

        public List<ProductReview>? Reviews { get; set; }
        public List<ProductQuestion>? Questions { get; set; }
    }
}