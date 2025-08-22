using Domain.Model.Category;

namespace DAL.Repository.Interface;

public interface ICategoryRepository : IAdvancedRepository<Category>
{
    public Task<IEnumerable<Category>> GetRootCategoriesAsync();
    
    public Task<IEnumerable<Category>> GetSubcategoriesByParentIdAsync(int parentId);
    public Task<IEnumerable<Category>> GetCategoryTreeAsync();
}