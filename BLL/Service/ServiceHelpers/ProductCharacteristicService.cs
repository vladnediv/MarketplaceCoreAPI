using System.Linq.Expressions;
using BLL.Service.Interface;
using BLL.Service.Model;
using BLL.Service.Model.Constants;
using DAL.Repository;
using DAL.Repository.Interface;
using Domain.Model.Product;

namespace BLL.Service;

public class ProductCharacteristicService : IAdvancedService<ProductCharacteristic>
{
    private readonly IAdvancedRepository<ProductCharacteristic> _repository;

    public ProductCharacteristicService(IAdvancedRepository<ProductCharacteristic> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<ProductCharacteristic>> GetAsync(int id)
    {
        var response = new ServiceResponse<ProductCharacteristic>();
        try
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                response.IsSuccess = true;
                response.Entity = entity;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(ProductCharacteristic), id);
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductCharacteristic>> CreateAsync(ProductCharacteristic entity)
    {
        var response = new ServiceResponse<ProductCharacteristic>();
        
        try
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductCharacteristic>> UpdateAsync(ProductCharacteristic entity)
    {
        var response = new ServiceResponse<ProductCharacteristic>();
        try
        {
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductCharacteristic>> DeleteAsync(ProductCharacteristic entity)
    {
        var response = new ServiceResponse<ProductCharacteristic>();
        try
        {
            await _repository.DeleteAsync(entity);
            await _repository.SaveChangesAsync();
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductCharacteristic>> DeleteByIdAsync(int id)
    {
        var response = new ServiceResponse<ProductCharacteristic>();
        try
        {
            await _repository.DeleteByIdAsync(id);
            await _repository.SaveChangesAsync();
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductCharacteristic>> GetAllAsync()
    {
        var response = new ServiceResponse<ProductCharacteristic>();
        try
        {
            var entities = await _repository.GetAllAsync();
            response.IsSuccess = true;
            response.Entities = entities.ToList();
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductCharacteristic>> GetAllAsync(Expression<Func<ProductCharacteristic, bool>> predicate)
    {
        var response = new ServiceResponse<ProductCharacteristic>();
        try
        {
            var entities = await _repository.GetAllAsync(predicate);
            response.IsSuccess = true;
            response.Entities = entities.ToList();
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductCharacteristic>> FirstOrDefaultAsync(Expression<Func<ProductCharacteristic, bool>> predicate)
    {
        var response = new ServiceResponse<ProductCharacteristic>();
        try
        {
            var entity = await _repository.FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                response.IsSuccess = true;
                response.Entity = entity;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFound(nameof(ProductCharacteristic));
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }
}