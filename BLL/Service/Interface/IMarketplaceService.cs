using System.Security.Claims;
using BLL.Service.Model;
using BLL.Service.Model.DTO.Order;
using DAL.Repository.DTO;

namespace BLL.Service.Interface;

public interface IMarketplaceService
{
    public Task<ServiceResponse<MarketplaceProductView>> GetProductByIdAsync(int id);
    public Task<ServiceResponse<ProductCardView>> GetProductsDTOAsync(string searchQuery);
    public Task<ServiceResponse<MarketplaceProductView>> GetProductsAsync();
    public Task<ServiceResponse<CreateProductQuestion>> CreateProductQuestionAsync(CreateProductQuestion entity);
    public Task<ServiceResponse<CreateProductReview>> CreateProductReviewAsync(CreateProductReview entity);
    public Task<ServiceResponse> CreateOrderAsync(CreateOrder entity);
    
    public int GetUserIdFromClaims(ClaimsPrincipal user);
}