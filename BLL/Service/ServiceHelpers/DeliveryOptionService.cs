using System.Linq.Expressions;
using BLL.Model;
using BLL.Model.Constants;
using BLL.Service.Interface.BasicInterface;
using DAL.Repository.Interface;
using Domain.Model.Product;

namespace BLL.Service.ServiceHelpers;

public class DeliveryOptionService : IAdvancedService<DeliveryOption>
{
    private readonly IAdvancedRepository<DeliveryOption> _repository;

    public DeliveryOptionService(IAdvancedRepository<DeliveryOption> repository)
    {
        _repository = repository;
    }
    
    
    public async Task<ServiceResponse<DeliveryOption>> GetAsync(int id)
    {
        ServiceResponse<DeliveryOption> response = new ServiceResponse<DeliveryOption>();
        try
        {
            DeliveryOption entity = await _repository.GetByIdAsync(id);
            if (entity != null)
            {
                response.IsSuccess = true;
                response.Entity = entity;
                return response;
            }
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(DeliveryOption), id);
            
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            return response;
        }
    }

    public async Task<ServiceResponse<DeliveryOption>> CreateAsync(DeliveryOption entity)
    {
        ServiceResponse<DeliveryOption> response = new ServiceResponse<DeliveryOption>();

        if (entity != null)
        {
            try
            {
                await _repository.AddAsync(entity);
                await _repository.SaveChangesAsync();
                
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
        response.IsSuccess = false;
        response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(entity), nameof(DeliveryOption));
        return response;
    }

    public async Task<ServiceResponse<DeliveryOption>> UpdateAsync(DeliveryOption entity)
    {
        ServiceResponse<DeliveryOption> response = new ServiceResponse<DeliveryOption>();

        if (entity != null)
        {
            try
            {
                await _repository.UpdateAsync(entity);
                await _repository.SaveChangesAsync();

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
        response.IsSuccess = false;
        response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(entity), nameof(DeliveryOption));
        
        return response;
    }

    public async Task<ServiceResponse<DeliveryOption>> DeleteAsync(DeliveryOption entity)
    {
        ServiceResponse<DeliveryOption> response = new ServiceResponse<DeliveryOption>();

        if (entity != null)
        {
            try
            {
                await _repository.DeleteAsync(entity);
                await _repository.SaveChangesAsync();
                
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
        response.IsSuccess = false;
        response.Message = ServiceResponseMessages.ArgumentIsNull(nameof(entity), nameof(DeliveryOption));
        
        return response;
    }

    public async Task<ServiceResponse<DeliveryOption>> DeleteByIdAsync(int id)
    {
        var response = new ServiceResponse<DeliveryOption>();
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

    public async Task<ServiceResponse<DeliveryOption>> GetAllAsync()
    {
        ServiceResponse<DeliveryOption> response = new ServiceResponse<DeliveryOption>();

        try
        {
            IEnumerable<DeliveryOption> entities = await _repository.GetAllAsync();

            if (entities != null)
            {
                response.IsSuccess = true;
                response.Entities = entities.ToList();
                return response;
            }

            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UnknownError;
            
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            
            return response;
        }
    }

    public async Task<ServiceResponse<DeliveryOption>> GetAllAsync(Expression<Func<DeliveryOption, bool>> predicate)
    {
        ServiceResponse<DeliveryOption> response = new ServiceResponse<DeliveryOption>();

        try
        {
            IEnumerable<DeliveryOption> entities = await _repository.GetAllAsync(predicate);

            if (entities != null)
            {
                response.IsSuccess = true;
                response.Entities = entities.ToList();
                return response;
            }

            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UnknownError;
            
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
            
            return response;
        }
    }

    public async Task<ServiceResponse<DeliveryOption>> FirstOrDefaultAsync(Expression<Func<DeliveryOption, bool>> predicate)
    {
        ServiceResponse<DeliveryOption> response = new ServiceResponse<DeliveryOption>();

        try
        {
            DeliveryOption entity = await _repository.FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                response.IsSuccess = true;
                response.Entity = entity;
                return response;
            }
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.UnknownError;
            
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