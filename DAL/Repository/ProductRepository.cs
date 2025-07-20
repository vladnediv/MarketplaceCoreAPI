using System.Linq.Expressions;
using DAL.Context;
using DAL.Repository.Interface;
using Domain.Model.Product;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Product> _products;

    public ProductRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
        _products = dbContext.Set<Product>();
    }
    
    public async Task<Product> GetByIdAsync(int id)
    {
        return await _products
            .Include(x => x.MediaFiles)
            .Include(x => x.Characteristics)
            .Include(x => x.DeliveryOptions)
            .Include(x => x.OrderItems)
            .Include(x => x.Reviews)
            .Include(x => x.Questions)
            .FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task AddAsync(Product entity)
    {
        await _products.AddAsync(entity);
        
    }

    public async Task UpdateAsync(Product entity)
    {
        await Task.Factory.StartNew(() =>
        {
            _products.Update(entity);
        });
        
    }

    public async Task DeleteAsync(Product entity)
    {
        await Task.Factory.StartNew(() =>
        {
            _products.Remove(entity);
        });
    }

    public async Task DeleteByIdAsync(int id)
    {
        Product? product = await _products.FirstOrDefaultAsync(x => x.Id == id);
        if (product != null)
        {
            await Task.Factory.StartNew(() =>
            {
                _products.Remove(product);
            });
        }
    }
    
    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _products.ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetAllAsync(Expression<Func<Product, bool>> predicate)
    {
        return await _products
            .Where(predicate)
            .Include(x => x.MediaFiles)
            .Include(x => x.Characteristics)
            .Include(x => x.DeliveryOptions)
            .Include(x => x.OrderItems)
            .Include(x => x.Reviews)
            .Include(x => x.Questions)
            .ToListAsync();
    }

    public async Task<Product> FirstOrDefaultAsync(Expression<Func<Product, bool>> predicate)
    {
        return await _products.FirstOrDefaultAsync(predicate);
    }
    public IQueryable<Product> GetQueryable()
    {
        return _context.Set<Product>().AsQueryable();
    }
    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}