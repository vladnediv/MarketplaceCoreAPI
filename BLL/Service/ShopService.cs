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
        //IAdvancedService<Order> orderService,
        IAdvancedService<ProductReview> reviewService,
        IAdvancedService<ProductQuestion> questionService)
    {
        _productService = productService;
        _questionAnswerService = questionAnswerService;
        //_orderService = orderService;
        _reviewService = reviewService;
        _questionService = questionService;
    }


    public async Task<ServiceResponse<Order>> GetOrdersByParameterAsync(Expression<Func<Order, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Product>> CreateProductAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Product>> UpdateProductAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Product>> DeleteProductAsync(Product product)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Product>> GetProductsByParameterAsync(Expression<Func<Product, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductQuestionAnswer>> CreateProductQuestionAnswerAsync(ProductQuestionAnswer productQuestionAnswer)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductReview>> GetProductReviewsByParameterAsync(Expression<Func<ProductReview, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductQuestion>> GetProductQuestionsByParameterAsync(Expression<Func<ProductQuestion, bool>> predicate)
    {
        throw new NotImplementedException();
    }
}