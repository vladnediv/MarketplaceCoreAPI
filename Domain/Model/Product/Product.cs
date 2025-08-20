using Domain.Model.Order;
using Domain.Model.Category;

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

        public bool IsGroup { get; set; }
        public int? GroupId { get; set; }

        public DateOnly CreatedAt { get; set; }

        public decimal? DiscountValue { get; set; }

        public int ProductBrandId { get; set; }

        public int? CategoryId { get; set; }
        public Category.Category? Category { get; set; }

        public IEnumerable<ProductMedia>? MediaFiles { get; set; }

        public IEnumerable<ProductCharacteristic> Characteristics { get; set; }

        public IEnumerable<ProductDeliveryOption> ProductDeliveryOptions { get; set; }

        public IEnumerable<ProductReview>? Reviews { get; set; }
        public IEnumerable<ProductQuestion>? Questions { get; set; }
    }
}