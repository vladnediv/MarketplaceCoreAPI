using System.Linq.Expressions;
using BLL.Service.Model;
using DAL.Repository.DTO;
using Domain.Model.Order;
using Domain.Model.Product;

namespace BLL.Service.Interface;

//TODO Think good about the functionality here
public interface IAdminService
{
    // Product management
    public Task<ServiceResponse> EditProductApprovedStatusAsync(int productId, bool isApproved);
    public Task<ServiceResponse<AdminProductView>> GetProductsByParameter(Expression<Func<Product, bool>> predicate);
    public Task<ServiceResponse> DeleteProductAsync(int productId);

    // Review management
    public Task<ServiceResponse> EditProductReviewApprovedStatusAsync(int reviewId, bool isApproved);
    public Task<ServiceResponse<ProductReviewDTO>> GetProductReviewsByParameterAsync(Expression<Func<ProductReview, bool>> predicate);

    // Question management
    public Task<ServiceResponse> EditProductQuestionApprovedStatusAsync(int reviewId, bool isApproved);
    public Task<ServiceResponse<ProductQuestionDTO>> GetProductQuestionsByParameterAsync(Expression<Func<ProductQuestion, bool>> predicate);
    public Task<ServiceResponse> AnswerProductQuestionAsync(int questionId, string answerText);

    //Delivery Management
    public Task<ServiceResponse> CreateDeliveryOptionAsync(string deliveryOption, decimal price);
    public Task<ServiceResponse<DeliveryOption>> GetAllDeliveryOptionsAsync();
    
    
    //TODO Order management
}