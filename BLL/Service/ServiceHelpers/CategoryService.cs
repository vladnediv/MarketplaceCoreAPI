using System.Linq.Expressions;
using BLL.Model;
using BLL.Model.Constants;
using BLL.Service.Interface.BasicInterface;
using DAL.Repository.Interface;
using Domain.Model.Category;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace BLL.Service.ServiceHelpers;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    
    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }


    public async Task<ServiceResponse<Category>> GetAsync(int id)
    {
        ServiceResponse<Category> response = new ServiceResponse<Category>();
        try
        {
            Category entity = await _categoryRepository.GetByIdAsync(id);
            if (entity != null)
            {
                response.IsSuccess = true;
                response.Entity = entity;
                return response;
            }
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(Category), id);
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<Category>> CreateAsync(Category entity)
    {
        ServiceResponse<Category> response = new ServiceResponse<Category>();

        if (entity == null)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(entity), nameof(Category));
            return response;
        }

        if (string.IsNullOrWhiteSpace(entity.Name))
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(entity.Name), nameof(Category));
            return response;
        }
        // Disallow leading/trailing spaces; enforce trimmed name
        var trimmedName = entity.Name.Trim();
        if (trimmedName.Length == 0 || trimmedName != entity.Name)
        {
            response.IsSuccess = false;
            response.Message = "Category name must not be empty and must not have leading or trailing spaces.";
            return response;
        }

        // Validate parent category if provided
        if (entity.ParentCategoryId.HasValue)
        {
            var parent = await _categoryRepository.GetByIdAsync(entity.ParentCategoryId.Value);
            if (parent == null)
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(Category), entity.ParentCategoryId.Value);
                return response;
            }
        }

        // Prevent duplicates within the same parent scope
        // Global uniqueness per user requirements
        var duplicate = await _categoryRepository.FirstOrDefaultAsync(
            x => x.Name == trimmedName && x.ParentCategoryId == entity.ParentCategoryId);
        if (duplicate != null)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.AlreadyExists(entity.Name, nameof(Category));
            return response;
        }

        try
        {
            entity.Name = trimmedName;
            await _categoryRepository.AddAsync(entity);
            await _categoryRepository.SaveChangesAsync();
            response.IsSuccess = true;
            response.Entity = entity;
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<Category>> UpdateAsync(Category entity)
    {
        ServiceResponse<Category> response = new ServiceResponse<Category>();

        if (entity == null)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(entity), nameof(Category));
            return response;
        }

        if (entity.Id <= 0)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(entity.Id), nameof(Category));
            return response;
        }

        if (string.IsNullOrWhiteSpace(entity.Name))
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(entity.Name), nameof(Category));
            return response;
        }
        // Disallow leading/trailing spaces; enforce trimmed name
        var trimmedName = entity.Name.Trim();
        if (trimmedName.Length == 0 || trimmedName != entity.Name)
        {
            response.IsSuccess = false;
            response.Message = "Category name must not be empty and must not have leading or trailing spaces.";
            return response;
        }

        // Prevent setting itself as a parent
        if (entity.ParentCategoryId.HasValue && entity.ParentCategoryId.Value == entity.Id)
        {
            response.IsSuccess = false;
            response.Message = "A category cannot be the parent of itself.";
            return response;
        }

        // Ensure category exists
        var existing = await _categoryRepository.GetByIdAsync(entity.Id);
        if (existing == null)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(Category), entity.Id);
            return response;
        }

        // Validate parent if provided
        if (entity.ParentCategoryId.HasValue && entity.ParentCategoryId.Value != 0)
        {
            var parent = await _categoryRepository.GetByIdAsync(entity.ParentCategoryId.Value);
            if (parent == null)
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(Category), entity.ParentCategoryId.Value);
                return response;
            }
            existing.ParentCategoryId = entity.ParentCategoryId;
        }
        else
        {
            existing.ParentCategoryId = null;
            existing.ParentCategory = null;
        }

        // Prevent duplicate names in the same parent scope (excluding itself)
        // Global uniqueness per user requirements
        var duplicate = await _categoryRepository.FirstOrDefaultAsync(
            x => x.Id != entity.Id && x.Name == trimmedName && x.ParentCategoryId == entity.ParentCategoryId);
        if (duplicate != null)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.AlreadyExists(entity.Name, nameof(Category));
            return response;
        }

        try
        {
            // entity.Name = trimmedName;
            existing.Name = trimmedName;
            
            await _categoryRepository.UpdateAsync(existing);
            await _categoryRepository.SaveChangesAsync();
            response.IsSuccess = true;
            response.Entity = entity;
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<Category>> DeleteAsync(Category entity)
    {
        ServiceResponse<Category> response = new ServiceResponse<Category>();

        if (entity == null)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(entity), nameof(Category));
            return response;
        }

        try
        {
            await DeleteSubcategoriesRecursive(entity);
            await _categoryRepository.DeleteAsync(entity);
            await _categoryRepository.SaveChangesAsync();
            response.IsSuccess = true;
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<Category>> DeleteByIdAsync(int id)
    {
        var response = new ServiceResponse<Category>();
        try
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(Category), id);
                return response;
            }

            await DeleteSubcategoriesRecursive(category);
            await _categoryRepository.DeleteByIdAsync(id);
            await _categoryRepository.SaveChangesAsync();
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }
    
    private async Task DeleteSubcategoriesRecursive(Category category)
    {
        foreach (var subcategory in category.Subcategories.ToList())
        {
            await DeleteSubcategoriesRecursive(subcategory);
            await _categoryRepository.DeleteAsync(subcategory);
        }
    }
    
    public async Task<ServiceResponse<Category>> GetAllAsync()
    {
        ServiceResponse<Category> response = new ServiceResponse<Category>();

        try
        {
            IEnumerable<Category> entities = await _categoryRepository.GetAllAsync();

            if (entities != null)
            {
                response.IsSuccess = true;
                response.Entities = entities.ToList();
                return response;
            }

            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UnknownError;
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<Category>> GetAllAsync(Expression<Func<Category, bool>> predicate)
    {
        ServiceResponse<Category> response = new ServiceResponse<Category>();

        try
        {
            IEnumerable<Category> entities = await _categoryRepository.GetAllAsync(predicate);

            if (entities != null)
            {
                response.IsSuccess = true;
                response.Entities = entities.ToList();
                return response;
            }

            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UnknownError;
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<Category>> FirstOrDefaultAsync(Expression<Func<Category, bool>> predicate)
    {
        ServiceResponse<Category> response = new ServiceResponse<Category>();

        try
        {
            Category entity = await _categoryRepository.FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                response.IsSuccess = true;
                response.Entity = entity;
                return response;
            }
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UnknownError;
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<Category>> GetRootCategoriesAsync()
    {
        ServiceResponse<Category> response = new ServiceResponse<Category>();
        try
        {
            var entities = await _categoryRepository.GetRootCategoriesAsync();
            response.IsSuccess = true;
            response.Entities = entities?.ToList();
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<Category>> GetSubcategoriesByParentIdAsync(int parentId)
    {
        ServiceResponse<Category> response = new ServiceResponse<Category>();

        if (parentId <= 0)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(parentId), nameof(Category));
            return response;
        }

        try
        {
            var parent = await _categoryRepository.GetByIdAsync(parentId);
            if (parent == null)
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(Category), parentId);
                return response;
            }

            var entities = await _categoryRepository.GetSubcategoriesByParentIdAsync(parentId);
            response.IsSuccess = true;
            response.Entities = entities?.ToList();
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<Category>> GetCategoryTreeAsync()
    {
        ServiceResponse<Category> response = new ServiceResponse<Category>();
        try
        {
            var entities = await _categoryRepository.GetCategoryTreeAsync();
            response.IsSuccess = true;
            response.Entities = entities?.ToList();
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }
}