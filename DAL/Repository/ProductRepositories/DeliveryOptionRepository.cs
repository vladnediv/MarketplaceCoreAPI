using System.Linq.Expressions;
using DAL.Context;
using DAL.Repository.Interface;
using Domain.Model.Product;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository;

public class DeliveryOptionRepository : IAdvancedRepository<DeliveryOption>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<DeliveryOption> _deliveryOptions;

    public DeliveryOptionRepository(ApplicationDbContext dbContext)
    {
        _context = dbContext;
        _deliveryOptions = dbContext.Set<DeliveryOption>();
    }
    
    public async Task<DeliveryOption> GetByIdAsync(int id)
    {
        return await _deliveryOptions.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(DeliveryOption entity)
    {
        await _deliveryOptions.AddAsync(entity);
    }

    public async Task UpdateAsync(DeliveryOption entity)
    {
        await Task.Factory.StartNew(() =>
        {
            _deliveryOptions.Update(entity);
        });
    }

    public async Task DeleteAsync(DeliveryOption entity)
    {
        await Task.Factory.StartNew(() =>
        {
            _deliveryOptions.Remove(entity);
        });
    }

    public async Task DeleteByIdAsync(int id)
    {
        DeliveryOption deliveryOption = await _deliveryOptions.FirstOrDefaultAsync(x => x.Id == id);
            await Task.Factory.StartNew(() =>
                    {
                        _deliveryOptions.Remove(deliveryOption);
                    });
        
    }

    public async Task<IEnumerable<DeliveryOption>> GetAllAsync()
    {
        return await _deliveryOptions.ToListAsync();
    }

    public async Task<IEnumerable<DeliveryOption>> GetAllAsync(Expression<Func<DeliveryOption, bool>> predicate)
    {
        return await _deliveryOptions.Where(predicate).ToListAsync();
    }

    public async Task<DeliveryOption> FirstOrDefaultAsync(Expression<Func<DeliveryOption, bool>> predicate)
    {
        return await _deliveryOptions.FirstOrDefaultAsync(predicate);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}