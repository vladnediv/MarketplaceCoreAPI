using System.Linq.Expressions;
using DAL.Context;
using DAL.Repository.Interface;
using Domain.Model.Product;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.ProductRepositories;

public class ProductQuestionRepository : IAdvancedRepository<ProductQuestion>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<ProductQuestion> _productQuestions;

    public ProductQuestionRepository(ApplicationDbContext context)
    {
        _context = context;
        _productQuestions = _context.Set<ProductQuestion>();
    }

    public async Task<ProductQuestion> GetByIdAsync(int id)
    {
        return await _productQuestions.FirstOrDefaultAsync(q => q.Id == id);
    }

    public async Task AddAsync(ProductQuestion entity)
    {
        await _productQuestions.AddAsync(entity);
    }

    public async Task UpdateAsync(ProductQuestion entity)
    {
        await Task.Factory.StartNew(() =>
        {
            _productQuestions.Update(entity);
        });
    }

    public async Task DeleteAsync(ProductQuestion entity)
    {
        await Task.Factory.StartNew(() =>
        {
            _productQuestions.Remove(entity);
        });
    }

    public async Task DeleteByIdAsync(int id)
    {
        ProductQuestion question = await _productQuestions.FirstOrDefaultAsync(q => q.Id == id);

            await Task.Factory.StartNew(() =>
            {
                _productQuestions.Remove(question);
            });
        
    }

    public async Task<IEnumerable<ProductQuestion>> GetAllAsync()
    {
        return await _productQuestions.ToListAsync();
    }

    public async Task<IEnumerable<ProductQuestion>> GetAllAsync(Expression<Func<ProductQuestion, bool>> predicate)
    {
        return await _productQuestions.Where(predicate).ToListAsync();
    }

    public async Task<ProductQuestion> FirstOrDefaultAsync(Expression<Func<ProductQuestion, bool>> predicate)
    {
        return await _productQuestions.FirstOrDefaultAsync(predicate);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}   