using System.Linq.Expressions;
using DAL.Context;
using DAL.Repository.Interface;
using Domain.Model.Cart;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.CartRepositories;

public class CartRepository : IAdvancedRepository<Cart>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<Cart> _carts;

    public CartRepository(ApplicationDbContext context)
    {
        _context = context;
        _carts = _context.Set<Cart>();
    }
    
    public async Task<Cart> GetByIdAsync(int id)
    {
        return await _carts.Include(x => x.CartItems).FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(Cart entity)
    {
        await _carts.AddAsync(entity);
    }

    public async Task UpdateAsync(Cart entity)
    {
        _carts.Update(entity);
    }

    public async Task DeleteAsync(Cart entity)
    {
        _carts.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        Cart cart = await GetByIdAsync(id);
        
        _carts.Remove(cart);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Cart>> GetAllAsync()
    {
        return await _carts.ToListAsync();
    }

    public async Task<IEnumerable<Cart>> GetAllAsync(Expression<Func<Cart, bool>> predicate)
    {
        return await _carts.Where(predicate).ToListAsync();
    }

    public async Task<Cart> FirstOrDefaultAsync(Expression<Func<Cart, bool>> predicate)
    {
        return await _carts.Where(predicate).Include(x => x.CartItems).FirstOrDefaultAsync();
    }
}