using System.Linq.Expressions;
using BLL.Model;
using BLL.Service.Interface;
using BLL.Service.Interface.BasicInterface;
using BLL.Service.Model;
using BLL.Service.Model.Constants;
using DAL.Repository.Interface;
using Domain.Model.Product;

namespace BLL.Service;

public class ProductQuestionAnswerService : IAdvancedService<ProductQuestionAnswer>
{
    private readonly IAdvancedRepository<ProductQuestionAnswer> _repository;

    public ProductQuestionAnswerService(IAdvancedRepository<ProductQuestionAnswer> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<ProductQuestionAnswer>> GetAsync(int id)
    {
        ServiceResponse<ProductQuestionAnswer> response = new ServiceResponse<ProductQuestionAnswer>();
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
                response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(ProductQuestionAnswer), id);
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductQuestionAnswer>> CreateAsync(ProductQuestionAnswer entity)
    {
        ServiceResponse<ProductQuestionAnswer> response = new ServiceResponse<ProductQuestionAnswer>();
        try
        {
            await _repository.AddAsync(entity);
            await _repository.SaveChangesAsync();
            response.IsSuccess = true;
            response.Entity = entity;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductQuestionAnswer>> UpdateAsync(ProductQuestionAnswer entity)
    {
        ServiceResponse<ProductQuestionAnswer> response = new ServiceResponse<ProductQuestionAnswer>();
        try
        {
            await _repository.UpdateAsync(entity);
            await _repository.SaveChangesAsync();
            response.IsSuccess = true;
            response.Entity = entity;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductQuestionAnswer>> DeleteAsync(ProductQuestionAnswer entity)
    {
        ServiceResponse<ProductQuestionAnswer> response = new ServiceResponse<ProductQuestionAnswer>();
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

    public async Task<ServiceResponse<ProductQuestionAnswer>> DeleteByIdAsync(int id)
    {
        var response = new ServiceResponse<ProductQuestionAnswer>();
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

    public async Task<ServiceResponse<ProductQuestionAnswer>> GetAllAsync()
    {
        var response = new ServiceResponse<ProductQuestionAnswer>();
        try
        {
            IEnumerable<ProductQuestionAnswer> entities = await _repository.GetAllAsync();
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

    public async Task<ServiceResponse<ProductQuestionAnswer>> GetAllAsync(Expression<Func<ProductQuestionAnswer, bool>> predicate)
    {
        var response = new ServiceResponse<ProductQuestionAnswer>();
        try
        {
            IEnumerable<ProductQuestionAnswer> entities = await _repository.GetAllAsync(predicate);
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

    public async Task<ServiceResponse<ProductQuestionAnswer>> FirstOrDefaultAsync(Expression<Func<ProductQuestionAnswer, bool>> predicate)
    {
        ServiceResponse<ProductQuestionAnswer> response = new ServiceResponse<ProductQuestionAnswer>();
        try
        {
            ProductQuestionAnswer entity = await _repository.FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                response.IsSuccess = true;
                response.Entity = entity;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFound(nameof(ProductQuestionAnswer));
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