using System.Linq.Expressions;
using DAL.Context;
using DAL.Repository.Interface;
using Domain.Model.Product;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class ProductReviewRepository : IAdvancedRepository<ProductReview>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<ProductReview> _productReviews;

    public ProductReviewRepository(ApplicationDbContext context)
    {
        _context = context;
        _productReviews = _context.Set<ProductReview>();
    }

    public async Task<ProductReview> GetByIdAsync(int id)
    {
        return await _productReviews.FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task AddAsync(ProductReview entity)
    {
        await _productReviews.AddAsync(entity);
    }

    public async Task UpdateAsync(ProductReview entity)
    {
        await Task.Factory.StartNew(() =>
        {
            _productReviews.Update(entity);
        });
    }

    public async Task DeleteAsync(ProductReview entity)
    {
        await Task.Factory.StartNew(() =>
        {
            _productReviews.Remove(entity);
        });
    }

    public async Task DeleteByIdAsync(int id)
    {
        ProductReview review = await _productReviews.FirstOrDefaultAsync(r => r.Id == id);

            await Task.Factory.StartNew(() =>
            {
                _productReviews.Remove(review);
            });
        
    }

    public async Task<IEnumerable<ProductReview>> GetAllAsync()
    {
        return await _productReviews.ToListAsync();
    }

    public async Task<IEnumerable<ProductReview>> GetAllAsync(Expression<Func<ProductReview, bool>> predicate)
    {
        return await _productReviews.Where(predicate).ToListAsync();
    }

    public async Task<ProductReview> FirstOrDefaultAsync(Expression<Func<ProductReview, bool>> predicate)
    {
        return await _productReviews.FirstOrDefaultAsync(predicate);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}