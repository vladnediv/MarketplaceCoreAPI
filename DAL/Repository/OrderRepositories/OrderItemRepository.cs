using System.Linq.Expressions;
using DAL.Context;
using DAL.Repository.Interface;
using Domain.Model.Order;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.OrderRepositories;

public class OrderItemRepository : IAdvancedRepository<OrderItem>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<OrderItem> _orderItems;

    public OrderItemRepository(ApplicationDbContext context)
    {
        _context = context;
        _orderItems = context.Set<OrderItem>();
    }

    public async Task<OrderItem> GetByIdAsync(int id)
    {
        return await _orderItems
            .Include(x => x.Product)
                .ThenInclude(p => p.MediaFiles)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(OrderItem entity)
    {
        await _orderItems.AddAsync(entity);
    }

    public async Task UpdateAsync(OrderItem entity)
    {
        await Task.Factory.StartNew(() => _orderItems.Update(entity));
    }

    public async Task DeleteAsync(OrderItem entity)
    {
        await Task.Factory.StartNew(() => _orderItems.Remove(entity));
    }

    public async Task DeleteByIdAsync(int id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            await Task.Factory.StartNew(() => _orderItems.Remove(entity));
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    // Includes: Product -> MediaFiles, Order
    public async Task<IEnumerable<OrderItem>> GetAllAsync()
    {
        return await _orderItems
            .Include(x => x.Product)
                .ThenInclude(p => p.MediaFiles)
            .ToListAsync();
    }

    // Includes: Product -> MediaFiles, Order
    public async Task<IEnumerable<OrderItem>> GetAllAsync(Expression<Func<OrderItem, bool>> predicate)
    {
        return await _orderItems
            .Where(predicate)
            .Include(x => x.Product)
                .ThenInclude(p => p.MediaFiles)
            .ToListAsync();
    }

    public async Task<OrderItem> FirstOrDefaultAsync(Expression<Func<OrderItem, bool>> predicate)
    {
        return await _orderItems
            .Include(x => x.Product)
                .ThenInclude(p => p.MediaFiles)
            .FirstOrDefaultAsync(predicate);
    }
}