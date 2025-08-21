namespace BLL.Service.Model.DTO.Category;

public class CRUDCategory
{
    public int? Id { get; set; }
    public string Name { get; set; }
    public int? ParentCategoryId { get; set; }
}