namespace BLL.Model.DTO.Category;

public class CreateCategory
{
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; }
}