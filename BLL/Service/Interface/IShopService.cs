using System.Linq.Expressions;
using System.Security.Claims;
using BLL.Model;
using BLL.Model.DTO.Category;
using BLL.Model.DTO.Order;
using BLL.Model.DTO.Product;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestion;
using BLL.Model.DTO.Product.IncludedModels.ProductQuestionAnswer;
using BLL.Model.DTO.Product.IncludedModels.ProductReview;
using Domain.Model.Order;
using Domain.Model.Product;

namespace BLL.Service.Interface;
public interface IShopService
{
    public Task<ServiceResponse> CreateProductAsync(CreateProduct product);
    public Task<ServiceResponse> UpdateProductAsync(UpdateProduct product);
    public Task<ServiceResponse> DeleteProductByIdAsync(int id, int userId);
    public Task<ServiceResponse<ShopProductView>> GetProductByIdAsync(int id);
    public Task<ServiceResponse<ShopProductView>> GetProductsByParameterAsync(Expression<Func<Product, bool>> predicate);
    public Task<ServiceResponse> EditProductStatusAsync(int productId, int shopId, ProductStatus status);
    
    
    public Task<ServiceResponse<CreateProductQuestionAnswer>> CreateProductQuestionAnswerAsync(CreateProductQuestionAnswer productQuestionAnswer);
    public Task<ServiceResponse<ShopProductReviewView>> GetProductReviewsByParameterAsync(Expression<Func<ProductReview, bool>> predicate);
    public Task<ServiceResponse<ProductQuestionDTO>> GetProductQuestionsByParameterAsync(Expression<Func<ProductQuestion, bool>> predicate);
    public Task<ServiceResponse> EditProductActiveStatusAsync(int productId, int userId, bool isActive);

    
    public Task<ServiceResponse<ShopOrderView>> GetShopOrdersAsync(ClaimsPrincipal user, OrderStatus? status, PaymentMethod? paymentMethod);
    public Task<ServiceResponse<ShopOrderView>> GetOrderByIdAsync(int id, ClaimsPrincipal user);
    public Task<ServiceResponse> EditOrderStatusAsync(int orderId, OrderStatus status);
    public Task<ServiceResponse> CheckOrderUpdatePermission(ClaimsPrincipal user, int orderId);
    public Task<ServiceResponse> EditDeliveryStatusAsync(int orderItemId, int shopId, DeliveryStatus status);
        
    
    public Task<ServiceResponse<CategoryDTO>> GetSubcategoriesAsync(int parentCategoryId);
    public Task<ServiceResponse<CategoryDTO>> GetCategoryTreeAsync();
    
    public int GetUserIdFromClaims(ClaimsPrincipal user);
}