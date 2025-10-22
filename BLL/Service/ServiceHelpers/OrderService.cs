using System.Linq.Expressions;
using BLL.Model;
using BLL.Model.Constants;
using BLL.Service.Interface.BasicInterface;
using DAL.Repository.Interface;
using Domain.Model.Order;

namespace BLL.Service.ServiceHelpers;

public class OrderService : IAdvancedService<Order>
{
    private readonly IAdvancedRepository<Order> _orderRepository;

    public OrderService(IAdvancedRepository<Order> orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<ServiceResponse<Order>> GetAsync(int id)
    {
        var response = new ServiceResponse<Order>();
        try
        {
            Order order = await _orderRepository.GetByIdAsync(id);

            if (order == null)
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(Order), id);
                return response;
            }

            response.IsSuccess = true;
            response.Entity = order;

            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;

            return response;
        }
    }

    public async Task<ServiceResponse<Order>> CreateAsync(Order entity)
    {
        var response = new ServiceResponse<Order>();

        try
        {
            await _orderRepository.AddAsync(entity);
            await _orderRepository.SaveChangesAsync();

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

    public async Task<ServiceResponse<Order>> UpdateAsync(Order entity)
    {
        var response = new ServiceResponse<Order>();

        try
        {
            await _orderRepository.UpdateAsync(entity);
            await _orderRepository.SaveChangesAsync();

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

    public async Task<ServiceResponse<Order>> DeleteAsync(Order entity)
    {
        var response = new ServiceResponse<Order>();

        try
        {
            await _orderRepository.DeleteAsync(entity);
            await _orderRepository.SaveChangesAsync();

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

    public async Task<ServiceResponse<Order>> DeleteByIdAsync(int id)
    {
        var response = new ServiceResponse<Order>();

        try
        {
            await _orderRepository.DeleteByIdAsync(id);
            await _orderRepository.SaveChangesAsync();

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

    public async Task<ServiceResponse<Order>> GetAllAsync()
    {
        var response = new ServiceResponse<Order>();

        try
        {
            IEnumerable<Order> list = await _orderRepository.GetAllAsync();

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

    public async Task<ServiceResponse<Order>> GetAllAsync(Expression<Func<Order, bool>> predicate)
    {
        var response = new ServiceResponse<Order>();

        try
        {
            IEnumerable<Order> list = await _orderRepository.GetAllAsync(predicate);

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

    public async Task<ServiceResponse<Order>> FirstOrDefaultAsync(Expression<Func<Order, bool>> predicate)
    {
        var response = new ServiceResponse<Order>();

        try
        {
            Order order = await _orderRepository.FirstOrDefaultAsync(predicate);

            response.IsSuccess = true;
            response.Entity = order;

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