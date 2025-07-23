using System.Linq.Expressions;
using AutoMapper;
using BLL.Service.Interface;
using BLL.Service.Model;
using DAL.Context;
using DAL.Repository;
using DAL.Repository.DTO;
using DAL.Repository.Interface;
using Domain.Model.Order;
using Domain.Model.Product;
using Microsoft.EntityFrameworkCore;

namespace BLL.Service;

//TODO Create OrderService
public class ShopService : IShopService
{
    private readonly IProductService _productService;
    private readonly IGenericService<ProductQuestionAnswer> _questionAnswerService;
    //private readonly IAdvancedService<Order> _orderService;
    private readonly IAdvancedService<ProductReview> _reviewService;
    private readonly IAdvancedService<ProductQuestion> _questionService;
    private readonly IAdvancedService<DeliveryOption> _deliveryOptionService;
    private readonly IMapper _mapper;

    public ShopService(
        IProductService productService,
        IGenericService<ProductQuestionAnswer> questionAnswerService,
        IAdvancedService<ProductReview> reviewService,
        IAdvancedService<ProductQuestion> questionService,
        IAdvancedService<DeliveryOption> deliveryOptionService,
        IMapper mapper)
    {
        _productService = productService;
        _questionAnswerService = questionAnswerService;
        _reviewService = reviewService;
        _questionService = questionService;
        _deliveryOptionService = deliveryOptionService;
        _mapper = mapper;
    }

    public async Task<ServiceResponse<CreateProduct>> CreateProductAsync(CreateProduct product)
    {
        Product entity = _mapper.Map<CreateProduct, Product>(product);
        
        ServiceResponse<Product> response = await _productService.CreateAsync(entity);
        
        ServiceResponse<CreateProduct> serviceResponse = new ServiceResponse<CreateProduct>();
        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
        }
        else
        {
            serviceResponse.IsSuccess = false;
        }
        serviceResponse.Message = response.Message;
        
        return serviceResponse;
    }

    public async Task<ServiceResponse<UpdateProduct>> UpdateProductAsync(UpdateProduct updateProduct)
    {
        Product entity = _mapper.Map<Product>(updateProduct);
        
        
        var oldProduct = await _productService.GetAsync(entity.Id);
        
        //TODO error with updating delivery options
        
        if (oldProduct.IsSuccess)
        {
            oldProduct.Entity.Name = entity.Name;
            oldProduct.Entity.Price = entity.Price;
            oldProduct.Entity.Stock = entity.Stock;
            oldProduct.Entity.DiscountValue = entity.DiscountValue;
            oldProduct.Entity.MediaFiles = entity.MediaFiles;
            oldProduct.Entity.Characteristics = entity.Characteristics;
            oldProduct.Entity.ProductDeliveryOptions = entity.ProductDeliveryOptions;
        }
        else
        {
            return new ServiceResponse<UpdateProduct>()
            {
                IsSuccess = false
            };
        }
        
        ServiceResponse<Product> response = await _productService.UpdateAsync(oldProduct.Entity);
        
        ServiceResponse<UpdateProduct> serviceResponse = new ServiceResponse<UpdateProduct>();
        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
        }
        else
        {
            serviceResponse.IsSuccess = false;
        }
        serviceResponse.Message = response.Message;
        return serviceResponse;
    }

    public async Task<ServiceResponse<object>> DeleteProductByIdAsync(int id)
    {
        ServiceResponse<Product> response = await _productService.DeleteByIdAsync(id);
        
        ServiceResponse<object> serviceResponse = new ServiceResponse<object>();
        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
        }
        else
        {
            serviceResponse.IsSuccess = false;
        }
        serviceResponse.Message = response.Message;
        return serviceResponse;
    }

    public async Task<ServiceResponse<ShopProductView>> GetProductByIdAsync(int id)
    {
        ServiceResponse<Product> res = await _productService.GetAsync(id);
        ServiceResponse<ShopProductView> response = new ServiceResponse<ShopProductView>();
        if (res.IsSuccess)
        {
            //TODO ERROR HERE!!!
            ShopProductView entity = _mapper.Map<Product, ShopProductView>(res.Entity);
            response.Entity = entity;
            response.IsSuccess = true;
        }
        else
        {
            response.IsSuccess = false;
        }
        response.Message = res.Message;
        return response;
    }

    public async Task<ServiceResponse<ShopProductView>> GetProductsByParameterAsync(Expression<Func<Product, bool>> predicate)
    {
        ServiceResponse<Product> response = await _productService.GetAllAsync(predicate);
        ServiceResponse<ShopProductView> res = new ServiceResponse<ShopProductView>();
        if (response.IsSuccess)
        {
            List<ShopProductView> entities = response.Entities.Select(x => _mapper.Map<Product, ShopProductView>(x)).ToList();
            res.Entities = entities;
            res.IsSuccess = true;
        }
        else
        {
            res.IsSuccess = false;
        }
        
        return res;
    }

    public async Task<ServiceResponse<CreateProductQuestionAnswer>> CreateProductQuestionAnswerAsync(CreateProductQuestionAnswer createProductQuestionAnswer)
    {
        ProductQuestionAnswer entity = _mapper.Map<ProductQuestionAnswer>(createProductQuestionAnswer);
        ServiceResponse<ProductQuestionAnswer> response = await _questionAnswerService.CreateAsync(entity);
        
        ServiceResponse<CreateProductQuestionAnswer> serviceResponse = new ServiceResponse<CreateProductQuestionAnswer>();

        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
        }
        else
        {
            serviceResponse.IsSuccess = false;
        }
        serviceResponse.Message = response.Message;
        return serviceResponse;
    }

    public async Task<ServiceResponse<ProductReviewDTO>> GetProductReviewsByParameterAsync(Expression<Func<ProductReview, bool>> predicate)
    {
        ServiceResponse<ProductReview> response = await _reviewService.GetAllAsync(predicate);
        
        ServiceResponse<ProductReviewDTO> serviceResponse = new ServiceResponse<ProductReviewDTO>();
        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
            serviceResponse.Entities = response.Entities.Select(x => _mapper.Map<ProductReview, ProductReviewDTO>(x)).ToList();
        }
        else
        {
            serviceResponse.IsSuccess = false;
        }
        serviceResponse.Message = response.Message;
        return serviceResponse;
    }

    public async Task<ServiceResponse<ProductQuestionDTO>> GetProductQuestionsByParameterAsync(Expression<Func<ProductQuestion, bool>> predicate)
    {
        ServiceResponse<ProductQuestion> response = await _questionService.GetAllAsync(predicate);
        
        ServiceResponse<ProductQuestionDTO> serviceResponse = new ServiceResponse<ProductQuestionDTO>();
        if (response.IsSuccess)
        {
            serviceResponse.IsSuccess = true;
            serviceResponse.Entities = response.Entities.Select(x => _mapper.Map<ProductQuestion, ProductQuestionDTO>(x)).ToList();
        }
        else
        {
            serviceResponse.IsSuccess = false;
        }
        serviceResponse.Message = response.Message;
        return serviceResponse;
    }

    // public async Task<ServiceResponse<Order>> GetOrdersByParameterAsync(Expression<Func<Order, bool>> predicate)
    // {
    //     ServiceResponse<Order> response = new ServiceResponse<Order>();
    //     
    //     //TODO use OrderService
    //     
    //     response.IsSuccess = false;
    //     return response;
    // }
}