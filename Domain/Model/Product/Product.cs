using Domain.Model.Order;

namespace Domain.Model.Product
{
    //TODO Add Category to Product
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }
        
        public int Stock { get; set; }

        public bool IsReviewed { get; set; }

        public bool IsGroup { get; set; }
        public int? GroupId { get; set; }

        public DateOnly CreatedAt { get; set; }

        public decimal? DiscountValue { get; set; }
        public int? DiscountPercent { get; set; }

        public int ProductBrandId { get; set; }

        public IEnumerable<ProductMedia>? MediaFiles { get; set; }

        public IEnumerable<ProductCharacteristic> Characteristics { get; set; }

        public IEnumerable<DeliveryOption> DeliveryOptions { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }

        public IEnumerable<ProductReview>? Reviews { get; set; }
        public IEnumerable<ProductQuestion>? Questions { get; set; }
    }
}