using System.Linq.Expressions;
using AutoMapper;
using BLL.Model;
using BLL.Model.Constants;
using BLL.Model.DTO.Product;
using BLL.Service.Interface.BasicInterface;
using DAL.Repository.Interface;
using Domain.Model.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;

namespace BLL.Service.ServiceHelpers;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IMemoryCache _cache;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository repository, IMemoryCache cache, IMapper mapper)
    {
        _repository = repository;
        _cache = cache;
        _mapper = mapper;
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

    public async Task<ServiceResponse<ProductCardView>> GetProductCards(string? searchQuery, int? categoryId, Expression<Func<Product, bool>>? predicate)
    {
        ServiceResponse<ProductCardView> response = new ServiceResponse<ProductCardView>();
        
        
        
        try
        {
            IQueryable<Product> query = _repository.GetQueryable();

            List<Product> productList = new List<Product>();
            
           
            //TODO Test here: get products by parent category
                productList = await query
                    .Where(predicate)
                    .Where(x => searchQuery.IsNullOrEmpty() ? x.Name == x.Name : x.Name.Contains(searchQuery))
                    .Where(x => x.Category.ParentCategoryId == categoryId || x.CategoryId == categoryId)
                    .Include(p => p.MediaFiles)
                    .Include(p => p.Reviews)
                    .Include(p => p.Questions)
                    .Include(x => x.Characteristics).ThenInclude(x => x.Characteristics)
                    .Include(x => x.Category).ToListAsync();
            

            response.Entities = productList.Select(x => _mapper.Map<ProductCardView>(x)).ToList();
                
                /*.Select(p => new ProductCardView
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                DiscountValue = p.DiscountValue,
                PictureUrl = p.MediaFiles.FirstOrDefault(x => x.MediaType == MediaType.Image).Url,
                Rating = p.Reviews.Where(x => x.IsApproved && x.IsReviewed).Any()
                    ? p.Reviews.Sum(r => r.Rating) / p.Reviews.Count()
                    : 0,
                CommentsCount = p.Reviews.Where(x => x.IsApproved && x.IsReviewed).Count(),
                CategoryName = p.Category.Name
            })*/

                var cacheName = $"cachedProducts{Guid.NewGuid()}";
            _cache.Set(cacheName, productList, TimeSpan.FromMinutes(1));
            
            response.IsSuccess = true;
            response.Message = cacheName;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = ex.Message;
        }

        return response;
    }
    
    public async Task<ServiceResponse> ModifyProductStockAsync(bool decrease, int productId, int amount)
    {
        //todo think again
        //separate method to check if all products are in stock, and just then modify the stock value
        ServiceResponse response = new ServiceResponse();
        
        //get the product by id
        var product = await GetAsync(productId);
        if (!product.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = product.Message;

            return response;
        }
        
        //decrease or increase product stock by the amount
        if (decrease)
        {
            if (product.Entity.Stock == 0)
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.ProductOutOfStock(product.Entity.Name);
                
                return response;
            }
            if(amount <= product.Entity.Stock) product.Entity.Stock -= amount;
            else
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.ProductLowOnStock(product.Entity.Name,  product.Entity.Stock);
                
                return response;
            }
        }
        else
        {
            product.Entity.Stock += amount;
        }
        
        var updateRes = await UpdateAsync(product.Entity);

        if (!updateRes.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = updateRes.Message;
                
            return response;
        }
            
        response.IsSuccess = true;
            
        return response;
    }

    public async Task<ServiceResponse> CheckIfProductOnStock(int productId, int amount)
    {
        var response = new ServiceResponse();

        var product = await _repository.GetByIdAsync(productId);
        if (product == null)
        {
            response.IsSuccess = false;
            response.Message = ServiceResponseMessages.EntityNotFoundById(nameof(Product), productId);
        }
        else
        {
            if (product.Stock >= amount)
            {
                response.IsSuccess = true;
            }
            else
            {
                response.IsSuccess = false;
                response.Message = ServiceResponseMessages.ProductLowOnStock(product.Name, product.Stock);
            }
        }
        
        return response;
    }
}