using System.Linq.Expressions;
using BLL.Service.Interface;
using BLL.Service.Model;
using BLL.Service.Model.Constants;
using DAL.Repository;
using DAL.Repository.Interface;
using Domain.Model.Product;

namespace BLL.Service;

public class ProductQuestionService : IAdvancedService<ProductQuestion>
{
    private readonly IAdvancedRepository<ProductQuestion> _repository;

    public ProductQuestionService(IAdvancedRepository<ProductQuestion> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<ProductQuestion>> GetAsync(int id)
    {
        var response = new ServiceResponse<ProductQuestion>();
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
                response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(ProductQuestion), id);
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductQuestion>> CreateAsync(ProductQuestion entity)
    {
        var response = new ServiceResponse<ProductQuestion>();
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

    public async Task<ServiceResponse<ProductQuestion>> UpdateAsync(ProductQuestion entity)
    {
        var response = new ServiceResponse<ProductQuestion>();
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

    public async Task<ServiceResponse<ProductQuestion>> DeleteAsync(ProductQuestion entity)
    {
        var response = new ServiceResponse<ProductQuestion>();
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

    public async Task<ServiceResponse<ProductQuestion>> DeleteByIdAsync(int id)
    {
        var response = new ServiceResponse<ProductQuestion>();
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

    public async Task<ServiceResponse<ProductQuestion>> GetAllAsync()
    {
        var response = new ServiceResponse<ProductQuestion>();
        try
        {
            IEnumerable<ProductQuestion> entities = await _repository.GetAllAsync();
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

    public async Task<ServiceResponse<ProductQuestion>> GetAllAsync(Expression<Func<ProductQuestion, bool>> predicate)
    {
        var response = new ServiceResponse<ProductQuestion>();
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

    public async Task<ServiceResponse<ProductQuestion>> FirstOrDefaultAsync(Expression<Func<ProductQuestion, bool>> predicate)
    {
        ServiceResponse<ProductQuestion> response = new ServiceResponse<ProductQuestion>();
        try
        {
            ProductQuestion entity = await _repository.FirstOrDefaultAsync(predicate);
            if (entity != null)
            {
                response.IsSuccess = true;
                response.Entity = entity;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFound(nameof(ProductQuestion));
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