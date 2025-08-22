namespace BLL.Model.DTO.Category;

public class CategoryDTO
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int ParentCategoryId { get; set; }
    public List<CategoryDTO> Subcategories { get; set; }
}