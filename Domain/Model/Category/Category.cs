using System.ComponentModel.DataAnnotations;

namespace Domain.Model.Category
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? ParentCategoryId { get; set; }
        public Category? ParentCategory { get; set; }

        public ICollection<Category> Subcategories { get; set; } = new List<Category>();

        public ICollection<Product.Product> Products { get; set; } = new List<Product.Product>();
    }
}

