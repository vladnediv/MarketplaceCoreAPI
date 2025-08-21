using System.Linq.Expressions;
using BLL.Model;
using BLL.Service.Interface.BasicInterface;
using BLL.Service.Model;
using DAL.Repository.Interface;
using Domain.Model.Category;

namespace BLL.Service;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    
    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }


    public async Task<ServiceResponse<Category>> GetAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Category>> CreateAsync(Category entity)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Category>> UpdateAsync(Category entity)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Category>> DeleteAsync(Category entity)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Category>> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Category>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Category>> GetAllAsync(Expression<Func<Category, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Category>> FirstOrDefaultAsync(Expression<Func<Category, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Category>> GetRootCategoriesAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Category>> GetSubcategoriesByParentIdAsync(int parentId)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Category>> GetCategoryTreeAsync()
    {
        throw new NotImplementedException();
    }
}