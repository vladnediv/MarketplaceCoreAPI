using System.Linq.Expressions;
using BLL.Service.Interface;
using BLL.Service.Model;
using DAL.Context;
using DAL.Repository;
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

    public ShopService(
        IProductService productService,
        IGenericService<ProductQuestionAnswer> questionAnswerService,
        IAdvancedService<ProductReview> reviewService,
        IAdvancedService<ProductQuestion> questionService)
    {
        _productService = productService;
        _questionAnswerService = questionAnswerService;
        _reviewService = reviewService;
        _questionService = questionService;
    }

    public async Task<ServiceResponse<Product>> CreateProductAsync(Product product)
    {
        ServiceResponse<Product> response = await _productService.CreateAsync(product);
        return response;
    }

    public async Task<ServiceResponse<Product>> UpdateProductAsync(Product product)
    {
        ServiceResponse<Product> response = await _productService.UpdateAsync(product);
        return response;
    }

    public async Task<ServiceResponse<Product>> DeleteProductAsync(Product product)
    {
        ServiceResponse<Product> response = await _productService.DeleteAsync(product);
        return response;
    }

    public async Task<ServiceResponse<Product>> DeleteProductByIdAsync(int id)
    {
        ServiceResponse<Product> res = await _productService.DeleteByIdAsync(id);
        return res;
    }

    public async Task<ServiceResponse<Product>> GetProductByIdAsync(int id)
    {
        ServiceResponse<Product> res = await _productService.GetAsync(id);
        return res;
    }

    public async Task<ServiceResponse<Product>> GetProductsByParameterAsync(Expression<Func<Product, bool>> predicate)
    {
        ServiceResponse<Product> response = await _productService.GetAllAsync(predicate);
        return response;
    }

    public async Task<ServiceResponse<ProductQuestionAnswer>> CreateProductQuestionAnswerAsync(ProductQuestionAnswer productQuestionAnswer)
    {
        ServiceResponse<ProductQuestionAnswer> response = await _questionAnswerService.CreateAsync(productQuestionAnswer);
        return response;
    }

    public async Task<ServiceResponse<ProductReview>> GetProductReviewsByParameterAsync(Expression<Func<ProductReview, bool>> predicate)
    {
        ServiceResponse<ProductReview> response = await _reviewService.GetAllAsync(predicate);
        return response;
    }

    public async Task<ServiceResponse<ProductQuestion>> GetProductQuestionsByParameterAsync(Expression<Func<ProductQuestion, bool>> predicate)
    {
        ServiceResponse<ProductQuestion> response = await _questionService.GetAllAsync(predicate);
        return response;
    }

    public async Task<ServiceResponse<Order>> GetOrdersByParameterAsync(Expression<Func<Order, bool>> predicate)
    {
        ServiceResponse<Order> response = new ServiceResponse<Order>();
        
        //TODO use OrderService
        
        response.IsSuccess = false;
        return response;
    }
}