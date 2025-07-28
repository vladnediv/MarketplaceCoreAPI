using System.Linq.Expressions;
using BLL.Service.Interface;
using BLL.Service.Model;
using DAL.Repository.Interface;
using Domain.Model.Product;

namespace BLL.Service;

public class AdminService : IAdminService
{
    private readonly IProductService  _productService;
    private readonly IAdvancedService<DeliveryOption> _deliveryOptionService;
    private IAdminService _adminServiceImplementation;

    public AdminService(IProductService productService,
        IAdvancedService<DeliveryOption> deliveryOptionService)
    {
        _productService = productService;
        _deliveryOptionService = deliveryOptionService;
    }
    
    public async Task<ServiceResponse<Product>> ApproveProductAsync(int productId)
    {
        var prodRes = await _productService.GetAsync(productId);
        if (!prodRes.IsSuccess)
        {
            return prodRes;
        }
        prodRes.Entity.IsReviewed = true;
        prodRes.Entity.IsApproved = true;
        var updateRes = await _productService.UpdateAsync(prodRes.Entity);
        return updateRes;
    }

    public async Task<ServiceResponse<Product>> RejectProductAsync(int productId)
    {
        var prodRes = await _productService.GetAsync(productId);
        if (!prodRes.IsSuccess)
        {
            return prodRes;
        }
        prodRes.Entity.IsReviewed = true;
        prodRes.Entity.IsApproved = false;
        var updateRes = await _productService.UpdateAsync(prodRes.Entity);
        return updateRes;
    }

    public async Task<ServiceResponse<Product>> GetPendingProductsAsync()
    {
        var productsRes = await _productService.GetAllAsync(x => x.IsReviewed == false);
        return productsRes;
    }

    public async Task<ServiceResponse<Product>> GetProductsByParameter(Expression<Func<Product, bool>> predicate)
    {
        ServiceResponse<Product> res = await _productService.GetAllAsync(predicate);
        return res;
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

    public async Task<ServiceResponse<DeliveryOption>> CreateDeliveryOptionAsync(string deliveryOption)
    {
        DeliveryOption delivery = new DeliveryOption()
        {
            Name = deliveryOption
        };
        ServiceResponse<DeliveryOption> res = await _deliveryOptionService.CreateAsync(delivery);
        return res;
    }

    public async Task<ServiceResponse<DeliveryOption>> GetAllDeliveryOptionsAsync()
    {
        ServiceResponse<DeliveryOption> res = await _deliveryOptionService.GetAllAsync();
        return res;
    }
}