using DAL.Context;
using DAL.Repository.Interface;
using Domain.Model.Cart;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repository.CartRepositories;

public class CartItemRepository : IGenericRepository<CartItem>
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<CartItem> _cartItems;

    public CartItemRepository(ApplicationDbContext context)
    {
        _context = context;
        _cartItems = _context.Set<CartItem>();
    }


    public async Task<CartItem> GetByIdAsync(int id)
    {
        CartItem cartItem = await _cartItems.FirstOrDefaultAsync(x => x.Id == id);
        
        return cartItem;
    }

    public async Task AddAsync(CartItem entity)
    {
        await _cartItems.AddAsync(entity);
    }

    public async Task UpdateAsync(CartItem entity)
    {
        _cartItems.Update(entity);
    }

    public async Task DeleteAsync(CartItem entity)
    {
        _cartItems.Remove(entity);
    }

    public async Task DeleteByIdAsync(int id)
    {
        CartItem cartItem = await GetByIdAsync(id);
        _cartItems.Remove(cartItem);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}