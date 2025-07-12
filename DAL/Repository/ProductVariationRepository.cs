using System.Linq.Expressions;
using DAL.Context;
using DAL.Repository.Interface;
using Domain.Model.Product;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class ProductVariationRepository : IAdvancedRepository<ProductVariation>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<ProductVariation> _productVariations;

    public ProductVariationRepository(ApplicationDbContext context)
    {
        _context = context;
        _productVariations = _context.Set<ProductVariation>();
    }

    public async Task<ProductVariation> GetByIdAsync(int id)
    {
        return await _productVariations.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(ProductVariation entity)
    {
        await _productVariations.AddAsync(entity);
    }

    public async Task UpdateAsync(ProductVariation entity)
    {
        await Task.Factory.StartNew(() => _productVariations.Update(entity));
    }

    public async Task DeleteAsync(ProductVariation entity)
    {
        await Task.Factory.StartNew(() => _productVariations.Remove(entity));
    }

    public async Task DeleteByIdAsync(int id)
    {
        ProductVariation? variation = await _productVariations.FirstOrDefaultAsync(x => x.Id == id);
        if (variation != null)
        {
            await Task.Factory.StartNew(() => _productVariations.Remove(variation));
        }
    }

    public async Task<IEnumerable<ProductVariation>> GetAllAsync()
    {
        return await _productVariations.ToListAsync();
    }

    public async Task<IEnumerable<ProductVariation>> GetAllAsync(Expression<Func<ProductVariation, bool>> predicate)
    {
        return await _productVariations.Where(predicate).ToListAsync();
    }

    public async Task<ProductVariation> FirstOrDefaultAsync(Expression<Func<ProductVariation, bool>> predicate)
    {
        return await _productVariations.FirstOrDefaultAsync(predicate);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}