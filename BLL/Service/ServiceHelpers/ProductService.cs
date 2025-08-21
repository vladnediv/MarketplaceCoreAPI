using System.Linq.Expressions;
using AutoMapper;
using BLL.Model;
using BLL.Model.DTO.Product;
using BLL.Service.Interface;
using BLL.Service.Interface.BasicInterface;
using BLL.Service.Model;
using BLL.Service.Model.Constants;
using DAL.Repository.DTO;
using DAL.Repository.Interface;
using Domain.Model.Product;
using Microsoft.EntityFrameworkCore;

namespace BLL.Service;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<ServiceResponse<Product>> GetAsync(int id)
    {
        ServiceResponse<Product> response = new ServiceResponse<Product>();
        try
        {
            Product product = await _repository.GetByIdAsync(id);
            if (product != null)
            {
                response.IsSuccess = true;
                response.Entity = product;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(Product), id);
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

    public async Task<ServiceResponse<Product>> DeleteByIdAsync(int id)
    {
        var response = new ServiceResponse<Product>();
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
            else
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.EntityNotFound(nameof(Product));
            }
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }
        return response;
    }

    public async Task<ServiceResponse<ProductCardView>> GetProductCards(string searchQuery, Expression<Func<Product, bool>> predicate)
    {
        ServiceResponse<ProductCardView> response = new ServiceResponse<ProductCardView>();

        try
        {
            IQueryable<Product> query = _repository.GetQueryable();
                
                IQueryable<ProductCardView> dtoList = query
                    .Where(p => p.Name.Contains(searchQuery))
                    .Where(predicate)
                .Include(p => p.MediaFiles)
                .Include(p => p.Reviews)
                .Include(p => p.Questions)
                .Select(p => new ProductCardView 
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    DiscountValue = p.DiscountValue,
                    PictureUrl = p.MediaFiles.FirstOrDefault(x => x.MediaType == MediaType.Image).Url,
                    Rating = p.Reviews.Any() ? p.Reviews.Sum(r => r.Rating) / p.Reviews.Count() : 0,
                    CommentsCount = p.Reviews.Count()
                });

            response.Entities = await dtoList.ToListAsync();
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
}