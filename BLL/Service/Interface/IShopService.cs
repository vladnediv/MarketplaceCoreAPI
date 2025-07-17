using System.Linq.Expressions;
using BLL.Service.Model;
using Domain.Model.Order;
using Domain.Model.Product;

namespace BLL.Service.Interface;
public interface IShopService
{
    

    public Task<ServiceResponse<Order>> GetOrdersByParameterAsync(Expression<Func<Order, bool>> predicate);

    
    public Task<ServiceResponse<Product>> CreateProductAsync(Product product);
    public Task<ServiceResponse<Product>> UpdateProductAsync(Product product);
    public Task<ServiceResponse<Product>> DeleteProductAsync(Product product);
    public Task<ServiceResponse<Product>> GetProductsByParameterAsync(Expression<Func<Product, bool>> predicate);
    
    
    public Task<ServiceResponse<ProductQuestionAnswer>> CreateProductQuestionAnswerAsync(ProductQuestionAnswer productQuestionAnswer);
    public Task<ServiceResponse<ProductReview>> GetProductReviewsByParameterAsync(Expression<Func<ProductReview, bool>> predicate);
    public Task<ServiceResponse<ProductQuestion>> GetProductQuestionsByParameterAsync(Expression<Func<ProductQuestion, bool>> predicate);
}