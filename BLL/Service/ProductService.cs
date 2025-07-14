using System.Linq.Expressions;
using BLL.Service.Interface;
using BLL.Service.Model;
using DAL.Repository;
using Domain.Model.Product;

namespace BLL.Service;

public class ProductService : IAdvancedService<Product>
{
    private readonly ProductRepository _repository;

    public ProductService(ProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<ServiceResponse<Product>> GetAsync(int id)
    {
        var response = new ServiceResponse<Product>();
        try
        {
            var product = await _repository.GetByIdAsync(id);
            if (product != null)
            {
                response.IsSuccess = true;
                response.Entity = product;
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<Product>> CreateAsync(Product entity)
    {
        var response = new ServiceResponse<Product>();
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

    public async Task<ServiceResponse<Product>> UpdateAsync(Product entity)
    {
        var response = new ServiceResponse<Product>();
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

    public async Task<ServiceResponse<Product>> DeleteAsync(Product entity)
    {
        var response = new ServiceResponse<Product>();
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

    public async Task<ServiceResponse<Product>> GetAllAsync()
    {
        var response = new ServiceResponse<Product>();
        try
        {
            var products = await _repository.GetAllAsync();
            response.IsSuccess = true;
            response.Entities = products.ToList();
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<Product>> GetAllAsync(Expression<Func<Product, bool>> predicate)
    {
        var response = new ServiceResponse<Product>();
        try
        {
            var products = await _repository.GetAllAsync(predicate);
            response.IsSuccess = true;
            response.Entities = products.ToList();
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<Product>> FirstOrDefaultAsync(Expression<Func<Product, bool>> predicate)
    {
        var response = new ServiceResponse<Product>();
        try
        {
            var product = await _repository.FirstOrDefaultAsync(predicate);
            if (product != null)
            {
                response.IsSuccess = true;
                response.Entity = product;
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