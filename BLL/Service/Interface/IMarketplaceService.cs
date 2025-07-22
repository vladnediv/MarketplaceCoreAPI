using BLL.Service.Model;
using DAL.Repository.DTO;
using Domain.Model.Order;
using Domain.Model.Product;

namespace BLL.Service.Interface;

public interface IMarketplaceService
{
    public Task<ServiceResponse<MarketplaceProductView>> GetProductByIdAsync(int id);
    public Task<ServiceResponse<ProductCardView>> GetProductsDTOAsync(string searchQuery);
    public Task<ServiceResponse<CreateProductQuestion>> CreateProductQuestionAsync(CreateProductQuestion entity);
    public Task<ServiceResponse<CreateProductReview>> CreateProductReviewAsync(CreateProductReview entity);
}