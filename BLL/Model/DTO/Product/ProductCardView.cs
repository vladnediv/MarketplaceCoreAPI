using BLL.Model.DTO.Category;

namespace BLL.Model.DTO.Product;

public class ProductCardView
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? PictureUrl { get; set; }
    public int Rating { get; set; }
    public int CommentsCount { get; set; }
    public decimal Price { get; set; }
    public decimal? DiscountValue { get; set; }
    public string CategoryName { get; set; }
}