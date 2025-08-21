using BLL.Model;
using BLL.Service.Model;
using Domain.Model.Category;

namespace BLL.Service.Interface.BasicInterface;

public interface ICategoryService : IAdvancedService<Category>
{
    public Task<ServiceResponse<Category>> GetRootCategoriesAsync();
    public Task<ServiceResponse<Category>> GetSubcategoriesByParentIdAsync(int parentId);
    public Task<ServiceResponse<Category>> GetCategoryTreeAsync();
}