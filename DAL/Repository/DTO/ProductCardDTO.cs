using Domain.Model.Product;

namespace DAL.Repository.DTO;

public class ProductCardDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string PictureUrl { get; set; }
    public int Rating { get; set; }
    public int CommentsCount { get; set; }
    public decimal Price { get; set; }
}