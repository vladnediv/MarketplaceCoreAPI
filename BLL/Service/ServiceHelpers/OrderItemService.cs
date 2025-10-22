using System.Linq.Expressions;
using BLL.Model;
using BLL.Model.Constants;
using BLL.Service.Interface.BasicInterface;
using DAL.Repository.Interface;
using Domain.Model.Order;

namespace BLL.Service.ServiceHelpers;

public class OrderItemService : IAdvancedService<OrderItem>
{
    private readonly IAdvancedRepository<OrderItem> _orderItemRepository;

    public OrderItemService(IAdvancedRepository<OrderItem> orderItemRepository)
    {
        _orderItemRepository = orderItemRepository;
    }

    public async Task<ServiceResponse<OrderItem>> GetAsync(int id)
    {
        var response = new ServiceResponse<OrderItem>();
        try
        {
            var item = await _orderItemRepository.GetByIdAsync(id);

            if (item == null)
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(OrderItem), id);
                return response;
            }

            response.IsSuccess = true;
            response.Entity = item;
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<OrderItem>> CreateAsync(OrderItem entity)
    {
        var response = new ServiceResponse<OrderItem>();
        try
        {
            await _orderItemRepository.AddAsync(entity);
            await _orderItemRepository.SaveChangesAsync();

            response.IsSuccess = true;
            response.Entity = entity;
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<OrderItem>> UpdateAsync(OrderItem entity)
    {
        var response = new ServiceResponse<OrderItem>();
        try
        {
            await _orderItemRepository.UpdateAsync(entity);
            await _orderItemRepository.SaveChangesAsync();

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

    public async Task<ServiceResponse<OrderItem>> DeleteAsync(OrderItem entity)
    {
        var response = new ServiceResponse<OrderItem>();
        try
        {
            await _orderItemRepository.DeleteAsync(entity);
            await _orderItemRepository.SaveChangesAsync();

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

    public async Task<ServiceResponse<OrderItem>> DeleteByIdAsync(int id)
    {
        var response = new ServiceResponse<OrderItem>();
        try
        {
            await _orderItemRepository.DeleteByIdAsync(id);
            await _orderItemRepository.SaveChangesAsync();

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

    public async Task<ServiceResponse<OrderItem>> GetAllAsync()
    {
        var response = new ServiceResponse<OrderItem>();
        try
        {
            IEnumerable<OrderItem> list = await _orderItemRepository.GetAllAsync();

            response.IsSuccess = true;
            response.Entities = list.ToList();
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<OrderItem>> GetAllAsync(Expression<Func<OrderItem, bool>> predicate)
    {
        var response = new ServiceResponse<OrderItem>();
        try
        {
            IEnumerable<OrderItem> list = await _orderItemRepository.GetAllAsync(predicate);

            response.IsSuccess = true;
            response.Entities = list.ToList();
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<OrderItem>> FirstOrDefaultAsync(Expression<Func<OrderItem, bool>> predicate)
    {
        var response = new ServiceResponse<OrderItem>();
        try
        {
            var item = await _orderItemRepository.FirstOrDefaultAsync(predicate);

            response.IsSuccess = true;
            response.Entity = item;
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