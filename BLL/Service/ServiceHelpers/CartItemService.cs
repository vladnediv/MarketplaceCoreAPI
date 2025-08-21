using BLL.Model;
using BLL.Service.Interface;
using BLL.Service.Interface.BasicInterface;
using BLL.Service.Model;
using BLL.Service.Model.Constants;
using DAL.Repository.Interface;
using Domain.Model.Cart;

namespace BLL.Service;

public class CartItemService : IGenericService<CartItem>
{
    private readonly IGenericRepository<CartItem> _cartItemRepository;

    public CartItemService(IGenericRepository<CartItem> cartItemRepository)
    {
        _cartItemRepository = cartItemRepository;
    }


    public async Task<ServiceResponse<CartItem>> GetAsync(int id)
    {
        ServiceResponse<CartItem> response = new ServiceResponse<CartItem>();

        try
        {
            CartItem cartItem = await _cartItemRepository.GetByIdAsync(id);
            if (cartItem == null)
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(CartItem), id);

                return response;
            }

            response.IsSuccess = true;
            response.Entity = cartItem;

            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;

            return response;
        }
    }

    public async Task<ServiceResponse<CartItem>> CreateAsync(CartItem entity)
    {
        ServiceResponse<CartItem> response = new ServiceResponse<CartItem>();

        try
        {
            await _cartItemRepository.AddAsync(entity);
            await _cartItemRepository.SaveChangesAsync();

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

    public async Task<ServiceResponse<CartItem>> UpdateAsync(CartItem entity)
    {
        ServiceResponse<CartItem> response = new ServiceResponse<CartItem>();

        try
        {
            await _cartItemRepository.UpdateAsync(entity);
            await _cartItemRepository.SaveChangesAsync();

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

    public async Task<ServiceResponse<CartItem>> DeleteAsync(CartItem entity)
    {
        ServiceResponse<CartItem> response = new ServiceResponse<CartItem>();

        try
        {
            await _cartItemRepository.DeleteAsync(entity);
            await _cartItemRepository.SaveChangesAsync();

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

    public async Task<ServiceResponse<CartItem>> DeleteByIdAsync(int id)
    {
        ServiceResponse<CartItem> response = new ServiceResponse<CartItem>();

        try
        {
            await _cartItemRepository.DeleteByIdAsync(id);
            await _cartItemRepository.SaveChangesAsync();

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
}