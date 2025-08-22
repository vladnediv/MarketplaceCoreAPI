namespace BLL.Model.DTO.Category;

public class UpdateCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; }
}