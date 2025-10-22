using System.Linq.Expressions;
using DAL.Context;
using DAL.Repository.Interface;
using Domain.Model.Product;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.ProductRepositories;

public class ProductQuestionAnswerRepository : IAdvancedRepository<ProductQuestionAnswer>
{
    private readonly ApplicationDbContext _dbContext;
    private readonly DbSet<ProductQuestionAnswer> _dbSet;

    public ProductQuestionAnswerRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<ProductQuestionAnswer>();
    }

    public async Task<ProductQuestionAnswer> GetByIdAsync(int id)
    {
        return await _dbSet.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AddAsync(ProductQuestionAnswer entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public async Task UpdateAsync(ProductQuestionAnswer entity)
    {
        await Task.Factory.StartNew(() => _dbSet.Update(entity));
    }

    public async Task DeleteAsync(ProductQuestionAnswer entity)
    {
        await Task.Factory.StartNew(() => _dbSet.Remove(entity));
    }

    public async Task DeleteByIdAsync(int id)
    {
        ProductQuestionAnswer answer = await _dbSet.FirstOrDefaultAsync(a => a.Id == id);
        
            await Task.Factory.StartNew(() => _dbSet.Remove(answer));
        
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductQuestionAnswer>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<ProductQuestionAnswer>> GetAllAsync(Expression<Func<ProductQuestionAnswer, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task<ProductQuestionAnswer> FirstOrDefaultAsync(Expression<Func<ProductQuestionAnswer, bool>> predicate)
    {
        return await _dbSet.FirstOrDefaultAsync(predicate);
    }
}