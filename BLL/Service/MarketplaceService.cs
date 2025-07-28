using AutoMapper;
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
    private readonly IMapper _mapper;

    public MarketplaceService(IProductService productService,
        IGenericService<ProductQuestion> productQuestionService,
        IGenericService<ProductReview> productReviewService,
        IMapper mapper)
    {
        _productService = productService;
        _productQuestionService = productQuestionService;
        _productReviewService = productReviewService;
        _mapper = mapper;
    }
    
        
    public async Task<ServiceResponse<MarketplaceProductView>> GetProductByIdAsync(int id)
    {
        ServiceResponse<Product> productResponse = await _productService.GetAsync(id);
        ServiceResponse<MarketplaceProductView> apiResponse = new ServiceResponse<MarketplaceProductView>();
        if (productResponse.IsSuccess)
        {
            apiResponse.IsSuccess = true;
            apiResponse.Entity = _mapper.Map<MarketplaceProductView>(productResponse.Entity);
        }
        else
        {
            apiResponse.IsSuccess = false;
        }
        
        return apiResponse;
    }

    public async Task<ServiceResponse<ProductCardView>> GetProductsDTOAsync(string searchQuery)
    {
        ServiceResponse<ProductCardView> response = await _productService.GetProductCards(searchQuery);
        return response;
    }

    public async Task<ServiceResponse<MarketplaceProductView>> GetProductsAsync()
    {
        ServiceResponse<Product> products = await _productService.GetAllAsync();
        
        ServiceResponse<MarketplaceProductView> apiResponse = new ServiceResponse<MarketplaceProductView>();
        if (products.IsSuccess)
        {
            apiResponse.IsSuccess = true;
            apiResponse.Entities = products.Entities.Select(x => _mapper.Map<MarketplaceProductView>(x)).ToList();
        }
        else
        {
            apiResponse.IsSuccess = false;
            apiResponse.Message = products.Message;
        }
        return apiResponse;
    }

    public async Task<ServiceResponse<CreateProductQuestion>> CreateProductQuestionAsync(CreateProductQuestion entity)
    {
        ProductQuestion productQuestion = _mapper.Map<ProductQuestion>(entity);
        ServiceResponse<ProductQuestion> serviceResponse = await _productQuestionService.CreateAsync(productQuestion);
        ServiceResponse<CreateProductQuestion> apiResponse = new ServiceResponse<CreateProductQuestion>();
        if (serviceResponse.IsSuccess)
        {
            apiResponse.IsSuccess = true;
        }
        else
        {
            apiResponse.IsSuccess = false;
        }
        //TODO Notify shop about new question
        return apiResponse;
    }

    public async Task<ServiceResponse<CreateProductReview>> CreateProductReviewAsync(CreateProductReview entity)
    {
        ProductReview productReview = _mapper.Map<ProductReview>(entity);
        ServiceResponse<ProductReview> response = await _productReviewService.CreateAsync(productReview);
        ServiceResponse<CreateProductReview> apiResponse = new ServiceResponse<CreateProductReview>();
        if (response.IsSuccess)
        {
            apiResponse.IsSuccess = true;
        }
        else
        {
            apiResponse.IsSuccess = false;
        }
        //TODO Notify shop about new review
        return apiResponse;
    }
}