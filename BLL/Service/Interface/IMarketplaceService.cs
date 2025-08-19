using System.Security.Claims;
using BLL.Service.Model;
using BLL.Service.Model.DTO.Cart;
using DAL.Repository.DTO;

namespace BLL.Service.Interface;

public interface IMarketplaceService
{
    public Task<ServiceResponse<MarketplaceProductView>> GetProductByIdAsync(int id);
    public Task<ServiceResponse<ProductCardView>> GetProductsDTOAsync(string searchQuery);
    public Task<ServiceResponse<MarketplaceProductView>> GetProductsAsync();
    public Task<ServiceResponse<CreateProductQuestion>> CreateProductQuestionAsync(CreateProductQuestion entity);
    public Task<ServiceResponse<CreateProductReview>> CreateProductReviewAsync(CreateProductReview entity);
    
    public Task<ServiceResponse> UploadCartToUserAsync(List<CartItemDTO> cartItems, ClaimsPrincipal user);
    
    public Task<ServiceResponse<CartDTO>> GetCartAsync(ClaimsPrincipal user);
    public Task<ServiceResponse> AddItemToCartAsync(CartItemDTO cartItem, ClaimsPrincipal user);
    public Task<ServiceResponse> RemoveItemFromCartAsync(CartItemDTO cartItem, ClaimsPrincipal user);
    
    public int GetUserIdFromClaims(ClaimsPrincipal user);
}