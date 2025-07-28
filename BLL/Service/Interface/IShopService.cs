using System.Linq.Expressions;
using BLL.Service.Model;
using DAL.Repository.DTO;
using Domain.Model.Order;
using Domain.Model.Product;

namespace BLL.Service.Interface;
public interface IShopService
{
    

    //public Task<ServiceResponse<Order>> GetOrdersByParameterAsync(Expression<Func<Order, bool>> predicate);

    
    public Task<ServiceResponse<CreateProduct>> CreateProductAsync(CreateProduct product);
    public Task<ServiceResponse<UpdateProduct>> UpdateProductAsync(UpdateProduct product);
    public Task<ServiceResponse<object>> DeleteProductByIdAsync(int id, int userId);
    public Task<ServiceResponse<ShopProductView>> GetProductByIdAsync(int id);
    public Task<ServiceResponse<ShopProductView>> GetProductsByParameterAsync(Expression<Func<Product, bool>> predicate);
    
    //TODO method to group products
    //public Task<ServiceResponse<object>> GroupProductsByVariation(List<int> productIds);
    
    
    public Task<ServiceResponse<CreateProductQuestionAnswer>> CreateProductQuestionAnswerAsync(CreateProductQuestionAnswer productQuestionAnswer);
    public Task<ServiceResponse<ProductReviewDTO>> GetProductReviewsByParameterAsync(Expression<Func<ProductReview, bool>> predicate);
    public Task<ServiceResponse<ProductQuestionDTO>> GetProductQuestionsByParameterAsync(Expression<Func<ProductQuestion, bool>> predicate);
}