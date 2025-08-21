using System.Linq.Expressions;
using DAL.Context;
using DAL.Repository.Interface;
using Domain.Model.Category;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.CategoryRepositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Category> _categories;

    public CategoryRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
        _categories = dbContext.Set<Category>();
    }
    
    public async Task<Category> GetByIdAsync(int id)
    {
        return await _categories
            .Include(x => x.ParentCategory)
            .Include(x => x.Subcategories)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Category entity)
    {
        await _categories.AddAsync(entity);
    }

    public async Task UpdateAsync(Category entity)
    {
        await Task.Factory.StartNew(() =>
        {
            _categories.Update(entity);
        });
    }

    public async Task DeleteAsync(Category entity)
    {
        await Task.Factory.StartNew(() =>
        {
            _categories.Remove(entity);
        });
    }

    public async Task DeleteByIdAsync(int id)
    {
        Category category = await _categories.FirstOrDefaultAsync(x => x.Id == id);
        await Task.Factory.StartNew(() =>
        {
            _categories.Remove(category);
        });
    }
    
    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _categories
            .Include(x => x.ParentCategory)
            .Include(x => x.Subcategories)
            .Include(x => x.Products)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetAllAsync(Expression<Func<Category, bool>> predicate)
    {
        return await _categories
            .Where(predicate)
            .Include(x => x.ParentCategory)
            .Include(x => x.Subcategories)
            .Include(x => x.Products)
            .ToListAsync();
    }

    public async Task<Category> FirstOrDefaultAsync(Expression<Func<Category, bool>> predicate)
    {
        return await _categories
            .Include(x => x.ParentCategory)
            .Include(x => x.Subcategories)
            .Include(x => x.Products)
            .FirstOrDefaultAsync(predicate);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    // Additional methods specific to Category
    public async Task<IEnumerable<Category>> GetRootCategoriesAsync()
    {
        return await _categories
            .Where(x => x.ParentCategoryId == null)
            .Include(x => x.Subcategories)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetSubcategoriesByParentIdAsync(int parentId)
    {
        return await _categories
            .Where(x => x.ParentCategoryId == parentId)
            .Include(x => x.Subcategories)
            .ToListAsync();
    }

    public async Task<IEnumerable<Category>> GetCategoryTreeAsync()
    {
        return await _categories
            .Where(x => x.ParentCategoryId == null)
            .Include(x => x.Subcategories)
            .ThenInclude(sc => sc.Subcategories)
            .ToListAsync();
    }
}