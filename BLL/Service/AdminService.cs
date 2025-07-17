using System.Linq.Expressions;
using BLL.Service.Interface;
using BLL.Service.Model;
using Domain.Model.Product;

namespace BLL.Service;

public class AdminService : IAdminService
{
    public async Task<ServiceResponse<Product>> ApproveProductAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Product>> RejectProductAsync(int productId, string reason)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Product>> GetPendingProductsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<Product>> GetProductsByParameter(Expression<Func<Product, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductReview>> ApproveReviewAsync(int reviewId)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductReview>> RejectReviewAsync(int reviewId, string reason)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductReview>> GetFlaggedReviewsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductQuestion>> GetUnansweredQuestionsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductQuestionAnswer>> AnswerProductQuestionAsync(int questionId, string answerText)
    {
        throw new NotImplementedException();
    }
}