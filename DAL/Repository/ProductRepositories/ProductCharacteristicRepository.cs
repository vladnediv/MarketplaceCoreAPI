using System.Linq.Expressions;
using DAL.Context;
using DAL.Repository.Interface;
using Domain.Model.Product;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.ProductRepositories;

public class ProductCharacteristicRepository : IAdvancedRepository<ProductCharacteristic>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<ProductCharacteristic> _productCharacteristics;

    public ProductCharacteristicRepository(ApplicationDbContext context)
    {
        _context = context;
        _productCharacteristics = _context.Set<ProductCharacteristic>();
    }

    public async Task<ProductCharacteristic> GetByIdAsync(int id)
    {
        return await _productCharacteristics.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(ProductCharacteristic entity)
    {
        await _productCharacteristics.AddAsync(entity);
    }

    public async Task UpdateAsync(ProductCharacteristic entity)
    {
        await Task.Factory.StartNew(() => _productCharacteristics.Update(entity));
    }

    public async Task DeleteAsync(ProductCharacteristic entity)
    {
        await Task.Factory.StartNew(() => _productCharacteristics.Remove(entity));
    }

    public async Task DeleteByIdAsync(int id)
    {
        ProductCharacteristic entity = await GetByIdAsync(id);

            await Task.Factory.StartNew(() => _productCharacteristics.Remove(entity));
        
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ProductCharacteristic>> GetAllAsync()
    {
        return await _productCharacteristics.ToListAsync();
    }

    public async Task<IEnumerable<ProductCharacteristic>> GetAllAsync(Expression<Func<ProductCharacteristic, bool>> predicate)
    {
        return await _productCharacteristics.Where(predicate).ToListAsync();
    }

    public async Task<ProductCharacteristic> FirstOrDefaultAsync(Expression<Func<ProductCharacteristic, bool>> predicate)
    {
        return await _productCharacteristics.FirstOrDefaultAsync(predicate);
    }
}