using System.Linq.Expressions;
using BLL.Service.Model;
using Domain.Model.Order;
using Domain.Model.Product;

namespace BLL.Service.Interface;

public interface IAdminService
{
    // Product management
    public Task<ServiceResponse<Product>> ApproveProductAsync(int productId);
    public Task<ServiceResponse<Product>> RejectProductAsync(int productId, string reason);
    public Task<ServiceResponse<Product>> GetPendingProductsAsync();
    public Task<ServiceResponse<Product>> GetProductsByParameter(Expression<Func<Product, bool>> predicate);

    // Review management
    public Task<ServiceResponse<ProductReview>> ApproveReviewAsync(int reviewId);
    public Task<ServiceResponse<ProductReview>> RejectReviewAsync(int reviewId, string reason);
    public Task<ServiceResponse<ProductReview>> GetFlaggedReviewsAsync();

    // Question management
    public Task<ServiceResponse<ProductQuestion>> GetUnansweredQuestionsAsync();
    public Task<ServiceResponse<ProductQuestionAnswer>> AnswerProductQuestionAsync(int questionId, string answerText);

    //TODO Order management
}