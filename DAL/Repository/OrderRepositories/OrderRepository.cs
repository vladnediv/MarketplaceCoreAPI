using System.Linq.Expressions;
using DAL.Context;
using DAL.Repository.Interface;
using Domain.Model.Order;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.OrderRepositories;

public class OrderRepository : IAdvancedRepository<Order>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Order> _orders;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
        _orders = _context.Set<Order>();
    }

    public async Task<Order> GetByIdAsync(int id)
    {
        return await _orders
            .Include(x => x.OrderItems)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Order entity)
    {
        await _orders.AddAsync(entity);
    }

    public async Task UpdateAsync(Order entity)
    {
        await Task.Factory.StartNew(() => _orders.Update(entity));
    }

    public async Task DeleteAsync(Order entity)
    {
        await Task.Factory.StartNew(() => _orders.Remove(entity));
    }

    public async Task DeleteByIdAsync(int id)
    {
        Order entity = await GetByIdAsync(id);
        
        await Task.Factory.StartNew(() => _orders.Remove(entity));
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Order>> GetAllAsync()
    {
        return await _orders.ToListAsync();
    }

    public async Task<IEnumerable<Order>> GetAllAsync(Expression<Func<Order, bool>> predicate)
    {
        return await _orders.Where(predicate).ToListAsync();
    }

    public async Task<Order> FirstOrDefaultAsync(Expression<Func<Order, bool>> predicate)
    {
        return await _orders.FirstOrDefaultAsync(predicate);
    }
}