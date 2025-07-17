using BLL.Service.Model;
using DAL.Repository.DTO;
using Domain.Model.Order;
using Domain.Model.Product;

namespace BLL.Service.Interface;

public interface IMarketplaceService
{
    public Task<ServiceResponse<Product>> GetProductByIdAsync(int id);
    public Task<ServiceResponse<ProductCardDTO>> GetProductsDTOAsync(string searchQuery);
    public Task<ServiceResponse<ProductQuestion>> CreateProductQuestionAsync(ProductQuestion entity);
    public Task<ServiceResponse<ProductReview>> CreateProductReviewAsync(ProductReview entity);
}