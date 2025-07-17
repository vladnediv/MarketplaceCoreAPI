using BLL.Service.Interface;
using BLL.Service.Model;
using DAL.Repository.DTO;
using DAL.Repository.Interface;
using Domain.Model.Product;

namespace BLL.Service;

public class MarketplaceService : IMarketplaceService
{
    private readonly IProductService _productService;
    private readonly IGenericService<ProductQuestion> _productQuestionService;
    private readonly IGenericService<ProductReview> _productReviewService;

    public MarketplaceService(IProductService productService,
        IGenericService<ProductQuestion> productQuestionService,
        IGenericService<ProductReview> productReviewService)
    {
        _productService = productService;
        _productQuestionService = productQuestionService;
        _productReviewService = productReviewService;
    }
    
        
    public async Task<ServiceResponse<Product>> GetProductByIdAsync(int id)
    {
        ServiceResponse<Product> response = await _productService.GetAsync(id);
        return response;
    }

    public async Task<ServiceResponse<ProductCardDTO>> GetProductsDTOAsync(string searchQuery)
    {
        ServiceResponse<ProductCardDTO> response = await _productService.GetProductsDTOAsync(searchQuery);
        return response;
    }

    public async Task<ServiceResponse<ProductQuestion>> CreateProductQuestionAsync(ProductQuestion entity)
    {
        ServiceResponse<ProductQuestion> response = await _productQuestionService.CreateAsync(entity);
        //TODO Notify shop about new question
        return response;
    }

    public async Task<ServiceResponse<ProductReview>> CreateProductReviewAsync(ProductReview entity)
    {
        ServiceResponse<ProductReview> response = await _productReviewService.CreateAsync(entity);
        //TODO Notify shop about new review
        return response;
    }
}