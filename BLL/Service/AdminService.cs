using System.Linq.Expressions;
using BLL.Service.Interface;
using BLL.Service.Model;
using DAL.Repository.DTO;
using DAL.Repository.Interface;
using Domain.Model.Product;

namespace BLL.Service;

public class AdminService : IAdminService
{
    private readonly IProductService  _productService;
    private readonly IAdvancedService<DeliveryOption> _deliveryOptionService;

    public AdminService(IProductService productService,
        IAdvancedService<DeliveryOption> deliveryOptionService)
    {
        _productService = productService;
        _deliveryOptionService = deliveryOptionService;
    }


    public async Task<ServiceResponse> EditProductApprovedStatusAsync(int productId, bool isApproved)
    {
        ServiceResponse serviceResponse = new ServiceResponse();
        
        var product = await _productService.GetAsync(productId);

        if (!product.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = product.Message;
            
            return serviceResponse;
        }
        
        product.Entity.IsReviewed = true;
        product.Entity.IsApproved = isApproved;
        
        var updateRes = await _productService.UpdateAsync(product.Entity);
        
        if (!updateRes.IsSuccess)
        {
            serviceResponse.IsSuccess = false;
            serviceResponse.Message = updateRes.Message;
            
            return serviceResponse;
        }
        
        serviceResponse.IsSuccess = true;
        return serviceResponse;
    }

    public async Task<ServiceResponse<AdminProductView>> GetProductsByParameter(Expression<Func<Product, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse> DeleteProductAsync(int productId)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse> EditProductReviewApprovedStatusAsync(int reviewId, bool isApproved)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductReviewDTO>> GetProductReviewsByParameterAsync(Expression<Func<ProductReview, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse> EditProductQuestionApprovedStatusAsync(int reviewId, bool isApproved)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse<ProductQuestionDTO>> GetProductQuestionsByParameterAsync(Expression<Func<ProductQuestion, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse> AnswerProductQuestionAsync(int questionId, string answerText)
    {
        throw new NotImplementedException();
    }

    public async Task<ServiceResponse> CreateDeliveryOptionAsync(string deliveryOption, decimal price)
    {
        var res = await _deliveryOptionService.CreateAsync(new DeliveryOption() { Name = deliveryOption, Price = price });

        return new ServiceResponse()
        {
            IsSuccess = res.IsSuccess,
            Message = res.Message
        };
    }

    public async Task<ServiceResponse<DeliveryOption>> GetAllDeliveryOptionsAsync()
    {
        ServiceResponse<DeliveryOption> res = await _deliveryOptionService.GetAllAsync();
        return res;
    }
}