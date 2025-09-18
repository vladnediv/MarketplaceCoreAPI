using System.Security.Claims;
using BLL.Model;
using BLL.Model.DTO.Cart;
using BLL.Model.DTO.Category;
using BLL.Model.DTO.Order;
using BLL.Model.DTO.Product;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestion;
using BLL.Model.DTO.Product.IncludedModels.ProductReview;

namespace BLL.Service.Interface;

public interface IMarketplaceService
{
    public Task<ServiceResponse<MarketplaceProductView>> GetProductByIdAsync(int id);
    public Task<ServiceResponse<ProductCardView>> GetProductsDTOAsync(string searchQuery);
    public Task<ServiceResponse<MarketplaceProductView>> GetProductsAsync();
    public Task<ServiceResponse<MarketplaceProductView>> GetProductsByCategoryAsync(int categoryId);
    public Task<ServiceResponse<CreateProductQuestion>> CreateProductQuestionAsync(CreateProductQuestion entity);
    public Task<ServiceResponse<CreateProductReview>> CreateProductReviewAsync(CreateProductReview entity);

    public Task<ServiceResponse<ProductsFilter>> GetFilterAsync(string cacheName);
    
    public Task<ServiceResponse> UploadCartToUserAsync(List<CartItemDTO> cartItems, ClaimsPrincipal user);
    public Task<ServiceResponse<CartDTO>> GetCartAsync(ClaimsPrincipal user);
    public Task<ServiceResponse> AddItemToCartAsync(CartItemDTO cartItem, ClaimsPrincipal user);
    public Task<ServiceResponse> RemoveItemFromCartAsync(CartItemDTO cartItem, ClaimsPrincipal user);
    
    public int GetUserIdFromClaims(ClaimsPrincipal user);
    
    public Task<ServiceResponse<CategoryDTO>> GetSubcategoriesAsync(int parentCategoryId);
    public Task<ServiceResponse<CategoryDTO>> GetCategoryTreeAsync();
    public Task<ServiceResponse<CategoryDTO>> GetRootCategoriesAsync();
    
    public Task<ServiceResponse<MarketplaceOrderView>> CreateOrderAsync(CreateOrder entity);
    public Task<ServiceResponse<MarketplaceOrderView>> GetOrderByIdAsync(int id, ClaimsPrincipal user);
    public Task<ServiceResponse<MarketplaceOrderView>> GetUserOrdersAsync(ClaimsPrincipal user);
}