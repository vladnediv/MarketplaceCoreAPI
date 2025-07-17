using System.Linq.Expressions;
using BLL.Service.Interface;
using BLL.Service.Model;
using DAL.Repository;
using DAL.Repository.Interface;
using Domain.Model.Product;

namespace BLL.Service;

public class ProductReviewService : IAdvancedService<ProductReview>
{
    private readonly IAdvancedRepository<ProductReview> _repository;

    public ProductReviewService(IAdvancedRepository<ProductReview> repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<ProductReview>> GetAsync(int id)
    {
        var response = new ServiceResponse<ProductReview>();
        try
        {
            var review = await _repository.GetByIdAsync(id);
            if (review != null)
            {
                response.IsSuccess = true;
                response.Entity = review;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductReview>> CreateAsync(ProductReview entity)
    {
        var response = new ServiceResponse<ProductReview>();
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

    public async Task<ServiceResponse<ProductReview>> UpdateAsync(ProductReview entity)
    {
        var response = new ServiceResponse<ProductReview>();
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

    public async Task<ServiceResponse<ProductReview>> DeleteAsync(ProductReview entity)
    {
        var response = new ServiceResponse<ProductReview>();
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

    public async Task<ServiceResponse<ProductReview>> DeleteByIdAsync(int id)
    {
        var response = new ServiceResponse<ProductReview>();
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
    public async Task<ServiceResponse<ProductReview>> GetAllAsync()
    {
        var response = new ServiceResponse<ProductReview>();
        try
        {
            var reviews = await _repository.GetAllAsync();
            response.IsSuccess = true;
            response.Entities = reviews.ToList();
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductReview>> GetAllAsync(Expression<Func<ProductReview, bool>> predicate)
    {
        var response = new ServiceResponse<ProductReview>();
        try
        {
            var reviews = await _repository.GetAllAsync(predicate);
            response.IsSuccess = true;
            response.Entities = reviews.ToList();
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductReview>> FirstOrDefaultAsync(Expression<Func<ProductReview, bool>> predicate)
    {
        var response = new ServiceResponse<ProductReview>();
        try
        {
            var review = await _repository.FirstOrDefaultAsync(predicate);
            if (review != null)
            {
                response.IsSuccess = true;
                response.Entity = review;
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