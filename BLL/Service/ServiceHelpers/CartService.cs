using System.Linq.Expressions;
using BLL.Model;
using BLL.Service.Interface;
using BLL.Service.Interface.BasicInterface;
using BLL.Service.Model;
using BLL.Service.Model.Constants;
using DAL.Repository.Interface;
using Domain.Model.Cart;

namespace BLL.Service;

public class CartService : IAdvancedService<Cart>
{
    private readonly IAdvancedRepository<Cart> _cartRepository;

    public CartService(IAdvancedRepository<Cart> cartRepository)
    {
        _cartRepository = cartRepository;
    }
    
    public async Task<ServiceResponse<Cart>> GetAsync(int id)
    {
        ServiceResponse<Cart> response = new ServiceResponse<Cart>();

        try
        {
            Cart cart = await _cartRepository.GetByIdAsync(id);

            if (cart == null)
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(Cart), id);

                return response;
            }
            
            response.IsSuccess = true;
            response.Entity = cart;
            
            return response;
            
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            
            return response;
        }
    }

    public async Task<ServiceResponse<Cart>> CreateAsync(Cart entity)
    {
        ServiceResponse<Cart> response = new ServiceResponse<Cart>();

        try
        {
            await _cartRepository.AddAsync(entity);
            await _cartRepository.SaveChangesAsync();

            response.IsSuccess = true;

            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            
            return response;
        }
        
    }

    public async Task<ServiceResponse<Cart>> UpdateAsync(Cart entity)
    {
        ServiceResponse<Cart> response = new ServiceResponse<Cart>();

        try
        {
            await _cartRepository.UpdateAsync(entity);
            await _cartRepository.SaveChangesAsync();

            response.IsSuccess = true;

            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;

            return response;
        }
    }

    
    public async Task<ServiceResponse<Cart>> DeleteAsync(Cart entity)
    {
        ServiceResponse<Cart> response = new ServiceResponse<Cart>();

        try
        {
            await _cartRepository.DeleteAsync(entity);
            await _cartRepository.SaveChangesAsync();
            
            response.IsSuccess = true;
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            
            return response;
        }
    }

    public async Task<ServiceResponse<Cart>> DeleteByIdAsync(int id)
    {
        ServiceResponse<Cart> response = new ServiceResponse<Cart>();

        try
        {
            await _cartRepository.DeleteByIdAsync(id);
            await _cartRepository.SaveChangesAsync();

            response.IsSuccess = true;
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            
            return response;
        }
    }   

    public async Task<ServiceResponse<Cart>> GetAllAsync()
    {
        ServiceResponse<Cart> response = new ServiceResponse<Cart>();

        try
        {
            IEnumerable<Cart> carts = await _cartRepository.GetAllAsync();
            
            response.IsSuccess = true;
            response.Entities = carts.ToList();
            
            return response;
        }
        catch(Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            
            return response;
        }
    }

    public async Task<ServiceResponse<Cart>> GetAllAsync(Expression<Func<Cart, bool>> predicate)
    {
        ServiceResponse<Cart> response = new ServiceResponse<Cart>();

        try
        {
            IEnumerable<Cart> carts = await _cartRepository.GetAllAsync(predicate);
            
            response.IsSuccess = true;
            response.Entities = carts.ToList();
            
            return response;
        }
        catch(Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            
            return response;
        }
    }

    public async Task<ServiceResponse<Cart>> FirstOrDefaultAsync(Expression<Func<Cart, bool>> predicate)
    {
        ServiceResponse<Cart> response = new ServiceResponse<Cart>();

        try
        {
            Cart cart = await _cartRepository.FirstOrDefaultAsync(predicate);

            if (cart == null)
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFound(nameof(Cart));
                
                return response;
            }
            
            response.IsSuccess = true;
            response.Entity = cart;

            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            
            return response;
        }
    }
}